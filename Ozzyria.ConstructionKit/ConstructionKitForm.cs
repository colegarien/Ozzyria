using System.Windows.Forms;

namespace Ozzyria.ConstructionKit
{
    public partial class ConstructionKitForm : Form
    {
        public ConstructionKitForm()
        {
            InitializeComponent();
            TileSetMetaDataFactory.EnsureInitializedMetaData();
            MapMetaDataFactory.EnsureInitializedMetaData();
        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == menuItemTileSet)
            {
                var tileSetForm = new TileSetForm();
                tileSetForm.ShowDialog();
            }
            else if(e.ClickedItem == menuItemMap)
            {
                var mapForm = new MapForm();
                mapForm.ShowDialog();
            }
        }

        private void spltRight_Panel1_Paint(object sender, PaintEventArgs e)
        {
            // TODO OZ-17 draw currently open map or object or whatever preview here!!
        }
    }
}
