using Ozzyria.Game.Persistence;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ozzyria.ConstructionKit
{
    public partial class ConstructionKitForm : Form
    {
        private string _currentMap = "";
        private string _currentTileSet = "";
        private Image _currentTileSetImage;

        public ConstructionKitForm()
        {
            InitializeComponent();
            TileSetMetaDataFactory.EnsureInitializedMetaData();
            MapMetaDataFactory.EnsureInitializedMetaData();

            listMap.Items.AddRange(MapMetaDataFactory.mapMetaDatas.Keys.ToArray());

            /* TODO OZ-17 missing features:
               [] ability to pan around map
               [] ability to zoom in and out
               [] ability to change "active layer"
               [] ability to paint/erase tiles from layers
               [] ability to specify or calculate or whatever the transition tiles, pathing, and walling when saving the map
               [] ability to save map tile edits
            */
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
                if (mapForm.ShowDialog() == DialogResult.OK)
                {
                    listMap.Items.Clear();
                    _currentMap = "";
                    listMap.Items.AddRange(MapMetaDataFactory.mapMetaDatas.Keys.ToArray());
                    panelMapEditor.Refresh();
                }
            }
        }

        private void panelMapEditor_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (MapMetaDataFactory.mapMetaDatas.ContainsKey(_currentMap))
            {
                var worldPersistence = new WorldPersistence();
                var tileMap = worldPersistence.LoadMap(_currentMap);

                foreach (var layer in tileMap.Layers)
                {
                    foreach (var tile in layer.Value)
                    {
                        // TODO OZ-17 draw to intermediary image so it doesn't get flashy
                        e.Graphics.DrawImage(_currentTileSetImage, new Rectangle(tile.X * Game.Tile.DIMENSION, tile.Y * Game.Tile.DIMENSION, Game.Tile.DIMENSION, Game.Tile.DIMENSION), tile.TextureCoordX * Game.Tile.DIMENSION, tile.TextureCoordY * Game.Tile.DIMENSION, Game.Tile.DIMENSION, Game.Tile.DIMENSION, GraphicsUnit.Pixel);
                    }
                }
            }
            else
            {
                g.Clear(Color.Gray);
            }
        }

        private void listMap_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var mapName = listMap.SelectedItem as string;
            if (MapMetaDataFactory.mapMetaDatas.ContainsKey(mapName) && mapName != _currentMap)
            {
                _currentMap = mapName;
                var metaData = MapMetaDataFactory.mapMetaDatas[mapName];

                if (metaData.TileSet != _currentTileSet)
                {
                    _currentTileSet = metaData.TileSet;
                    _currentTileSetImage?.Dispose();
                    _currentTileSetImage = Image.FromFile(Content.Loader.Root() + "/TileSets/Sprites/" + _currentTileSet + ".png");
                }
                panelMapEditor.Refresh();
            }
        }
    }
}
