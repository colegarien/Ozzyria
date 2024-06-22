using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class SelectTool : ITool
    {
        private const float UNSELECT_THRESHOLD = 4;
        public bool doingSelect = false;
        public WorldBoundary Selection = new WorldBoundary
        {
            WorldX = 0,
            WorldY = 0,
            WorldWidth = 0,
            WorldHeight = 0
        };

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && !doingSelect)
            {
                doingSelect = true;
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                Selection.WorldX = mouseWorldX;
                Selection.WorldY = mouseWorldY;

                Selection.WorldWidth = 0;
                Selection.WorldHeight = 0;
            }
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && doingSelect)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                Selection.WorldWidth = mouseWorldX - Selection.WorldX;
                Selection.WorldHeight = mouseWorldY - Selection.WorldY;
            }
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            if (!mouseState.IsLeftDown && doingSelect)
            {
                doingSelect = false;
                if (Math.Abs(Selection.WorldWidth) < UNSELECT_THRESHOLD && Math.Abs(Selection.WorldHeight) < UNSELECT_THRESHOLD)
                {
                    map.SelectedRegion = null;
                }
                else
                {
                    // snap to tile grid
                    var snappedLeft = (int)Math.Floor(Selection.WorldX / 32);
                    var snappedTop = (int)Math.Floor(Selection.WorldY / 32);
                    var snappedRight = (int)Math.Floor((Selection.WorldX + Selection.WorldWidth - 1) / 32);
                    var snappedBottom = (int)Math.Floor((Selection.WorldY + Selection.WorldHeight - 1) / 32);

                    map.SelectedRegion = new TileBoundary
                    {
                        TileX = snappedLeft < snappedRight ? snappedLeft : snappedRight,
                        TileY = snappedTop < snappedBottom ? snappedTop : snappedBottom,
                        TileWidth = (snappedLeft < snappedRight ? snappedRight - snappedLeft : snappedLeft - snappedRight) + 1,
                        TileHeight = (snappedTop < snappedBottom ? snappedBottom - snappedTop : snappedTop - snappedBottom) + 1,
                    };
                }

                // clear selection if selection window is tiny
                Selection.WorldWidth = 0;
                Selection.WorldHeight = 0;
            }
        }
    }
}
