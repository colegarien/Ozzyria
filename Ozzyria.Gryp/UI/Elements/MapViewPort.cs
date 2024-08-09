using Ozzyria.Gryp.Models;
using SkiaSharp.Views.Desktop;
using SkiaSharp;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.MapTools;
using Ozzyria.Gryp.Models.Event;

namespace Ozzyria.Gryp.UI.Elements
{
    internal class MapViewPort: SKGLControl
    {
        internal ToolBelt? _toolBelt;
        internal Map? _map;
        internal Camera _camera = new Camera();

        #region Attachment
        public void AttachMap(Map map)
        {
            _map = map;
        }

        public void AttachToolBelt(ToolBelt toolBelt)
        {
            _toolBelt = toolBelt;
        }
        #endregion

        #region Camera Controls
        public void ResetCamera()
        {
            _camera.SizeCamera(ClientSize.Width, ClientSize.Height);
        }

        public void CenterOnWorldCoordinate(float worldX, float worldY)
        {
            _camera.MoveToViewCoordinates(-_camera.WorldToView(worldX) + (_camera.ViewWidth / 2f), -_camera.WorldToView(worldY) + (_camera.ViewHeight / 2f));
        }
        #endregion

        #region Form Events

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(_map != null && _toolBelt != null)
                _toolBelt.HandleMouseMove(e, _camera, _map);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_map != null && _toolBelt != null)
                _toolBelt.HandleMouseUp(e, _camera, _map);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_map != null && _toolBelt != null)
                _toolBelt.HandleMouseDown(e, _camera, _map);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            var scale = (e.Delta > 0)
                ? 0.1f
                : -0.1f;
            var targetScale = _camera.Scale * (1 + scale);
            _camera.ScaleTo(e.X, e.Y, targetScale);
        }


        protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            if (_map == null || _toolBelt == null)
                return;

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
                e.Surface.Canvas.DrawRect(new SKRect(_camera.ViewX, _camera.ViewY, _camera.ViewX + _camera.WorldToView(_map.Width * 32), _camera.ViewY + _camera.WorldToView(_map.Height * 32)), Paints.MapBackingPaint);

                // render layers
                for (int i = 0; i < _map.Layers.Count; i++)
                {
                    if (_map.IsLayerVisible(i))
                    {
                        _map.Layers[i].RenderToCanvas(e.Surface.Canvas, _camera);
                    }
                }

                // render overlay grid
                for (var x = 0; x < _map.Width; x++)
                {
                    for (var y = 0; y < _map.Height; y++)
                    {
                        var renderX = _camera.ViewX + _camera.WorldToView(x * 32);
                        var renderY = _camera.ViewY + _camera.WorldToView(y * 32);
                        var renderRight = renderX + _camera.WorldToView(32);
                        var renderBottom = renderY + _camera.WorldToView(32);

                        if (renderRight >= 0 && renderX < _camera.ViewWidth && renderBottom >= 0 && renderY < _camera.ViewHeight)
                        {
                            e.Surface.Canvas.DrawRect(new SKRect(renderX, renderY, renderRight, renderBottom), Paints.MapGridOverlayPaint);
                        }
                    }
                }

                if (_map.SelectedEntity != null)
                {
                    // TODO would be good to not render these if the entity tool is selected?
                    var renderX = _camera.ViewX + _camera.WorldToView(_map.SelectedEntity.WorldX);
                    var renderY = _camera.ViewY + _camera.WorldToView(_map.SelectedEntity.WorldY);
                    var renderRadius = _camera.WorldToView(2);
                    e.Surface.Canvas.DrawCircle(new SKPoint(renderX, renderY), renderRadius, Paints.SelectionDotOverlayPaint);
                }

                if (_map.SelectedWall != null)
                {
                    // TODO would be good to not render these if the wall tool is selected?
                    var renderX = _camera.ViewX + _camera.WorldToView(_map.SelectedWall.Boundary.WorldX + (_map.SelectedWall.Boundary.WorldWidth / 2f));
                    var renderY = _camera.ViewY + _camera.WorldToView(_map.SelectedWall.Boundary.WorldY + (_map.SelectedWall.Boundary.WorldHeight / 2f));
                    var renderRadius = _camera.WorldToView(2);
                    e.Surface.Canvas.DrawCircle(new SKPoint(renderX, renderY), renderRadius, Paints.SelectionDotOverlayPaint);
                }

                _toolBelt.RenderOverlay(e.Surface.Canvas, _camera, _map);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.Alt || e.KeyCode == Keys.Menu)
            {
                e.Handled = true;
                EventBus.Notify(new SwitchToDropperEvent { });
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Delete)
            {
                ChangeHistory.StartTracking();
                _map?.RemoveSelectedEntity();
                _map?.RemoveSelectedWall();
                ChangeHistory.FinishTracking();
            }

            if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.Alt || e.KeyCode == Keys.Menu)
            {
                e.Handled = true;
                EventBus.Notify(new UnswitchFromDropperEvent { });
            }
        }

#endregion
    }
}
