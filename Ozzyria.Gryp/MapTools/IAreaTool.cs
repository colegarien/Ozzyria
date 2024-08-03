using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal abstract class IAreaTool : ITool
    {
        private const float CANCEL_THRESHOLD = 4;
        protected WorldBoundary Area = new WorldBoundary
        {
            WorldX = 0,
            WorldY = 0,
            WorldWidth = 0,
            WorldHeight = 0
        };
        public bool trackingAreaSelect = false;

        protected abstract void OnCancel(MouseState mouseState, Camera camera, Map map);
        protected abstract void OnComplete(MouseState mouseState, Camera camera, Map map);

        public WorldBoundary GetWorldArea()
        {
            return Area.Clone();
        }

        public TileBoundary GetTileArea()
        {
            // convert to tile-space
            var snappedLeft = (int)Math.Floor(Area.WorldX / 32);
            var snappedTop = (int)Math.Floor(Area.WorldY / 32);
            var snappedRight = (int)Math.Floor((Area.WorldX + Area.WorldWidth) / 32);
            var snappedBottom = (int)Math.Floor((Area.WorldY + Area.WorldHeight) / 32);

            return new TileBoundary
            {
                TileX = snappedLeft,
                TileY = snappedTop,
                TileWidth = snappedRight - snappedLeft + 1,
                TileHeight = snappedBottom - snappedTop + 1
            };
        }


        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && !trackingAreaSelect)
            {
                trackingAreaSelect = true;

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
            if (mouseState.IsLeftDown && trackingAreaSelect)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                Area.WorldWidth = mouseWorldX - Area.WorldX;
                Area.WorldHeight = mouseWorldY - Area.WorldY;
            }
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            if (!mouseState.IsLeftDown && trackingAreaSelect)
            {
                if (Area.WorldWidth < 0)
                {
                    Area.WorldX += Area.WorldWidth;
                    Area.WorldWidth = Math.Abs(Area.WorldWidth);
                }

                if (Area.WorldHeight < 0)
                {
                    Area.WorldY += Area.WorldHeight;
                    Area.WorldHeight = Math.Abs(Area.WorldHeight);
                }

                trackingAreaSelect = false;
                if (Area.WorldWidth < CANCEL_THRESHOLD && Area.WorldHeight < CANCEL_THRESHOLD)
                {
                    OnCancel(mouseState, camera, map);
                }
                else
                {
                    OnComplete(mouseState, camera, map);
                }

                // clear selection if selection window is tiny
                Area.WorldWidth = 0;
                Area.WorldHeight = 0;
            }
        }
    }
}
