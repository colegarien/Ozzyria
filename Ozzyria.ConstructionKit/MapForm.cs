using System;
using System.Linq;
using System.Windows.Forms;

namespace Ozzyria.ConstructionKit
{
    public partial class MapForm : Form
    {
        private string _currentMap = "";

        public MapForm()
        {
            InitializeComponent();

            var mapMetaData = MapMetaDataFactory.mapMetaDatas;
            var mapNames = mapMetaData.Keys;
            dropDownMap.Items.AddRange(mapNames.ToArray());

            var tileSetMetaData = TileSetMetaDataFactory.tileSetMetaDatas;
            var tileSetNames = tileSetMetaData.Keys;
            dropDownTileSet.Items.AddRange(tileSetNames.ToArray());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // TODO OZ-17 pull in all the for fields and update the meta-datas for the current item (this will help with brand-new maps saving all the values)
            MapMetaDataFactory.SaveMetaData();
            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MapMetaDataFactory.SaveMetaData();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MapMetaDataFactory.InitializeMetaData();
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var prompt = new SimplePrompt("New Map");
            var result = prompt.ShowDialog();

            if (result == DialogResult.OK)
            {
                var newMapId = prompt.GetPromptInput();

                var errorCaption = "";
                var errorMessage = "";

                if (newMapId.Equals(""))
                {
                    errorCaption = "Missing ID";
                    errorMessage = "New Map ID should not be empty!";
                }
                else if (isMapIdInUse(newMapId))
                {
                    errorCaption = "ID In Use";
                    errorMessage = "Supplied ID \"" + newMapId + "\" is already in use!";
                }

                if (!errorMessage.Equals(""))
                {
                    MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MapMetaDataFactory.AddNewMap(newMapId);

                    dropDownMap.Items.Add(newMapId);
                    dropDownMap.SelectedItem = newMapId;
                }
            }
        }

        private bool isMapIdInUse(string id)
        {
            return dropDownMap.Items.IndexOf(id) != -1;
        }

        private void dropDownMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mapName = (string)dropDownMap.SelectedItem ?? "";
            if (MapMetaDataFactory.mapMetaDatas.ContainsKey(mapName) && mapName != _currentMap)
            {
                _currentMap = mapName;
                var metaData = MapMetaDataFactory.mapMetaDatas[_currentMap];

                if (dropDownTileSet.Items.Contains(metaData.TileSet))
                {
                    dropDownTileSet.SelectedItem = metaData.TileSet;
                }
                else
                {
                    dropDownTileSet.SelectedIndex = 0;
                }

                numWidth.Value = metaData.Width;
                numHeight.Value = metaData.Height;
                numLayers.Value = metaData.Layers;
            }
        }

        private void dropDownTileSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MapMetaDataFactory.mapMetaDatas.ContainsKey(_currentMap))
            {
                var metaData = MapMetaDataFactory.mapMetaDatas[_currentMap];
                metaData.TileSet = (string)dropDownTileSet.SelectedItem;
            }
        }

        private void numWidth_ValueChanged(object sender, EventArgs e)
        {
            if (MapMetaDataFactory.mapMetaDatas.ContainsKey(_currentMap))
            {
                var metaData = MapMetaDataFactory.mapMetaDatas[_currentMap];
                metaData.Width = (int)numWidth.Value;
            }
        }

        private void numHeight_ValueChanged(object sender, EventArgs e)
        {
            if (MapMetaDataFactory.mapMetaDatas.ContainsKey(_currentMap))
            {
                var metaData = MapMetaDataFactory.mapMetaDatas[_currentMap];
                metaData.Height = (int)numHeight.Value;
            }
        }

        private void numLayers_ValueChanged(object sender, EventArgs e)
        {
            if (MapMetaDataFactory.mapMetaDatas.ContainsKey(_currentMap))
            {
                var metaData = MapMetaDataFactory.mapMetaDatas[_currentMap];
                metaData.Layers = (int)numLayers.Value;
            }
        }
    }
}
