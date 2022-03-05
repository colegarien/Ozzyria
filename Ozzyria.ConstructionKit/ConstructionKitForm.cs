using Ozzyria.Game.Persistence;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Ozzyria.ConstructionKit
{
    public partial class ConstructionKitForm : Form
    {
        private string _currentMap = "";
        private string _currentTileSet = "";
        private Image _currentTileSetImage;

        private float zoom = 1.0f;
        private float mapEditorX = 0;
        private float mapEditorY = 0;

        // for panning
        private Point mousePanStart;
        private float mapEditorStartX = 0;
        private float mapEditorStartY = 0;
        private bool middleMousePressed = false;

        // for paint tools
        private Point currentMousePosition;

        public ConstructionKitForm()
        {
            InitializeComponent();
            TileSetMetaDataFactory.EnsureInitializedMetaData();
            MapMetaDataFactory.EnsureInitializedMetaData();

            listMap.Items.AddRange(MapMetaDataFactory.mapMetaDatas.Keys.ToArray());

            // TODO OZ-17 just make a panel subclass to avoid this hack
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, panelMapEditor, new object[] { true });

            /* TODO OZ-17 missing features:
               [X] ability to pan around map
               [X] ability to zoom in and out
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
            if (MapMetaDataFactory.mapMetaDatas.ContainsKey(_currentMap))
            {
                using (var context = new BufferedGraphicsContext())
                {
                    var buffer = context.Allocate(e.Graphics, panelMapEditor.DisplayRectangle);
                    buffer.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    buffer.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    buffer.Graphics.Clear(Color.Gray);
                    buffer.Graphics.ScaleTransform(zoom, zoom);

                    var worldPersistence = new WorldPersistence();
                    var tileMap = worldPersistence.LoadMap(_currentMap);

                    buffer.Graphics.DrawRectangle(new Pen(Color.CornflowerBlue), new Rectangle((int)mapEditorX, (int)mapEditorY, tileMap.Width * Game.Tile.DIMENSION, tileMap.Height * Game.Tile.DIMENSION));
                    foreach (var layer in tileMap.Layers)
                    {
                        foreach (var tile in layer.Value)
                        {
                            buffer.Graphics.DrawImage(_currentTileSetImage, new Rectangle((int)mapEditorX + tile.X * Game.Tile.DIMENSION, (int)mapEditorY + tile.Y * Game.Tile.DIMENSION, Game.Tile.DIMENSION, Game.Tile.DIMENSION), tile.TextureCoordX * Game.Tile.DIMENSION, tile.TextureCoordY * Game.Tile.DIMENSION, Game.Tile.DIMENSION, Game.Tile.DIMENSION, GraphicsUnit.Pixel);
                        }
                    }

                    var mouseMapX = (currentMousePosition.X / zoom);
                    var mouseMapY = (currentMousePosition.Y / zoom);
                    buffer.Graphics.DrawLine(new Pen(Color.Red), mouseMapX-10, mouseMapY, mouseMapX+10, mouseMapY);
                    buffer.Graphics.DrawLine(new Pen(Color.Red), mouseMapX, mouseMapY-10, mouseMapX, mouseMapY+10);

                    // TODO OZ-17 calculate what tile mouseToTileX/Y is in and highlight it
                    var tileX = (int)System.Math.Floor((mouseMapX - mapEditorX) / Game.Tile.DIMENSION);
                    var tileY = (int)System.Math.Floor((mouseMapY - mapEditorY) / Game.Tile.DIMENSION);
                    if(tileX >= 0 && tileX < tileMap.Width && tileY >= 0 && tileY < tileMap.Height)
                        buffer.Graphics.DrawRectangle(new Pen(Color.CornflowerBlue), new Rectangle((int)mapEditorX + tileX * Game.Tile.DIMENSION, (int)mapEditorY + tileY * Game.Tile.DIMENSION, Game.Tile.DIMENSION, Game.Tile.DIMENSION));

                    buffer.Render();
                }
            }
            else
            {
                e.Graphics.Clear(Color.Gray);
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

                var mapWidth = (float)(metaData.Width * Game.Tile.DIMENSION);
                var mapHeight = (float)(metaData.Height * Game.Tile.DIMENSION);

                // center in window based
                zoom = 1f;
                mapEditorX = (int)((panelMapEditor.DisplayRectangle.Width * 0.5f) - (mapWidth * 0.5f));
                mapEditorY = (int)((panelMapEditor.DisplayRectangle.Height * 0.5f) - (mapHeight * 0.5f));

                // biggest dimension should take of 88% of the screen (cause it look nice)
                var newZoom = (0.88f * panelMapEditor.DisplayRectangle.Width) / mapWidth;
                if (panelMapEditor.DisplayRectangle.Height < panelMapEditor.DisplayRectangle.Width)
                {
                    newZoom = (0.88f * panelMapEditor.DisplayRectangle.Height) / mapHeight;
                }

                ZoomTo((int)(panelMapEditor.DisplayRectangle.Width * 0.5f), (int)(panelMapEditor.DisplayRectangle.Height * 0.5f), newZoom);
                
                panelMapEditor.Refresh();
            }
        }

        private void ZoomTo(int xOrigin, int yOrigin, float targetZoomPercent)
        {
            var previousWorldXOrigin = (int)(xOrigin / zoom);
            var previousWorldYOrigin = (int)(yOrigin / zoom);

            zoom = targetZoomPercent;
            if (zoom < 0.05f)
            {
                zoom = 0.05f;
            }
            else if (zoom > 10f)
            {
                zoom = 10f;
            }

            var currentWorldXOrigin = (int)(xOrigin / zoom);
            var currentWorldYOrigin = (int)(yOrigin / zoom);

            mapEditorX += currentWorldXOrigin - previousWorldXOrigin;
            mapEditorY += currentWorldYOrigin - previousWorldYOrigin;
        }

        private void panelMapEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && !middleMousePressed)
            {
                mousePanStart = e.Location;
                mapEditorStartX = mapEditorX;
                mapEditorStartY = mapEditorY;
                middleMousePressed = true;
            }
        }

        private void panelMapEditor_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePosition = e.Location;

            if (e.Button == MouseButtons.Middle && middleMousePressed)
            {
                int deltaX = e.Location.X - mousePanStart.X;
                int deltaY = e.Location.Y - mousePanStart.Y;

                mapEditorX = (int)(mapEditorStartX + (deltaX / zoom));
                mapEditorY = (int)(mapEditorStartY + (deltaY / zoom));
            }

            panelMapEditor.Refresh();
        }

        private void panelMapEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && middleMousePressed)
            {
                middleMousePressed = false;
            }
        }

        private void panelMapEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            var scale = (e.Delta > 0)
                ? 0.1f
                : -0.1f;
            var targetZoomPercent = zoom * (1 + scale);

            ZoomTo(e.X, e.Y, targetZoomPercent);
            panelMapEditor.Refresh();
        }
    }
}
