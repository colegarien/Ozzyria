using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;
using SkiaSharp;

namespace Ozzyria.Gryp.MapTools
{
    internal class ToolBelt
    {
        private MouseState mouseState = new MouseState();
        private Dictionary<string, ITool> tools;

        public ToolBelt()
        {
            tools = new Dictionary<string, ITool>
            {
                { "pan", new PanTool { Enabled = true, } },
                { "select", new SelectTool() },
                { "brush", new BrushTool() },
                { "fill", new FillTool() },
                { "filled_rectangle", new FilledRectangleTool() },
                { "rectangle", new RectangleTool() },
                { "line", new LineTool() },
            };
        }

        public void ToogleTool(string toolKey, bool isEnabled)
        {
            if (!tools.ContainsKey(toolKey))
            {
                return;
            }

            tools[toolKey].Enabled = isEnabled;
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
                if(tool.Value.Enabled)
                    tool.Value.OnMouseDown(mouseState, camera, map);
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
                if (tool.Value.Enabled)
                    tool.Value.OnMouseMove(mouseState, camera, map);
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
                if (tool.Value.Enabled)
                    tool.Value.OnMouseUp(mouseState, camera, map);
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

            if(map.SelectedRegion != null)
            {
                var renderX = camera.ViewX + camera.WorldToView(map.SelectedRegion.TileX * 32);
                var renderY = camera.ViewY + camera.WorldToView(map.SelectedRegion.TileY * 32);
                canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(map.SelectedRegion.TileWidth*32)-1, renderY + camera.WorldToView(map.SelectedRegion.TileHeight * 32) -1), Paints.TileSelectionPaint);
            }

            foreach (var tool in tools)
            {
                if (!tool.Value.Enabled)
                    continue;

                if (tool.Value is SelectTool)
                {
                    var selectTool = tool.Value as SelectTool;

                    if (selectTool.Selection.WorldWidth != 0 && selectTool.Selection.WorldHeight != 0)
                    {
                        var renderX = camera.ViewX + camera.WorldToView(selectTool.Selection.WorldX);
                        var renderY = camera.ViewY + camera.WorldToView(selectTool.Selection.WorldY);
                        canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(selectTool.Selection.WorldWidth), renderY + camera.WorldToView(selectTool.Selection.WorldHeight)), Paints.TileHighlightPaint);
                    }
                }
                else if (tool.Value is FilledRectangleTool)
                {
                    var filledRectangleTool = tool.Value as FilledRectangleTool;

                    if (filledRectangleTool.Area.WorldWidth != 0 && filledRectangleTool.Area.WorldHeight != 0)
                    {
                        var renderX = camera.ViewX + camera.WorldToView(filledRectangleTool.Area.WorldX);
                        var renderY = camera.ViewY + camera.WorldToView(filledRectangleTool.Area.WorldY);
                        canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(filledRectangleTool.Area.WorldWidth), renderY + camera.WorldToView(filledRectangleTool.Area.WorldHeight)), Paints.TileHighlightPaint);
                    }
                }
                else if (tool.Value is RectangleTool)
                {
                    var rectangleTool = tool.Value as RectangleTool;

                    if (rectangleTool.Area.WorldWidth != 0 && rectangleTool.Area.WorldHeight != 0)
                    {
                        var renderX = camera.ViewX + camera.WorldToView(rectangleTool.Area.WorldX);
                        var renderY = camera.ViewY + camera.WorldToView(rectangleTool.Area.WorldY);
                        canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(rectangleTool.Area.WorldWidth), renderY + camera.WorldToView(rectangleTool.Area.WorldHeight)), Paints.TileHighlightPaint);
                    }
                }
                else if (tool.Value is LineTool)
                {
                    var lineTool = tool.Value as LineTool;

                    if (lineTool.LineEndX != 0 || lineTool.LineStartX != 0 || lineTool.LineEndY != 0 || lineTool.LineStartY != 0)
                    {
                        var renderStartX = camera.ViewX + camera.WorldToView(lineTool.LineStartX);
                        var renderStartY = camera.ViewY + camera.WorldToView(lineTool.LineStartY);
                        var renderEndX = camera.ViewX + camera.WorldToView(lineTool.LineEndX);
                        var renderEndY = camera.ViewY + camera.WorldToView(lineTool.LineEndY);

                        canvas.DrawLine(renderStartX, renderStartY, renderEndX, renderEndY, Paints.TileHighlightPaint);
                    }
                }
            }
        }
    }
}
