using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;
using SkiaSharp;

namespace Ozzyria.Gryp.MapTools
{
    internal class ToolBelt
    {
        private MouseState mouseState = new MouseState();
        private ITool[] tools;

        public ToolBelt()
        {
            tools = new ITool[]
            {
                new PanTool(),
                new SelectTool(),
                new BrushTool(),
            };
        }

        public void HandleMouseDown(MouseEventArgs e, Camera camera, Map map)
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

            foreach (var tool in tools)
            {
                tool.OnMouseDown(mouseState, camera, map);
            }
        }

        public void HandleMouseMove(MouseEventArgs e, Camera camera, Map map)
        {
            mouseState.PreviousMouseX = mouseState.MouseX;
            mouseState.PreviousMouseY = mouseState.MouseY;

            mouseState.MouseX = e.X;
            mouseState.MouseY = e.Y;

            foreach(var tool in tools)
            {
                tool.OnMouseMove(mouseState, camera, map);
            }
        }

        public void HandleMouseUp(MouseEventArgs e, Camera camera, Map map)
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

            foreach (var tool in tools)
            {
                tool.OnMouseUp(mouseState, camera, map);
            }
        }


        public void RenderOverlay(SKCanvas canvas, Camera camera, Map map)
        {
            // render mouse hover highlight
            var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
            var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
            var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
            var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

            if (mouseTileX >= 0 && mouseTileY >= 0 && mouseTileX < map.Width && mouseTileY < map.Height)
            {
                var renderX = camera.ViewX + camera.WorldToView(mouseTileX * 32);
                var renderY = camera.ViewY + camera.WorldToView(mouseTileY * 32);
                canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(32), renderY + camera.WorldToView(32)), Paints.TileHighlightPaint);
            }

            foreach (var tool in tools)
            {
                if (tool is SelectTool)
                {
                    var selectTool = tool as SelectTool;

                    if (selectTool.Selection.WorldWidth != 0 && selectTool.Selection.WorldHeight != 0)
                    {
                        var renderX = camera.ViewX + camera.WorldToView(selectTool.Selection.WorldX);
                        var renderY = camera.ViewY + camera.WorldToView(selectTool.Selection.WorldY);
                        canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(selectTool.Selection.WorldWidth), renderY + camera.WorldToView(selectTool.Selection.WorldHeight)), Paints.TileHighlightPaint);
                    }
                }
            }
        }
    }
}
