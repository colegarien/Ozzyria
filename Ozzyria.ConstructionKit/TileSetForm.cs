using System;
using System.Linq;
using System.Windows.Forms;

namespace Ozzyria.ConstructionKit
{

    public partial class TileSetForm : Form
    {

        public TileSetForm()
        {
            InitializeComponent();
            var metaData = TileSetMetaDataFactory.tileSetMetaDatas;

            var tileSetNames = metaData.Keys;
            comboBoxTileSet.Items.AddRange(tileSetNames.ToArray());

            // TODO be better about this, maybe specify somewhere (see Renderable.cs constants)
            dropDownZDepth.Items.AddRange(new ComboBoxItem[]
            {
                new ComboBoxItem{ Id = 0, Name = "Background"},
                new ComboBoxItem{ Id = 10, Name = "Items"},
                new ComboBoxItem{ Id = 25, Name = "Middleground"},
                new ComboBoxItem{ Id = 50, Name = "Foreground"},
                new ComboBoxItem{ Id = 255, Name = "In-Game UI"},
                new ComboBoxItem{ Id = 99999, Name = "Debug"},
            });
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.Close(); // TODO save stuff and don't close;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close(); // TODO save stuff and close
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            var prompt = new SimplePrompt("New Tile Set");
            var result = prompt.ShowDialog();

            if (result == DialogResult.OK)
            {
                var newTileSetId = prompt.GetPromptInput();

                var errorCaption = "";
                var errorMessage = "";

                if (newTileSetId.Equals(""))
                {
                    errorCaption = "Missing ID";
                    errorMessage = "New Tile Set ID should not be empty!";
                }
                else if (isTileSetIdInUse(newTileSetId))
                {
                    errorCaption = "ID In Use";
                    errorMessage = "Supplied ID \"" + newTileSetId + "\" is already in use!";
                }

                if (!errorMessage.Equals(""))
                {
                    MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    TileSetMetaDataFactory.AddNewTileSet(newTileSetId);
                    // TODO save meta to file
                    comboBoxTileSet.Items.Add(newTileSetId);
                    comboBoxTileSet.SelectedItem = newTileSetId;
                }
            }
        }

        private bool isTileSetIdInUse(string id)
        {
            return comboBoxTileSet.Items.IndexOf(id) != -1;
        }

        private void buttonNewTileType_Click(object sender, EventArgs e)
        {
            var tileSetId = (string)comboBoxTileSet.SelectedItem ?? "";
            if (!TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey(tileSetId))
            {
                MessageBox.Show("Must First Select a Tile Set!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var prompt = new SimplePrompt("New Tile Type");
            var result = prompt.ShowDialog();

            if (result == DialogResult.OK)
            {
                var newTileTypeName = prompt.GetPromptInput();

                var errorCaption = "";
                var errorMessage = "";

                if (newTileTypeName.Equals(""))
                {
                    errorCaption = "Missing Name";
                    errorMessage = "New Tile Type Name should not be empty!";
                }
                else if (isTileTypeNameInUse(newTileTypeName))
                {
                    errorCaption = "Name In Use";
                    errorMessage = "Supplied Name \"" + newTileTypeName + "\" is already in use!";
                }

                if (!errorMessage.Equals(""))
                {
                    MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var existingTileTypeIds = TileSetMetaDataFactory.tileSetMetaDatas[tileSetId].TileTypes;
                    var newTileTypeId = existingTileTypeIds.Count > 0 ? (existingTileTypeIds.Max() + 1) : 0;
                    var newTileTypeItem = new ComboBoxItem
                    {
                        Id = newTileTypeId,
                        Name = newTileTypeName
                    };
                    listTileTypes.Items.Add(newTileTypeItem);
                    listTileTypes.SelectedItem = newTileTypeItem;
                    TileSetMetaDataFactory.AddNewTileType(tileSetId, newTileTypeItem.Id, newTileTypeItem.Name);
                }
            }
        }

        private bool isTileTypeNameInUse(string name)
        {
            var tileSetId = (string)comboBoxTileSet.SelectedItem ?? "";
            return TileSetMetaDataFactory.tileSetMetaDatas[tileSetId].TileNames.Values.Any(n => n == name);
        }

        private void comboBoxTileSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];

                // TODO avoid double clearing and loading if tileset is already been selected
                listTileTypes.Items.Clear();
                foreach(var tileTypeIdNamePair in metaData.TileNames)
                {
                    listTileTypes.Items.Add(new ComboBoxItem
                    {
                        Id = tileTypeIdNamePair.Key,
                        Name = tileTypeIdNamePair.Value,
                    });
                }
            }
        }

        private void listTileTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO move into re-usabler function! (RefreshTileTypeForm or something?)
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                dropDownZDepth.SelectedItem = dropDownZDepth.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i =>
                        (metaData.BaseTileZ.ContainsKey(tileTypeId) && i.Id == metaData.BaseTileZ[tileTypeId])
                        || (!metaData.BaseTileZ.ContainsKey(tileTypeId) && i.Id == 0)
                    );


                if (metaData.TilesThatSupportTransitions.Any(i => i == tileTypeId))
                    radTranistionableYes.Checked = true;
                else
                    radTranistionableNo.Checked = true;
                if (metaData.TilesThatSupportPathing.Any(i => i == tileTypeId))
                    radPathableYes.Checked = true;
                else
                    radPathableNo.Checked = true;
                if (metaData.TilesThatSupportWalling.Any(i => i == tileTypeId))
                    radWallYes.Checked = true;
                else
                    radWallNo.Checked = true;

                // TODO tileset image component for picking X and Y cooridnates
                    // - add higlight for current tile type
                    // - add pathing arrows for pathing
                    // - add offset and thickness indication for walls
                // TODO wall tiles offset and thickness adjustments
            }
        }

        private void radTranistionableYes_CheckedChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (radTranistionableYes.Checked && !metaData.TilesThatSupportTransitions.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportTransitions.Add(tileTypeId);
                }
                else if (!radTranistionableYes.Checked && metaData.TilesThatSupportTransitions.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportTransitions.Remove(tileTypeId);
                }
            }
        }

        private void radPathableYes_CheckedChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (radPathableYes.Checked && !metaData.TilesThatSupportPathing.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportPathing.Add(tileTypeId);
                }
                else if (!radPathableYes.Checked && metaData.TilesThatSupportPathing.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportPathing.Remove(tileTypeId);
                }
            }
        }

        private void radWallYes_CheckedChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (radWallYes.Checked && !metaData.TilesThatSupportWalling.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportWalling.Add(tileTypeId);
                }
                else if (!radWallYes.Checked && metaData.TilesThatSupportWalling.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportWalling.Remove(tileTypeId);
                }
            }
        }

        private void dropDownZDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                var item = dropDownZDepth.SelectedItem as ComboBoxItem;

                if (item.Id == 0 && metaData.BaseTileZ.ContainsKey(tileTypeId))
                {
                    metaData.BaseTileZ.Remove(tileTypeId);
                }
                else if(item.Id != 0)
                {
                    metaData.BaseTileZ[tileTypeId] = item.Id;
                }
            }
        }
    }

    class ComboBoxItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
