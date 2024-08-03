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
                { "wall", new WallTool() },
                { "entity", new EntityTool() },
                { "dropper", new DropperTool() },
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

                if(tool.Value is IAreaTool)
                {
                    var areaTool = tool.Value as IAreaTool;
                    var area = areaTool?.GetWorldArea() ?? new WorldBoundary();

                    if (area.WorldWidth != 0 && area.WorldHeight != 0)
                    {
                        var renderX = camera.ViewX + camera.WorldToView(area.WorldX);
                        var renderY = camera.ViewY + camera.WorldToView(area.WorldY);
                        canvas.DrawRect(new SKRect(renderX, renderY, renderX + camera.WorldToView(area.WorldWidth), renderY + camera.WorldToView(area.WorldHeight)), Paints.TileHighlightPaint);
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

                // draw special tool overlays for selection tools
                if (tool.Value is WallTool && map.SelectedWall != null)
                {
                    var wallTool = tool.Value as WallTool;

                    var toolArea = wallTool?.GetWorldArea() ?? new WorldBoundary();
                    var wallArea = map.SelectedWall.Boundary;

                    var isResizing = wallTool?.isResizing ?? false;

                    var wallAreaX = camera.ViewX + camera.WorldToView(wallArea.WorldX);
                    var wallAreaY = camera.ViewY + camera.WorldToView(wallArea.WorldY);
                    var wallAreaWidth = camera.WorldToView(wallArea.WorldWidth);
                    var wallAreaHeight = camera.WorldToView(wallArea.WorldHeight);

                    var toolAreaX = camera.ViewX + camera.WorldToView(toolArea.WorldX);
                    var toolAreaY = camera.ViewY + camera.WorldToView(toolArea.WorldY);
                    var toolAreaWidth = camera.WorldToView(toolArea.WorldWidth);
                    var toolAreaHeight = camera.WorldToView(toolArea.WorldHeight);
                    if (wallTool != null && !isResizing)
                    {
                        // not resizing, just draw the wall and resize handles
                        canvas.DrawRect(new SKRect(wallAreaX, wallAreaY, wallAreaX + wallAreaWidth, wallAreaY + wallAreaHeight), Paints.WallSelectionPaint);

                        foreach (var handle in wallTool.Handles)
                        {
                            var paint = handle == wallTool.SelectedHandle
                                ? Paints.ActivationHandleHoverPaint
                                : Paints.ActivationHandlePaint;
                            var handleX = camera.ViewX + camera.WorldToView(handle.ActivationArea.WorldX);
                            var handleY = camera.ViewY + camera.WorldToView(handle.ActivationArea.WorldY);
                            var handleWidth = camera.WorldToView(handle.ActivationArea.WorldWidth);
                            var handleHeight = camera.WorldToView(handle.ActivationArea.WorldHeight);

                            canvas.DrawRect(new SKRect(handleX, handleY, handleX + handleWidth, handleY + handleHeight), paint);
                        }
                    }
                    else if(wallTool != null && isResizing)
                    {
                        // is resizing, draw the underlying current wall and the resizing overlay
                        canvas.DrawRect(new SKRect(wallAreaX, wallAreaY, wallAreaX + wallAreaWidth, wallAreaY + wallAreaHeight), Paints.TileHighlightPaint);
                        canvas.DrawRect(new SKRect(toolAreaX, toolAreaY, toolAreaX + toolAreaWidth, toolAreaY + toolAreaHeight), Paints.WallSelectionPaint);
                    }
                }
                else if (tool.Value is EntityTool && map.SelectedEntity != null)
                {
                    var entityX = camera.ViewX + camera.WorldToView(map.SelectedEntity.WorldX);
                    var entityY = camera.ViewY + camera.WorldToView(map.SelectedEntity.WorldY);
                    var entityHalfWidth = camera.WorldToView(32) / 2f;
                    var entityHalfHeight = camera.WorldToView(32) / 2f;
                    canvas.DrawLine(entityX - entityHalfWidth, entityY, entityX + entityHalfWidth, entityY, Paints.EntitySelectionPaint);
                    canvas.DrawLine(entityX, entityY - entityHalfHeight, entityX, entityY + entityHalfHeight, Paints.EntitySelectionPaint);
                    canvas.DrawCircle(entityX, entityY, entityHalfWidth, Paints.EntitySelectionPaint);
                }
            }
        }
    }
}
