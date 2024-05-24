using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

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
            camera.SizeCamera(mapViewPort.ClientSize.Width, mapViewPort.ClientSize.Height);

            // Initialize Pens
            mapEditorFont = new Font(new FontFamily("Arial"), 12);
            bluePen = new Pen(Color.Blue);
            redPen = new Pen(Color.Red);
            greenBrush = new SolidBrush(Color.Green);
            redBrush = new SolidBrush(Color.Red);
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
                camera.MoveToViewCoordinates((_map.Width / 2f) - (mapViewPort.ClientSize.Width / 2f), (_map.Height / 2f) - (mapViewPort.ClientSize.Height / 2f));

                mapGridImage = null;
                mainStatusLabel.Text = "Successfully created map";
            }
            else
            {
                mainStatusLabel.Text = "Canceled";
            }
        }

        private void RebuildLayerView()
        {
            var selectedIndices = layerList.SelectedIndices.Cast<int>().ToArray();

            layerImageList.Images.Clear();
            layerList.Items.Clear();

            for (int i = 0; i < _map.Layers.Count; i++)
            {
                layerImageList.Images.Add(_map.Layers[i].GetThumbnail().ToBitmap());
                layerList.Items.Add(new ListViewItem { Text = "Layer " + i, ImageIndex = i });
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

        private void skglControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            var scale = (e.Delta > 0)
                ? 0.1f
                : -0.1f;
            var targetScale = camera.Scale * (1 + scale);
            camera.ScaleTo(e.X, e.Y, targetScale);
        }

        private void skglControl1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseState.PreviousMouseX = mouseState.MouseX;
            mouseState.PreviousMouseY = mouseState.MouseY;

            mouseState.MouseX = e.X;
            mouseState.MouseY = e.Y;

            if (mouseState.IsMiddleDown)
            {
                var mouseDeltaX = mouseState.MouseX - mouseState.PreviousMouseX;
                var mouseDeltaY = mouseState.MouseY - mouseState.PreviousMouseY;
                camera.MoveToViewCoordinates(camera.ViewX + mouseDeltaX, camera.ViewY + mouseDeltaY);
            }
        }

        private void skglControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseState.IsLeftDown = true;
                mouseState.LeftDownStartX = e.X;
                mouseState.LeftDownStartY = e.Y;
            }

            if (e.Button == MouseButtons.Right)
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

        private void skglControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mouseState.IsLeftDown)
            {
                mouseState.IsLeftDown = false;
            }

            if (e.Button == MouseButtons.Right && mouseState.IsRightDown)
            {
                mouseState.IsRightDown = false;
            }

            if (e.Button == MouseButtons.Middle && mouseState.IsMiddleDown)
            {
                mouseState.IsMiddleDown = false;
            }
        }

        private void reRenderTimer_Tick(object sender, EventArgs e)
        {
            mapViewPort.Invalidate();
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

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            // Draw background Grid
            var bluePaint = new SKPaint
            {
                Color = new SKColor(
                red: (byte)0,
                green: (byte)0,
                blue: (byte)255,
                alpha: (byte)255),
                StrokeWidth = 1,
                IsAntialias = true
            };
            var canvasSize = e.Surface.Canvas.DeviceClipBounds.Size;
            e.Surface.Canvas.Clear(new SKColor(
                red: (byte)0,
                green: (byte)0,
                blue: (byte)0,
                alpha: (byte)255));
            for (var x = 0; x <= (canvasSize.Width / 16); x++)
            {
                for (var y = 0; y <= (canvasSize.Height / 16); y++)
                {
                    e.Surface.Canvas.DrawLine((x * 16), 0, (x * 16), canvasSize.Height, bluePaint);
                    e.Surface.Canvas.DrawLine(0, (y * 16), canvasSize.Width, (y * 16), bluePaint);
                }
            }

            if (_map.Width > 0 && _map.Height > 0)
            {
                // render map backing
                e.Surface.Canvas.DrawRect(new SKRect(camera.ViewX, camera.ViewY, camera.ViewX+camera.WorldToView(_map.Width * 32), camera.ViewY+ camera.WorldToView(_map.Height*32)), new SKPaint
                {
                    Color = new SKColor(
                    red: (byte)255,
                    green: (byte)230,
                    blue: (byte)230,
                    alpha: (byte)255),
                    StrokeWidth = 1,
                    IsAntialias = true
                });

            }

            for (int i = 0; i < _map.Layers.Count; i++)
            {
                //var image = _map.Layers[i].GetImage();
                //e.Surface.Canvas.DrawBitmap(image, new SKRect(camera.ViewX, camera.ViewY, camera.ViewX+camera.WorldToView(image.Width), camera.ViewY+camera.WorldToView(image.Height)), null);
                _map.Layers[i].RenderToCanvas(e.Surface.Canvas, camera);
            }


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
                        e.Surface.Canvas.DrawRect(new SKRect(renderX, renderY, renderRight, renderBottom), new SKPaint
                        {
                            Color = new SKColor(
                            red: (byte)255,
                            green: (byte)0,
                            blue: (byte)0,
                            alpha: (byte)255),
                            StrokeWidth = 1,
                            IsStroke = true,
                            IsAntialias = false
                        });
                    }
                }
            }

            if (_map.Width > 0 && _map.Height > 0)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);
                if (mouseTileX >= 0 && mouseTileY >= 0 && mouseTileX < _map.Width && mouseTileY < _map.Height)
                {
                    var renderX = camera.ViewX + camera.WorldToView(mouseTileX * 32);
                    var renderY = camera.ViewY + camera.WorldToView(mouseTileY * 32);
                    e.Surface.Canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(32), renderY + camera.WorldToView(32)), new SKPaint
                    {
                        Color = new SKColor(
                        red: (byte)0,
                        green: (byte)255,
                        blue: (byte)0,
                        alpha: (byte)255),
                        StrokeWidth = 2,
                        IsStroke = true,
                        IsAntialias = false
                    });
                }
            }
        }
    }
}
