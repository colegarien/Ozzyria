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

        internal Camera camera = new Camera();
        internal MouseState mouseState = new MouseState();

        public MainGrypWindow()
        {
            InitializeComponent();
            camera.SizeCamera(mapViewPort.ClientSize.Width, mapViewPort.ClientSize.Height);
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
                camera.MoveToViewCoordinates(-camera.WorldToView(_map.Width * 32 / 2f) + (mapViewPort.ClientSize.Width / 2f), -camera.WorldToView(_map.Height * 32 / 2f) + (mapViewPort.ClientSize.Height / 2f));

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

        private void mapViewPort_MouseWheel(object sender, MouseEventArgs e)
        {
            var scale = (e.Delta > 0)
                ? 0.1f
                : -0.1f;
            var targetScale = camera.Scale * (1 + scale);
            camera.ScaleTo(e.X, e.Y, targetScale);
        }

        private void mapViewPort_MouseMove(object sender, MouseEventArgs e)
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

        private void mapViewPort_MouseDown(object sender, MouseEventArgs e)
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

        private void mapViewPort_MouseUp(object sender, MouseEventArgs e)
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

        private void mapViewPort_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            // Draw background Grid
            var canvasSize = e.Surface.Canvas.DeviceClipBounds.Size;
            e.Surface.Canvas.Clear(Paints.CanvasColor);
            for (var x = 0; x <= (canvasSize.Width / 16); x++)
            {
                for (var y = 0; y <= (canvasSize.Height / 16); y++)
                {
                    e.Surface.Canvas.DrawLine((x * 16), 0, (x * 16), canvasSize.Height, Paints.CanvasGridPaint);
                    e.Surface.Canvas.DrawLine(0, (y * 16), canvasSize.Width, (y * 16), Paints.CanvasGridPaint);
                }
            }

            if (_map.Width > 0 && _map.Height > 0)
            {
                // render map backing
                e.Surface.Canvas.DrawRect(new SKRect(camera.ViewX, camera.ViewY, camera.ViewX+camera.WorldToView(_map.Width * 32), camera.ViewY+ camera.WorldToView(_map.Height*32)), Paints.MapBackingPaint);

                // render layers
                for (int i = 0; i < _map.Layers.Count; i++)
                {
                    _map.Layers[i].RenderToCanvas(e.Surface.Canvas, camera);
                }

                // render overlay grid
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
                            e.Surface.Canvas.DrawRect(new SKRect(renderX, renderY, renderRight, renderBottom), Paints.MapGridOverlayPaint);
                        }
                    }
                }

                // render mouse hover higlight
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);
                if (mouseTileX >= 0 && mouseTileY >= 0 && mouseTileX < _map.Width && mouseTileY < _map.Height)
                {
                    var renderX = camera.ViewX + camera.WorldToView(mouseTileX * 32);
                    var renderY = camera.ViewY + camera.WorldToView(mouseTileY * 32);
                    e.Surface.Canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(32), renderY + camera.WorldToView(32)), Paints.TileHighlightPaint);
                }
            }
        }
    }
}
