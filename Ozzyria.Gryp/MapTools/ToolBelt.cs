using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Event;
using SkiaSharp;

namespace Ozzyria.Gryp.MapTools
{
    internal class ToolBelt : IEventSubscriber<OverlayRenderEvent>, IEventSubscriber<MouseUpEvent>, IEventSubscriber<MouseDownEvent>, IEventSubscriber<MouseMoveEvent>
    {
        private Dictionary<string, ITool> tools;

        public ToolBelt()
        {
            EventBus.Subscribe(this);
            tools = new Dictionary<string, ITool>
            {
                { "pan", new PanTool { Enabled = true, } },
                { "move", new MoveTool() },
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

        public void OnNotify(MouseDownEvent e)
        {
            foreach (var tool in tools)
            {
                if(tool.Value.Enabled)
                    tool.Value.OnMouseDown(e.MouseState, e.Camera, e.Map);
            }
        }

        public void OnNotify(MouseMoveEvent e)
        {
            foreach (var tool in tools)
            {
                if (tool.Value.Enabled)
                    tool.Value.OnMouseMove(e.MouseState, e.Camera, e.Map);
            }
        }

        public void OnNotify(MouseUpEvent e)
        {
            foreach (var tool in tools)
            {
                if (tool.Value.Enabled)
                    tool.Value.OnMouseUp(e.MouseState, e.Camera, e.Map);
            }
        }


        void IEventSubscriber<OverlayRenderEvent>.OnNotify(OverlayRenderEvent e)
        {
            var canvas = e.Canvas;
            var camera = e.Camera;
            var map = e.Map;
            var mouseState = e.MouseState;

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

                        wallTool.PlaceHandles(map);
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
                    RenderEntityShape(canvas, camera, map.SelectedEntity.WorldX, map.SelectedEntity.WorldY, Paints.EntitySelectionPaint);
                }
                else if(tool.Value is MoveTool)
                {
                    var moveTool = tool.Value as MoveTool;
                    if(moveTool != null)
                    {
                        if(map.SelectedEntity != null)
                        {
                            RenderEntityShape(canvas, camera, map.SelectedEntity.WorldX, map.SelectedEntity.WorldY, Paints.EntitySelectionPaint);
                            if (moveTool.isMovingEntity)
                            {
                                RenderEntityShape(canvas, camera, map.SelectedEntity.WorldX, map.SelectedEntity.WorldY, Paints.EntityGhostPaint);
                                RenderLine(canvas, camera, map.SelectedEntity.WorldX, map.SelectedEntity.WorldY, mouseWorldX, mouseWorldY, Paints.EntityGhostPaint);
                                RenderEntityShape(canvas, camera, mouseWorldX, mouseWorldY, Paints.EntitySelectionPaint);
                            }
                        }
                        
                        if (map.SelectedWall != null)
                        {
                            RenderWorldBoundary(canvas, camera, map.SelectedWall.Boundary, Paints.WallSelectionPaint);
                            if(moveTool.isMovingWall)
                            {
                                var previewBoundary = new WorldBoundary
                                {
                                    WorldX = mouseWorldX - (map.SelectedWall.Boundary.WorldWidth / 2f),
                                    WorldY = mouseWorldY - (map.SelectedWall.Boundary.WorldHeight / 2f),
                                    WorldWidth = map.SelectedWall.Boundary.WorldWidth,
                                    WorldHeight = map.SelectedWall.Boundary.WorldHeight,
                                };

                                RenderWorldBoundary(canvas, camera, map.SelectedWall.Boundary, Paints.WallGhostPaint);
                                RenderLine(canvas, camera, map.SelectedWall.Boundary.WorldX + (map.SelectedWall.Boundary.WorldWidth / 2f), map.SelectedWall.Boundary.WorldY + (map.SelectedWall.Boundary.WorldHeight / 2f), mouseWorldX, mouseWorldY, Paints.EntityGhostPaint);
                                RenderWorldBoundary(canvas, camera, previewBoundary, Paints.WallSelectionPaint);
                            }
                        }
                    }
                }
            }
        }

        private void RenderLine(SKCanvas canvas, Camera camera, float worldX1, float worldY1, float worldX2, float worldY2, SKPaint paint)
        {
            var x1 = camera.ViewX + camera.WorldToView(worldX1);
            var y1 = camera.ViewY + camera.WorldToView(worldY1);
            var x2 = camera.ViewX + camera.WorldToView(worldX2);
            var y2 = camera.ViewY + camera.WorldToView(worldY2);
            canvas.DrawLine(new SKPoint(x1, y1), new SKPoint(x2, y2), paint);
        }

        private void RenderEntityShape(SKCanvas canvas, Camera camera, float worldX, float worldY, SKPaint paint)
        {
            var entityX = camera.ViewX + camera.WorldToView(worldX);
            var entityY = camera.ViewY + camera.WorldToView(worldY);
            var entityHalfWidth = camera.WorldToView(32) / 2f;
            var entityHalfHeight = camera.WorldToView(32) / 2f;
            canvas.DrawLine(entityX - entityHalfWidth, entityY, entityX + entityHalfWidth, entityY, paint);
            canvas.DrawLine(entityX, entityY - entityHalfHeight, entityX, entityY + entityHalfHeight, paint);
            canvas.DrawCircle(entityX, entityY, entityHalfWidth, paint);

        }

        private void RenderWorldBoundary(SKCanvas canvas, Camera camera, WorldBoundary boundary, SKPaint paint)
        {
            var boundaryX = camera.ViewX + camera.WorldToView(boundary.WorldX);
            var boundaryY = camera.ViewY + camera.WorldToView(boundary.WorldY);
            var boundaryWidth = camera.WorldToView(boundary.WorldWidth);
            var boundaryHeight = camera.WorldToView(boundary.WorldHeight);

            canvas.DrawRect(new SKRect(boundaryX, boundaryY, boundaryX + boundaryWidth, boundaryY + boundaryHeight), paint);
        }
    }
}
