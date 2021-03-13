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
                    // TODO verify that it's fine that the item index is being assumed as the tile type (scared some dumb re-ordering could throw crap off
                    listTileTypes.Items.Add(newTileTypeName);
                    listTileTypes.SelectedItem = newTileTypeName;
                    TileSetMetaDataFactory.AddNewTileType((string)comboBoxTileSet.SelectedItem, listTileTypes.SelectedIndex, newTileTypeName);
                }
            }
        }

        private bool isTileTypeNameInUse(string name)
        {
            // TODO verify that it's fine that the item index is being assumed as the tile type (scared some dumb re-ordering could throw crap off
            return listTileTypes.Items.Contains(name);
        }

        private void comboBoxTileSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO clear out form then re-fill form based on current active tileset id
        }
    }
}
