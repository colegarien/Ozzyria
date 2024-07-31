using Ozzyria.Content.Models.Area;
using Ozzyria.Gryp.MapTools;
using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Reflection;

namespace Ozzyria.Gryp
{
    public partial class MainGrypWindow : Form
    {
        internal Map _map = new Map();

        internal Camera camera = new Camera();
        internal ToolBelt toolBelt = new ToolBelt();

        internal string _lastSelectedPreset = "";
        internal bool processingThumbnails = false;

        internal ToolStripButton? _preQuickSwitchTool = null; 

        public MainGrypWindow()
        {
            InitializeComponent();
            camera.SizeCamera(mapViewPort.ClientSize.Width, mapViewPort.ClientSize.Height);

            cmbPrefab.Items.AddRange(new string[] {
                "slime_spawner",
                "door",
                "exp_orb",
            });

            // hackity hack to override DoubleBuffered without making custom class
            typeof(ListView).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, layerList, new object[] { true });
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                camera.MoveToViewCoordinates(-camera.WorldToView(_map.Width * 32 / 2f) + (mapViewPort.ClientSize.Width / 2f), -camera.WorldToView(_map.Height * 32 / 2f) + (mapViewPort.ClientSize.Height / 2f));

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
            var mapDialog = new OpenMapDialog();
            if (mapDialog.ShowDialog() == DialogResult.OK)
            {
                var areaId = mapDialog.AreaId;
                var areaData = AreaData.Retrieve(areaId);

                if (areaData != null && areaData.TileData != null)
                {
                    _map.FromAreaData(areaData);

                    ChangeHistory.Clear();
                    RebuildLayerView();
                    RebuildBrushView();

                    // Center Camera onto Map
                    camera.MoveToViewCoordinates(-camera.WorldToView(_map.Width * 32 / 2f) + (mapViewPort.ClientSize.Width / 2f), -camera.WorldToView(_map.Height * 32 / 2f) + (mapViewPort.ClientSize.Height / 2f));
                    mainStatusLabel.Text = "Successfully loaded " + areaId;

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
                layerList.SelectedIndices.Add(0);
            }
        }

        private void mapViewPort_MouseWheel(object sender, MouseEventArgs e)
        {
            var scale = (e.Delta > 0)
                ? 0.1f
                : -0.1f;
            var targetScale = camera.Scale * (1 + scale);
            camera.ScaleTo(e.X, e.Y, targetScale);
        }

        private void mapViewPort_MouseMove(object sender, MouseEventArgs e)
        {
            toolBelt.HandleMouseMove(e, camera, _map);
        }

        private void mapViewPort_MouseDown(object sender, MouseEventArgs e)
        {
            toolBelt.HandleMouseDown(e, camera, _map);
        }

        private void mapViewPort_MouseUp(object sender, MouseEventArgs e)
        {
            toolBelt.HandleMouseUp(e, camera, _map);
            RebuildBrushView(); // TODO this is a hack to handle dropper potentially dropping (might could just do a simple event situation?)
            // TODO this is a hack because there isn't another easy way to trigger an event of "hey an entity got selected just now" currently
            if(_map.SelectedEntity != null && (_map.SelectedEntity.PrefabId != _map.CurrentEntity.PrefabId || _map.CurrentEntity.Attributes != _map.SelectedEntity.Attributes))
            {
                // select up current entity and selected
                _map.CurrentEntity.PrefabId = _map.SelectedEntity.PrefabId;
                _map.CurrentEntity.Attributes = _map.SelectedEntity.Attributes.ToDictionary(kv => kv.Key, kv => kv.Value);

                // now trigger UI changes
                cmbPrefab.SelectedItem = _map.CurrentEntity.PrefabId;
                foreach(DataGridViewRow row in tableEntityAttributes.Rows)
                {
                    var rowKey = row.Cells["columnKey"]?.Value?.ToString() ?? "";
                    if (_map.CurrentEntity.Attributes.ContainsKey(rowKey))
                    {
                        var valueKey = row.Cells["columnValue"].Value = _map.CurrentEntity.Attributes[rowKey];
                    }
                }
            }
        }

        private void reRenderTimer_Tick(object sender, EventArgs e)
        {
            // force a timely rerender
            mapViewPort.Invalidate();
        }

        private void onToolChecked_CheckedChanged(object sender, EventArgs e)
        {
            // if is a checked-able tool
            _map.SelectedEntity = null;
            if (sender is ToolStripButton && ((ToolStripButton)sender).Checked)
            {
                toolBelt.ToogleTool(((ToolStripButton)sender).Tag?.ToString() ?? "", true);
                foreach (ToolStripItem item in mainToolbelt.Items)
                {
                    if (item is ToolStripButton && item != sender)
                    {
                        // Uncheck all other tools in the toolbelt
                        toolBelt.ToogleTool(((ToolStripButton)item).Tag?.ToString() ?? "", false);
                        ((ToolStripButton)item).Checked = false;
                    }
                }
            }
            else if (sender is ToolStripButton)
            {
                toolBelt.ToogleTool(((ToolStripButton)sender).Tag?.ToString() ?? "", false);
            }
        }

        private void mapViewPort_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            // Draw background Grid
            var canvasSize = e.Surface.Canvas.DeviceClipBounds.Size;
            e.Surface.Canvas.Clear(Paints.CanvasColor);
            for (var x = 0; x <= (canvasSize.Width / 16); x++)
            {
                for (var y = 0; y <= (canvasSize.Height / 16); y++)
                {
                    e.Surface.Canvas.DrawLine((x * 16), 0, (x * 16), canvasSize.Height, Paints.CanvasGridPaint);
                    e.Surface.Canvas.DrawLine(0, (y * 16), canvasSize.Width, (y * 16), Paints.CanvasGridPaint);
                }
            }

            if (_map.Width > 0 && _map.Height > 0)
            {
                // render map backing
                e.Surface.Canvas.DrawRect(new SKRect(camera.ViewX, camera.ViewY, camera.ViewX + camera.WorldToView(_map.Width * 32), camera.ViewY + camera.WorldToView(_map.Height * 32)), Paints.MapBackingPaint);

                // render layers
                for (int i = 0; i < _map.Layers.Count; i++)
                {
                    if (_map.IsLayerVisible(i))
                    {
                        _map.Layers[i].RenderToCanvas(e.Surface.Canvas, camera);
                    }
                }

                if(_map.SelectedEntity != null)
                {
                    var entityX = camera.ViewX + camera.WorldToView(_map.SelectedEntity.WorldX);
                    var entityY = camera.ViewY + camera.WorldToView(_map.SelectedEntity.WorldY);
                    var entityHalfWidth = camera.WorldToView(32) / 2f;
                    var entityHalfHeight = camera.WorldToView(32) / 2f;
                    e.Surface.Canvas.DrawLine(entityX - entityHalfWidth, entityY, entityX + entityHalfWidth, entityY, Paints.TileSelectionPaint);
                    e.Surface.Canvas.DrawLine(entityX, entityY - entityHalfHeight, entityX, entityY + entityHalfHeight, Paints.TileSelectionPaint);
                    e.Surface.Canvas.DrawCircle(entityX, entityY, entityHalfWidth, Paints.TileSelectionPaint);
                }

                // render overlay grid
                for (var x = 0; x < _map.Width; x++)
                {
                    for (var y = 0; y < _map.Height; y++)
                    {
                        var renderX = camera.ViewX + camera.WorldToView(x * 32);
                        var renderY = camera.ViewY + camera.WorldToView(y * 32);
                        var renderRight = renderX + camera.WorldToView(32);
                        var renderBottom = renderY + camera.WorldToView(32);

                        if (renderRight >= 0 && renderX < camera.ViewWidth && renderBottom >= 0 && renderY < camera.ViewHeight)
                        {
                            e.Surface.Canvas.DrawRect(new SKRect(renderX, renderY, renderRight, renderBottom), Paints.MapGridOverlayPaint);
                        }
                    }
                }

                toolBelt.RenderOverlay(e.Surface.Canvas, camera, _map);
            }
        }

        private void logicTimer_Tick(object sender, EventArgs e)
        {
            // TODO pipe commands to/from toolbelt (will likely need to actually process data in a separate Thread)

            // Check if thumbnails need refreshed
            if (!processingThumbnails)
            {
                processingThumbnails = true;
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

                    processingThumbnails = false;
                });
            }
        }

        private void layerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (layerList?.SelectedIndices?.Count <= 0)
            {
                _map.ActiveLayer = 0;
                return;
            }

            _map.ActiveLayer = layerList?.SelectedIndices[0] ?? 0;
            btnHideShowLayer.Text = _map.IsLayerVisible(_map.ActiveLayer) ? "Hide" : "Show";
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

                mainStatusLabel.Text = "Successfully edited texture";
                RebuildBrushView();
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
            RebuildBrushView();
        }

        private void btnRemoveBrush_Click(object sender, EventArgs e)
        {
            if (_map == null || _map.Width <= 0 || _map.Height <= 0 || _map.ActiveLayer < 0)
            {
                return;
            }

            var rebuildBrushView = false;
            foreach (ListViewItem selectItem in listCurrentBrush.SelectedItems.Cast<ListViewItem>().OrderByDescending(e => e.Index))
            {
                rebuildBrushView = true;
                _map.CurrentBrush.RemoveAt(selectItem.Index);
            }

            if (rebuildBrushView)
            {
                RebuildBrushView();
                mainStatusLabel.Text = "Brush(s) removed";
            }
        }

        private void RebuildBrushView()
        {
            if (listCurrentBrush.Items.Count == _map.CurrentBrush.Count && _map.CurrentBrush.Count > 0)
            {
                // TODO this is a hack to skip excessive calls due to us having to call this all the time to hopefully catch window changes due to DropperTool (should remove once add command pattern)
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

                mainStatusLabel.Text = "Brush preset selected";
                RebuildBrushView();
            }
            else
            {
                mainStatusLabel.Text = "Preset selection canceled";
            }
        }

        private void MainGrypWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_map.IsDirty)
            {
                var result = MessageBox.Show(
                    "You have unsaved changes.\r\nWould you like to save before exiting?",
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

        private void cmbPrefab_SelectedIndexChanged(object sender, EventArgs e)
        {
            var newPrefabId = cmbPrefab?.SelectedItem?.ToString() ?? "";
            if (_map.CurrentEntity.Attributes == null)
                _map.CurrentEntity.Attributes = new Dictionary<string, string>();
            else if (_map.CurrentEntity.PrefabId != newPrefabId)
                _map.CurrentEntity.Attributes.Clear();
            _map.CurrentEntity.PrefabId = newPrefabId;

            tableEntityAttributes.Rows.Clear();
            switch (_map.CurrentEntity.PrefabId)
            {
                case "slime_spawner":
                    break;
                case "door":
                    tableEntityAttributes.Rows.Add(new string[] { "new_area_id", _map.CurrentEntity.Attributes.GetValueOrDefault("new_area_id") ?? "" });
                    tableEntityAttributes.Rows.Add(new string[] { "new_area_x", _map.CurrentEntity.Attributes.GetValueOrDefault("new_area_x") ?? "" });
                    tableEntityAttributes.Rows.Add(new string[] { "new_area_y", _map.CurrentEntity.Attributes.GetValueOrDefault("new_area_y") ?? "" });
                    break;
                case "exp_orb":
                    tableEntityAttributes.Rows.Add(new string[] { "amount", _map.CurrentEntity.Attributes.GetValueOrDefault("amount") ?? "" });
                    break;
            }

            if(_map.SelectedEntity != null)
            {
                _map.SelectedEntity.PrefabId = _map.CurrentEntity.PrefabId;
                _map.SelectedEntity.Attributes = _map.CurrentEntity.Attributes.ToDictionary(kv => kv.Key, kv => kv.Value);
            }
        }

        private void tableEntityAttributes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex <= tableEntityAttributes.Rows.Count)
            {
                var changedRow = tableEntityAttributes.Rows[e.RowIndex];
                var rowKey = changedRow.Cells["columnKey"]?.Value?.ToString() ?? "";
                var rowValue = changedRow.Cells["columnValue"]?.Value?.ToString() ?? "";

                _map.CurrentEntity.Attributes[rowKey] = rowValue;
                if(_map.SelectedEntity != null)
                {
                    _map.SelectedEntity.Attributes[rowKey] = rowValue;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeHistory.Undo(_map);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeHistory.Redo(_map);
        }

        private void mapViewPort_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.LMenu || e.KeyCode == Keys.Alt || e.KeyCode == Keys.Menu)
            {
                e.Handled = true;
                foreach (ToolStripItem item in mainToolbelt.Items)
                {
                    if (item is ToolStripButton && ((ToolStripButton)item).Checked)
                    {
                        if(item == toolDropper)
                        {
                            // The dropper tool is already selected, don't need todo anything
                            return;
                        }
                        else
                        {
                            // track the currently selected tool so it can be reselected on release
                            _preQuickSwitchTool = (ToolStripButton)item;
                        }
                    }
                }
                toolDropper.Checked = true;
            }
        }

        private void mapViewPort_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                _map.RemoveSelectedEntity();
            }

            if(e.KeyCode == Keys.LMenu || e.KeyCode == Keys.Alt || e.KeyCode == Keys.Menu)
            {
                e.Handled = true;
                toolDropper.Checked = false;
                if (_preQuickSwitchTool != null)
                {
                    _preQuickSwitchTool.Checked = true;
                    _preQuickSwitchTool = null;
                }
            }
        }
    }
}
