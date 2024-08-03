using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class WallTool : IAreaTool
    {
        private bool isSelecting = false;

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            base.OnMouseDown(mouseState, camera, map);
            if(mouseState.IsRightDown && !isSelecting)
            {
                isSelecting = true;
            }
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            base.OnMouseMove(mouseState, camera, map);
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            base.OnMouseUp(mouseState, camera, map);

            if(!mouseState.IsRightDown && isSelecting)
            {
                isSelecting = false;

                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                map.SelectWall(mouseWorldX, mouseWorldY);
            }
        }


        protected override void OnCancel(MouseState mouseState, Camera camera, Map map)
        {
            // TODO select a handle? Might need to add some fancy state to handle handles
        }

        protected override void OnComplete(MouseState mouseState, Camera camera, Map map)
        {
            map.AddWall(new Wall { Boundary = GetWorldArea() });
        }
    }
}
