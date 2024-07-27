﻿using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class EntityTool : ITool
    {
        // TODO make this configurable
        public string PrefabId { get; set; } = "slime_spawner";

        bool isEntitying = false;
        bool isErasing = false;

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && !isEntitying)
            {
                isEntitying = true;
            }
            else if (mouseState.IsRightDown && !isErasing)
            {
                isErasing = true;
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
                map.AddEntity(new Entity
                {
                    PrefabId = PrefabId,
                    WorldX = mouseWorldX,
                    WorldY = mouseWorldY,
                });
            }

            if (!mouseState.IsRightDown && isErasing)
            {
                isErasing = false;
                map.RemoveEntities(mouseWorldX, mouseWorldY);
            }
        }
    }
}