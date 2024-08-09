using Ozzyria.Content.Models.Area;
using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Event;
using Ozzyria.Gryp.UI.Dialogs;
using SkiaSharp.Views.Desktop;
using System.Reflection;

namespace Ozzyria.Gryp
{
    public partial class MainGrypWindow : Form, IEventSubscriber<BrushChangeEvent>, IEventSubscriber<SelectedEntityChangeEvent>, IEventSubscriber<ActiveLayerChangedEvent>
    {
        internal Map _map = new Map();

        internal string _lastSelectedPreset = "";
        internal bool _processingThumbnails = false;

        public MainGrypWindow()
        {
            InitializeComponent();
            mainToolbelt.AttachMap(_map);
            mapViewPort.AttachMap(_map);
            mapViewPort.ResetCamera();

            EventBus.Subscribe(this);

            cmbPrefab.Items.AddRange(new string[] {
                "slime_spawner",
                "door",
                "exp_orb",
            });

            // hackity hack to override DoubleBuffered without making custom class
            typeof(ListView).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, layerList, new object[] { true });
        }

        #region Menu Strip
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUnsavedChanges();

            var mapDialog = new NewMapDialog();
            if (mapDialog.ShowDialog() == DialogResult.OK)
            {
                _map.MetaData.AreaId = mapDialog.NewMapResult.Id;
                _map.MetaData.DisplayName = mapDialog.NewMapResult.DisplayName;
                _map.MetaData.CreatedAt = DateTime.Now;
                _map.MetaData.UpdatedAt = DateTime.Now;

                _map.Width = mapDialog.NewMapResult.Width;
                _map.Height = mapDialog.NewMapResult.Height;

                _map.Layers.Clear();
                _map.PushLayer();
                _map.PushLayer();

                RebuildLayerView();
                RebuildBrushView();

                // Center Camera onto Map
                mapViewPort.CenterOnWorldCoordinate(_map.Width * 32 / 2f, _map.Height * 32 / 2f);

                mapViewPort.Focus();

                ChangeHistory.Clear();
                mainStatusLabel.Text = "Successfully created map";
            }
            else
            {
                mainStatusLabel.Text = "Canceled";
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUnsavedChanges();

            var mapDialog = new OpenMapDialog();
            if (mapDialog.ShowDialog() == DialogResult.OK)
            {
                var areaId = mapDialog.AreaId;
                var areaData = AreaData.Retrieve(areaId);

                if (areaData != null && areaData.TileData != null)
                {
                    _map.FromAreaData(areaData);

                    RebuildLayerView();
                    RebuildBrushView();

                    // Center Camera onto Map
                    mapViewPort.CenterOnWorldCoordinate(_map.Width * 32 / 2f, _map.Height * 32 / 2f);
                    mainStatusLabel.Text = "Successfully loaded " + areaId;

                    ChangeHistory.Clear();
                    mapViewPort.Focus();
                }
                else if (areaData != null & areaData?.TileData == null)
                {
                    mainStatusLabel.Text = "Missing TileData for " + areaId;
                }
                else
                {
                    mainStatusLabel.Text = "Failed to load " + areaId;
                }
            }
            else
            {
                mainStatusLabel.Text = "Canceled";
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_map.MetaData.AreaId != "" && _map.IsDirty)
            {
                _map.ToAreaData().Store(_map.MetaData.AreaId);
                mainStatusLabel.Text = "Saved Map " + _map.MetaData.AreaId;

                mapViewPort.Focus();
            }
            else
            {
                mainStatusLabel.Text = "Nothing to save..";
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeHistory.Undo(_map);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeHistory.Redo(_map);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainGrypWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckForUnsavedChanges();
        }

        private void CheckForUnsavedChanges()
        {
            if (_map.IsDirty)
            {
                var result = MessageBox.Show(
                    "You have unsaved changes.\r\nWould you like to save?",
                    "Unsaved Changes!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2 // default to no in-case user enters real hard
                );

                if (result == DialogResult.Yes)
                {
                    _map.ToAreaData().Store(_map.MetaData.AreaId);
                }
            }
        }

        #endregion

        #region MapViewPort routing
        private void MainGrypWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.Alt || e.KeyCode == Keys.Menu)
            {
                e.Handled = true;
                EventBus.Notify(new SwitchToDropperEvent { });
            }
        }

        private void MainGrypWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ChangeHistory.StartTracking();
                _map.RemoveSelectedEntity();
                _map.RemoveSelectedWall();
                ChangeHistory.FinishTracking();
            }

            if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.Alt || e.KeyCode == Keys.Menu)
            {
                e.Handled = true;
                EventBus.Notify(new UnswitchFromDropperEvent { });
            }
        }

        private void reRenderTimer_Tick(object sender, EventArgs e)
        {
            // force a timely rerender
            mapViewPort.Invalidate();
        }
        #endregion

        #region Layer View
        private void RebuildLayerView()
        {
            var selectedIndices = layerList.SelectedIndices.Cast<int>().ToArray();

            layerImageList.Images.Clear();
            layerList.Items.Clear();

            for (int i = 0; i < _map.Layers.Count; i++)
            {
                layerImageList.Images.Add(_map.Layers[i].GetThumbnail(layerImageList.ImageSize.Width).ToBitmap());
                var layerName = _map.IsLayerVisible(_map.ActiveLayer) ? ("Layer " + i) : ("*Layer " + i);
                layerList.Items.Add(new ListViewItem { Text = layerName, ImageIndex = i });
            }

            foreach (var index in selectedIndices)
            {
                if (index >= 0 && index < layerList.Items.Count)
                {
                    layerList.SelectedIndices.Add(index);
                }
            }

            if (layerList.SelectedIndices.Count <= 0 && layerList.Items.Count > 0)
            {
                // if no items were re-selected and there are items, select the first in the list
                layerList.Items[0].Selected = true;
            }
        }

        private void logicTimer_Tick(object sender, EventArgs e)
        {
            // Check if thumbnails need refreshed
            if (!_processingThumbnails)
            {
                _processingThumbnails = true;
                Task.Run(() =>
                {
                    bool refreshLayers = false;
                    for (int i = 0; i < _map.Layers.Count; i++)
                    {
                        if (i < layerImageList.Images.Count && _map.Layers[i].HasChanged())
                        {
                            refreshLayers = true;
                            layerImageList.Images[i] = _map.Layers[i].GetThumbnail(layerImageList.ImageSize.Width).ToBitmap();
                        }
                    }

                    if (refreshLayers)
                    {
                        layerList.Invalidate();
                    }

                    _processingThumbnails = false;
                });
            }
        }

        private void layerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentLayer = _map.ActiveLayer;
            if (layerList?.SelectedIndices?.Count <= 0)
            {
                // ListItemView's signal index changes twice, once for clearing and once for re-selecting when set to MultiSelect=false
                return;
            }

            ChangeHistory.StartTracking();
            _map.ActiveLayer = layerList?.SelectedIndices[0] ?? 0;
            btnHideShowLayer.Text = _map.IsLayerVisible(_map.ActiveLayer) ? "Hide" : "Show";
            if (_map.ActiveLayer != currentLayer && currentLayer >= 0)
            {
                EventBus.Notify(new ActiveLayerChangedEvent { });
                ChangeHistory.TrackChange(new LayerChange { Layer = currentLayer });
                _map.UnselectEntity();
                _map.UnselectWall();
            }

            ChangeHistory.FinishTracking();
        }

        private void btnNewLayer_Click(object sender, EventArgs e)
        {
            if (_map.Width > 0 && _map.Height > 0)
            {
                _map.PushLayer();
                RebuildLayerView();
                mainStatusLabel.Text = "Layer added";
            }
        }

        private void btnRemoveLayer_Click(object sender, EventArgs e)
        {
            var rebuildLayerView = false;
            foreach (ListViewItem selectItem in layerList.SelectedItems.Cast<ListViewItem>().OrderByDescending(e => e.Index))
            {
                rebuildLayerView = true;
                _map.Layers.RemoveAt(selectItem.Index);
            }

            if (rebuildLayerView)
            {
                RebuildLayerView();
                mainStatusLabel.Text = "Layer(s) removed";
            }
        }

        private void btnHideShowLayer_Click(object sender, EventArgs e)
        {
            if (_map == null || _map.Width <= 0 || _map.Height <= 0 || _map.ActiveLayer < 0)
            {
                return;
            }

            if (_map.IsLayerVisible(_map.ActiveLayer))
            {
                _map.IsLayerHidden[_map.ActiveLayer] = true;
                layerList.Items[_map.ActiveLayer].Text = "*Layer " + _map.ActiveLayer;
                btnHideShowLayer.Text = "Show";
                mainStatusLabel.Text = "Layer " + _map.ActiveLayer + " hidden";
            }
            else
            {
                _map.IsLayerHidden[_map.ActiveLayer] = false;
                layerList.Items[_map.ActiveLayer].Text = "Layer " + _map.ActiveLayer;
                btnHideShowLayer.Text = "Hide";
                mainStatusLabel.Text = "Layer " + _map.ActiveLayer + " shown";
            }

        }

        void IEventSubscriber<ActiveLayerChangedEvent>.OnNotify(ActiveLayerChangedEvent e)
        {
            if (layerList.SelectedIndices.Count > 0 && !layerList.SelectedIndices.Contains(_map.ActiveLayer))
            {
                var i = 0;
                foreach (ListViewItem item in layerList.Items)
                {
                    if (i == _map.ActiveLayer)
                    {
                        item.Selected = true;
                        break;
                    }
                    i++;
                }
            }
        }
        #endregion

        #region Brush View
        private void listCurrentBrush_DoubleClick(object sender, EventArgs e)
        {
            if (_map == null || _map.Width <= 0 || _map.Height <= 0 || _map.ActiveLayer < 0)
            {
                return;
            }

            var selectedIndex = listCurrentBrush.SelectedIndices.Cast<int>().FirstOrDefault();
            if (selectedIndex >= _map.CurrentBrush.Count() || selectedIndex < 0)
            {
                return;
            }

            var currentDrawableId = _map.CurrentBrush[selectedIndex];

            var textureDialog = new EditTextureDialog(currentDrawableId);
            if (textureDialog.ShowDialog() == DialogResult.OK && textureDialog.TextureResult != "")
            {
                _map.CurrentBrush[selectedIndex] = textureDialog.TextureResult;
                EventBus.Notify(new BrushChangeEvent { });

                mainStatusLabel.Text = "Successfully edited texture";
            }
            else
            {
                mainStatusLabel.Text = "Texture edit canceled";
            }
        }

        private void btnAddBrush_Click(object sender, EventArgs e)
        {
            if (_map == null || _map.Width <= 0 || _map.Height <= 0 || _map.ActiveLayer < 0)
            {
                return;
            }

            _map.CurrentBrush.Add("grass");
            EventBus.Notify(new BrushChangeEvent { });
        }

        private void btnRemoveBrush_Click(object sender, EventArgs e)
        {
            if (_map == null || _map.Width <= 0 || _map.Height <= 0 || _map.ActiveLayer < 0)
            {
                return;
            }

            var brushChanged = false;
            foreach (ListViewItem selectItem in listCurrentBrush.SelectedItems.Cast<ListViewItem>().OrderByDescending(e => e.Index))
            {
                brushChanged = true;
                _map.CurrentBrush.RemoveAt(selectItem.Index);
            }

            if (brushChanged)
            {
                EventBus.Notify(new BrushChangeEvent { });
                mainStatusLabel.Text = "Brush(s) removed";
            }
        }

        private void RebuildBrushView()
        {
            var selectedIndices = listCurrentBrush.SelectedIndices.Cast<int>().ToArray();
            listCurrentBrush.Items.Clear();
            foreach (var id in _map.CurrentBrush)
            {
                listCurrentBrush.Items.Add(new ListViewItem
                {
                    Text = $"{id}"
                });
            }

            foreach (var index in selectedIndices)
            {
                if (index >= 0 && index < listCurrentBrush.Items.Count)
                {
                    listCurrentBrush.SelectedIndices.Add(index);
                }
            }

            if (listCurrentBrush.SelectedIndices.Count <= 0 && listCurrentBrush.Items.Count > 0)
            {
                // if no items were re-selected and there are items, select the first in the list
                listCurrentBrush.SelectedIndices.Add(0);
            }
        }

        private void btnBrushPreset_Click(object sender, EventArgs e)
        {
            if (_map == null || _map.Width <= 0 || _map.Height <= 0 || _map.ActiveLayer < 0)
            {
                return;
            }

            var presetDialog = new BrushPresetDialog(_lastSelectedPreset);
            if (presetDialog.ShowDialog() == DialogResult.OK)
            {
                _lastSelectedPreset = presetDialog.SelectedPreset;
                _map.CurrentBrush.Clear();
                _map.CurrentBrush.AddRange(presetDialog.PresetResult);
                EventBus.Notify(new BrushChangeEvent { });

                mainStatusLabel.Text = "Brush preset selected";
            }
            else
            {
                mainStatusLabel.Text = "Preset selection canceled";
            }
        }

        void IEventSubscriber<BrushChangeEvent>.OnNotify(BrushChangeEvent e)
        {
            if (listCurrentBrush.Items.Count == _map.CurrentBrush.Count && _map.CurrentBrush.Count > 0)
            {
                // check if brush and brush view match
                bool match = true;
                for (int i = 0; i < _map.CurrentBrush.Count; i++)
                {
                    if (_map.CurrentBrush[i] != listCurrentBrush.Items[i].Text)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    // don't rebuild if there is no change
                    return;
                }
            }

            RebuildBrushView();
        }
        #endregion

        #region Entity View
        private void cmbPrefab_SelectedIndexChanged(object sender, EventArgs e)
        {
            var newPrefabId = cmbPrefab?.SelectedItem?.ToString() ?? "";
            if (_map.CurrentEntityBrush.Attributes == null)
                _map.CurrentEntityBrush.Attributes = new Dictionary<string, string>();
            else if (_map.CurrentEntityBrush.PrefabId != newPrefabId)
                _map.CurrentEntityBrush.Attributes.Clear();
            _map.CurrentEntityBrush.PrefabId = newPrefabId;

            tableEntityAttributes.Rows.Clear();
            switch (_map.CurrentEntityBrush.PrefabId)
            {
                case "slime_spawner":
                    break;
                case "door":
                    tableEntityAttributes.Rows.Add(new string[] { "new_area_id", _map.CurrentEntityBrush.Attributes.GetValueOrDefault("new_area_id") ?? "" });
                    tableEntityAttributes.Rows.Add(new string[] { "new_area_x", _map.CurrentEntityBrush.Attributes.GetValueOrDefault("new_area_x") ?? "" });
                    tableEntityAttributes.Rows.Add(new string[] { "new_area_y", _map.CurrentEntityBrush.Attributes.GetValueOrDefault("new_area_y") ?? "" });
                    break;
                case "exp_orb":
                    tableEntityAttributes.Rows.Add(new string[] { "amount", _map.CurrentEntityBrush.Attributes.GetValueOrDefault("amount") ?? "" });
                    break;
            }

            if (_map.SelectedEntity != null && _map.SelectedEntity.PrefabId != _map.CurrentEntityBrush.PrefabId)
            {
                ChangeHistory.StartTracking();
                ChangeHistory.TrackChange(new EditEntityChange
                {
                    InternalId = _map.SelectedEntity.InternalId,
                    PrefabId = _map.SelectedEntity.PrefabId,
                    WorldX = _map.SelectedEntity.WorldX,
                    WorldY = _map.SelectedEntity.WorldY,
                    Attributes = _map.SelectedEntity.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>(),
                });

                _map.IsDirty = true;
                _map.SelectedEntity.PrefabId = _map.CurrentEntityBrush.PrefabId;
                _map.SelectedEntity.Attributes = _map.CurrentEntityBrush.Attributes.ToDictionary(kv => kv.Key, kv => kv.Value);
                ChangeHistory.FinishTracking();
            }
        }

        private void tableEntityAttributes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex <= tableEntityAttributes.Rows.Count)
            {
                var changedRow = tableEntityAttributes.Rows[e.RowIndex];
                var rowKey = changedRow.Cells["columnKey"]?.Value?.ToString() ?? "";
                var rowValue = changedRow.Cells["columnValue"]?.Value?.ToString() ?? "";

                _map.CurrentEntityBrush.Attributes[rowKey] = rowValue;
                if (_map.SelectedEntity != null && (!_map.SelectedEntity.Attributes.ContainsKey(rowKey) || _map.SelectedEntity.Attributes[rowKey] != rowValue))
                {
                    ChangeHistory.StartTracking();
                    ChangeHistory.TrackChange(new EditEntityChange
                    {
                        InternalId = _map.SelectedEntity.InternalId,
                        PrefabId = _map.SelectedEntity.PrefabId,
                        WorldX = _map.SelectedEntity.WorldX,
                        WorldY = _map.SelectedEntity.WorldY,
                        Attributes = _map.SelectedEntity.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>(),
                    });

                    _map.IsDirty = true;
                    if (_map.SelectedEntity.Attributes == null)
                        _map.SelectedEntity.Attributes = new Dictionary<string, string>();
                    _map.SelectedEntity.Attributes[rowKey] = rowValue;
                    ChangeHistory.FinishTracking();
                }
            }
        }

        void IEventSubscriber<SelectedEntityChangeEvent>.OnNotify(SelectedEntityChangeEvent e)
        {
            if (_map.SelectedEntity != null && (_map.SelectedEntity.PrefabId != _map.CurrentEntityBrush.PrefabId || _map.CurrentEntityBrush.Attributes != _map.SelectedEntity.Attributes))
            {
                // select up current entity and selected
                _map.CurrentEntityBrush.PrefabId = _map.SelectedEntity.PrefabId;
                _map.CurrentEntityBrush.Attributes = _map.SelectedEntity.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>();

                // now trigger UI changes
                cmbPrefab.SelectedItem = _map.CurrentEntityBrush.PrefabId;
                tableEntityAttributes.Rows.Clear();
                foreach (var kv in _map.CurrentEntityBrush.Attributes)
                {
                    tableEntityAttributes.Rows.Add(new string[] { kv.Key, kv.Value });
                }
            }
        }
        #endregion

    }
}
