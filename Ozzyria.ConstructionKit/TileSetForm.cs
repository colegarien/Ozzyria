using System;
using System.Windows.Forms;

namespace Ozzyria.ConstructionKit
{
    public partial class TileSetForm : Form
    {
        public TileSetForm()
        {
            InitializeComponent();
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
                    // TODO intialize tileset, save meta, and add to dropdown options
                }
            }
        }

        private bool isTileSetIdInUse(string id)
        {
            return false; // TODO actually validate id
        }
    }
}
