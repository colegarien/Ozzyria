using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class FilledRectangleTool : ITool
    {
        private const float SKIP_THRESHOLD = 4;

        public uint BrushResource { get; set; } = 1;
        public int BrushTextureX { get; set; } = 0;
        public int BrushTextureY { get; set; } = 0;

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
            if(mouseState.IsLeftDown && !doingRectangle)
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

                    for (int tileX = snappedLeft; tileX <= snappedRight; tileX++)
                    {
                        for (int tileY = snappedTop; tileY <= snappedBottom; tileY++)
                        {
                            map.PushTile(new TileData
                            {
                                Images = new List<TextureCoords>() {
                                    new TextureCoords()
                                    {
                                        Resource = BrushResource,
                                        TextureX = BrushTextureX,
                                        TextureY = BrushTextureY,
                                    }
                                },
                            }, tileX, tileY);
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
