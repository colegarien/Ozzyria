using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class PanTool : ITool
    {
        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            // nothing special to be done
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsMiddleDown)
            {
                var mouseDeltaX = mouseState.MouseX - mouseState.PreviousMouseX;
                var mouseDeltaY = mouseState.MouseY - mouseState.PreviousMouseY;
                camera.MoveToViewCoordinates(camera.ViewX + mouseDeltaX, camera.ViewY + mouseDeltaY);
            }
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            // nothing special to be done
        }
    }
}
