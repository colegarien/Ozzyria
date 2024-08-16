using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class PathTool : ITool
    {
        bool isPathing = false;
        bool originalAutoTile = false;

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if(mouseState.IsLeftDown && !isPathing)
            {
                isPathing = true;
                originalAutoTile = map.AutoTile;

                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                ChangeHistory.StartTracking();
                map.AutoTile = true;
                var currentTile = map.GetTile(mouseTileX, mouseTileY);
                if (currentTile != null) {
                    map.PushTile(currentTile, mouseTileX, mouseTileY);
                }
            }
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if(mouseState.IsLeftDown && isPathing)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                var currentTile = map.GetTile(mouseTileX, mouseTileY);
                if (currentTile != null)
                {
                    map.PushTile(currentTile, mouseTileX, mouseTileY);
                }
            }
            else if (isPathing)
            {
                // mouse is up but tool still thinks it's pathing
                isPathing = false;
                map.AutoTile = originalAutoTile;
                ChangeHistory.FinishTracking();
            }
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            if (!mouseState.IsLeftDown && isPathing)
            {
                isPathing = false;
                map.AutoTile = originalAutoTile;

                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                var currentTile = map.GetTile(mouseTileX, mouseTileY);
                if (currentTile != null)
                {
                    map.PushTile(currentTile, mouseTileX, mouseTileY);
                }
                ChangeHistory.FinishTracking();
            }
        }
    }
}
