using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class SelectTool : ITool
    {
        public Boundary Selection = new Boundary
        {
            WorldX = 0,
            WorldY = 0,
            WorldWidth = 0,
            WorldHeight = 0
        };

        public void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                Selection.WorldX = mouseWorldX;
                Selection.WorldY = mouseWorldY;

                Selection.WorldWidth = 0;
                Selection.WorldHeight = 0;
            }
        }

        public void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                Selection.WorldWidth = mouseWorldX - Selection.WorldX;
                Selection.WorldHeight = mouseWorldY - Selection.WorldY;
            }
        }

        public void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            if (!mouseState.IsLeftDown)
            {
                if (Selection.WorldWidth < 4 && Selection.WorldHeight < 4)
                {
                    // clear selection if selection window is tiny
                    Selection.WorldWidth = 0;
                    Selection.WorldHeight = 0;
                }
                else
                {
                    // TODO actually just fire off an "onSelectEvent" or something or maybe just set selection directly on map... hmm... maybe not?

                    // snap to tile grid
                    var snappedX = (int)Math.Floor(Selection.WorldX / 32);
                    var snappedY = (int)Math.Floor(Selection.WorldY / 32);
                    var snappedWidth = (int)Math.Floor((Selection.WorldX + Selection.WorldWidth - 1) / 32) - snappedX + 1;
                    var snappedHeight = (int)Math.Floor((Selection.WorldY + Selection.WorldHeight - 1) / 32) - snappedY + 1;

                    Selection.WorldX = snappedX * 32;
                    Selection.WorldY = snappedY * 32;
                    Selection.WorldWidth = snappedWidth * 32;
                    Selection.WorldHeight = snappedHeight * 32;
                }
            }
        }
    }
}
