using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class EntityTool : ITool
    {
        bool isEntitying = false;
        bool isSelecting = false;

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && !isEntitying)
            {
                isEntitying = true;
            }
            else if (mouseState.IsRightDown && !isSelecting)
            {
                isSelecting = true;
            }
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            // no-op
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
            var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);

            if (!mouseState.IsLeftDown && isEntitying)
            {
                isEntitying = false;
                if ((map.CurrentEntityBrush.PrefabId ?? "") != "")
                {
                    map.AddEntity(new Entity
                    {
                        PrefabId = map.CurrentEntityBrush.PrefabId,
                        WorldX = mouseWorldX,
                        WorldY = mouseWorldY,
                        Attributes = map.CurrentEntityBrush.Attributes.ToDictionary(kv => kv.Key, kv => kv.Value)
                    });
                }
            }

            if (!mouseState.IsRightDown && isSelecting)
            {
                isSelecting = false;
                map.SelectEntity(mouseWorldX, mouseWorldY);
            }
        }
    }
}
