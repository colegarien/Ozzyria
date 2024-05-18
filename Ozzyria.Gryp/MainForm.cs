using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using System.Reflection;
using System.Windows.Forms;

namespace Ozzyria.Gryp
{
    public partial class MainGrypWindow : Form
    {
        internal Map _map = new Map();
        internal Bitmap mapGridImage = null;

        internal Camera camera = new Camera();


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

            // TODO infer graphics to draw
            if (_map.Width > 0 && _map.Height > 0)
            {
                if (mapGridImage == null)
                {
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
            }

            for (int i = 0; i < _map.Layers.Count; i++)
            {
                var image = _map.Layers[i].GetImage();
                e.Graphics.DrawImage(image, new RectangleF(camera.ViewX, camera.ViewY, camera.WorldToView(image.Width), camera.WorldToView(image.Height)));
            }

            if (mapGridImage != null)
            {
                e.Graphics.DrawImage(mapGridImage, new RectangleF(camera.ViewX, camera.ViewY, camera.WorldToView(mapGridImage.Width), camera.WorldToView(mapGridImage.Height)));
            }

            // Render Width x Height
            e.Graphics.DrawString($"{_map.Width}x{_map.Height}", mapEditorFont, greenBrush, new PointF());
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

        private void viewPortPanel_Scroll(object sender, ScrollEventArgs e)
        {
            camera.Scale += (e.NewValue - e.OldValue) * 0.001f;
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
            // Track mouse position
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
                foreach(ToolStripItem item in mainToolbelt.Items) {
                    if(item is ToolStripButton && item != sender)
                    {
                        // Uncheck all other tools in the toolblet
                        ((ToolStripButton)item).Checked = false;
                    }
                }
            }
        }
    }
}
