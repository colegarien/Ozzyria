﻿using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class RectangleTool : ITool
    {
        private const float SKIP_THRESHOLD = 4;

        public uint BrushResource { get; set; } = 1;
        public int BrushTextureX { get; set; } = 0;
        public int BrushTextureY { get; set; } = 0;
        public int Stroke { get; set; } = 0;


        public bool doingRectangle = false;
        public WorldBoundary Area = new WorldBoundary
        {
            WorldX = 0,
            WorldY = 0,
            WorldWidth = 0,
            WorldHeight = 0
        };

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && !doingRectangle)
            {
                doingRectangle = true;

                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                Area.WorldX = mouseWorldX;
                Area.WorldY = mouseWorldY;

                Area.WorldWidth = 0;
                Area.WorldHeight = 0;
            }
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && doingRectangle)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                Area.WorldWidth = mouseWorldX - Area.WorldX;
                Area.WorldHeight = mouseWorldY - Area.WorldY;
            }
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            if (!mouseState.IsLeftDown && doingRectangle)
            {
                doingRectangle = false;
                if (Math.Abs(Area.WorldWidth) < SKIP_THRESHOLD && Math.Abs(Area.WorldHeight) < SKIP_THRESHOLD)
                {
                    map.SelectedRegion = null;
                }
                else
                {
                    // snap to tile grid
                    var snappedLeft = (int)Math.Floor(Area.WorldX / 32);
                    var snappedTop = (int)Math.Floor(Area.WorldY / 32);
                    var snappedRight = (int)Math.Floor((Area.WorldX + Area.WorldWidth - 1) / 32);
                    var snappedBottom = (int)Math.Floor((Area.WorldY + Area.WorldHeight - 1) / 32);

                    if(snappedRight < snappedLeft)
                    {
                        var temp = snappedLeft;
                        snappedLeft = snappedRight;
                        snappedRight = temp;
                    }

                    if(snappedBottom < snappedTop)
                    {
                        var temp = snappedBottom;
                        snappedBottom = snappedTop;
                        snappedTop = temp;
                    }

                    for (int tileX = snappedLeft; tileX <= snappedRight; tileX++)
                    {
                        for (int tileY = snappedTop; tileY <= snappedBottom; tileY++)
                        {
                            // Only paint around stroke
                            if (tileY - snappedTop <= Stroke || snappedBottom - tileY <= Stroke || tileX - snappedLeft <= Stroke || snappedRight - tileX <= Stroke)
                            {
                                var tileData = new TileData();
                                tileData.Images.AddRange(map.CurrentBrush);
                                map.PushTile(tileData, tileX, tileY);
                            }
                        }
                    }
                }

                // clear selection if selection window is tiny
                Area.WorldWidth = 0;
                Area.WorldHeight = 0;
            }
        }
    }
}
