using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;
using System.Drawing;
using System.Reflection;

namespace Ozzyria.Gryp
{
    public partial class MainGrypWindow : Form
    {
        internal Map _map = new Map();
        internal Bitmap mapGridImage = null;

        internal Camera camera = new Camera();
        internal MouseState mouseState = new MouseState();


        internal Font mapEditorFont;
        internal Pen bluePen;
        internal Pen redPen;
        internal Brush greenBrush;
        internal Brush redBrush;

        public MainGrypWindow()
        {
            InitializeComponent();

            // Initialize Pens
            mapEditorFont = new Font(new FontFamily("Arial"), 12);
            bluePen = new Pen(Color.Blue);
            redPen = new Pen(Color.Red);
            greenBrush = new SolidBrush(Color.Green);
            redBrush = new SolidBrush(Color.Red);

            // hackity hack to override DoubleBuffered without making custom class
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, viewPortPanel, new object[] { true });
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mapDialog = new NewMapDialog();
            if (mapDialog.ShowDialog() == DialogResult.OK)
            {
                _map.Width = mapDialog.NewMapResult.Width;
                _map.Height = mapDialog.NewMapResult.Height;

                _map.Layers.Clear();
                _map.PushLayer();
                _map.PushLayer();
                _map.PushLayer();

                RebuildLayerView();

                // Center Camera onto Map
                camera.MoveToViewCoordinates((_map.Width / 2f) - (viewPortPanel.ClientSize.Width / 2f), (_map.Height / 2f) - (viewPortPanel.ClientSize.Height / 2f));

                mapGridImage = null;
                mainStatusLabel.Text = "Successfully created map";
            }
            else
            {
                mainStatusLabel.Text = "Canceled";
            }
        }

        private void viewPortPanel_Paint(object sender, PaintEventArgs e)
        {
            // Draw background Grid
            var canvasSize = e.ClipRectangle.Size;
            for (var x = 0; x <= (canvasSize.Width / 16); x++)
            {
                for (var y = 0; y <= (canvasSize.Height / 16); y++)
                {
                    e.Graphics.DrawLine(bluePen, (x * 16), 0, (x * 16), canvasSize.Height);
                    e.Graphics.DrawLine(bluePen, 0, (y * 16), canvasSize.Width, (y * 16));
                }
            }

            if (_map.Width > 0 && _map.Height > 0)
            {
                if (mapGridImage == null)
                {
                    // re-build mape image
                    mapGridImage = new Bitmap(_map.Width * 32 + 1, _map.Height * 32 + 1);
                    using (var graphics = Graphics.FromImage(mapGridImage))
                    {
                        for (var x = 0; x < _map.Width; x++)
                        {
                            for (var y = 0; y < _map.Height; y++)
                            {
                                graphics.DrawRectangle(redPen, new Rectangle(x * 32, y * 32, 32, 32));
                            }
                        }
                    }
                }

                // render map backing
                e.Graphics.FillRectangle(Brushes.DarkSlateGray, new RectangleF(camera.ViewX, camera.ViewY, camera.WorldToView(mapGridImage.Width), camera.WorldToView(mapGridImage.Height)));

            }

            for (int i = 0; i < _map.Layers.Count; i++)
            {
                var image = _map.Layers[i].GetImage();
                e.Graphics.DrawImage(image, new RectangleF(camera.ViewX, camera.ViewY, camera.WorldToView(image.Width), camera.WorldToView(image.Height)));
            }

            if (mapGridImage != null)
            {
                e.Graphics.DrawImage(mapGridImage, new RectangleF(camera.ViewX, camera.ViewY, camera.WorldToView(mapGridImage.Width), camera.WorldToView(mapGridImage.Height)));

                // render highlight square
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);
                if (mouseTileX >= 0 && mouseTileY >= 0 && mouseTileX < _map.Width && mouseTileY < _map.Height)
                {
                    e.Graphics.DrawRectangle(Pens.Green, new RectangleF(camera.ViewX + camera.WorldToView(mouseTileX * 32), camera.ViewY + camera.WorldToView(mouseTileY * 32), camera.WorldToView(32), camera.WorldToView(32)));
                }
            }


        }

        private void RebuildLayerView()
        {
            layerImageList.Images.Clear();
            layerList.Items.Clear();

            for (int i = 0; i < _map.Layers.Count; i++)
            {
                layerImageList.Images.Add(_map.Layers[i].GetImage());
                layerList.Items.Add(new ListViewItem { Text = "Layer " + i, ImageIndex = i });
            }
        }

        private void viewPortPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            var scale = (e.Delta > 0)
                ? 0.1f
                : -0.1f;
            var targetScale = camera.Scale * (1 + scale);
            camera.ScaleTo(e.X, e.Y, targetScale);
        }

        private void viewPortPanel_MouseMove(object sender, MouseEventArgs e)
        {
            mouseState.PreviousMouseX = mouseState.MouseX;
            mouseState.PreviousMouseY = mouseState.MouseY;

            mouseState.MouseX = e.X;
            mouseState.MouseY = e.Y;

            if(mouseState.IsMiddleDown)
            {
                var mouseDeltaX = mouseState.MouseX - mouseState.PreviousMouseX;
                var mouseDeltaY = mouseState.MouseY - mouseState.PreviousMouseY;
                camera.MoveToViewCoordinates(camera.ViewX + mouseDeltaX, camera.ViewY + mouseDeltaY);
            }
        }

        private void viewPortPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                mouseState.IsLeftDown = true;
                mouseState.LeftDownStartX = e.X;
                mouseState.LeftDownStartY = e.Y;
            }

            if(e.Button == MouseButtons.Right)
            {
                mouseState.IsRightDown = true;
                mouseState.RightDownStartX = e.X;
                mouseState.RightDownStartY = e.Y;
            }

            if (e.Button == MouseButtons.Middle)
            {
                mouseState.IsMiddleDown = true;
                mouseState.MiddleDownStartX = e.X;
                mouseState.MiddleDownStartY = e.Y;
            }
        }

        private void viewPortPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && mouseState.IsLeftDown)
            {
                mouseState.IsLeftDown = false;
            }

            if(e.Button == MouseButtons.Right && mouseState.IsRightDown)
            {
                mouseState.IsRightDown = false;
            }

            if(e.Button == MouseButtons.Middle &&  mouseState.IsMiddleDown)
            {
                mouseState.IsMiddleDown = false;
            }
        }

        private void reRenderTimer_Tick(object sender, EventArgs e)
        {
            viewPortPanel.Refresh();
        }

        private void layerList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
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
            else if (e.KeyCode == Keys.A)
            {
                if (_map.Width > 0 && _map.Height > 0)
                {
                    _map.PushLayer();
                    RebuildLayerView();
                    mainStatusLabel.Text = "Layer added";
                }
            }
        }

        private void onToolChecked_CheckedChanged(object sender, EventArgs e)
        {
            // if is a checked-able tool
            if (sender is ToolStripButton && ((ToolStripButton)sender).Checked)
            {
                foreach (ToolStripItem item in mainToolbelt.Items)
                {
                    if (item is ToolStripButton && item != sender)
                    {
                        // Uncheck all other tools in the toolblet
                        ((ToolStripButton)item).Checked = false;
                    }
                }
            }
        }
    }
}
