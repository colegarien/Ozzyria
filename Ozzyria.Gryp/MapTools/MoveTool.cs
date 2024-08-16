
using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class MoveTool : ITool
    {
        public bool isMovingWall = false;
        public bool isMovingEntity = false;

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if(mouseState.IsLeftDown && !isMovingWall && !isMovingEntity)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);

                if (map.SelectedEntity != null && Math.Sqrt(Math.Pow(map.SelectedEntity.WorldX - mouseWorldX, 2) + Math.Pow(map.SelectedEntity.WorldY - mouseWorldY, 2)) <= 16)
                {
                    isMovingEntity = true;
                }
                else if (map.SelectedWall != null && map.SelectedWall.Boundary.Contains(mouseWorldX, mouseWorldY))
                {
                    isMovingWall = true;
                }
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

            if (!mouseState.IsLeftDown && isMovingEntity)
            {
                isMovingEntity = false;
                if (map.SelectedEntity != null)
                {
                    ChangeHistory.StartTracking();
                    ChangeHistory.TrackChange(new EditEntityChange
                    {
                        InternalId = map.SelectedEntity.InternalId,
                        PrefabId = map.SelectedEntity.PrefabId,
                        WorldX = map.SelectedEntity.WorldX,
                        WorldY = map.SelectedEntity.WorldY,
                        Attributes = map.SelectedEntity.Attributes?.Clone() ?? new Model.Types.ValuePacket(),
                    });

                    map.IsDirty = true;
                    map.SelectedEntity.WorldX = mouseWorldX;
                    map.SelectedEntity.WorldY = mouseWorldY;
                    ChangeHistory.FinishTracking();
                }
            }
            else if(!mouseState.IsLeftDown && isMovingWall)
            {
                isMovingWall = false;
                if (map.SelectedWall != null)
                {
                    ChangeHistory.StartTracking();
                    ChangeHistory.TrackChange(new EditWallChange
                    {
                        InternalId = map.SelectedWall.InternalId,
                        Boundary = new WorldBoundary
                        {
                            WorldX = map.SelectedWall.Boundary.WorldX,
                            WorldY = map.SelectedWall.Boundary.WorldY,
                            WorldWidth = map.SelectedWall.Boundary.WorldWidth,
                            WorldHeight = map.SelectedWall.Boundary.WorldHeight
                        }
                    });

                    map.IsDirty = true;
                    map.SelectedWall.Boundary.WorldX = mouseWorldX - (map.SelectedWall.Boundary.WorldWidth / 2f);
                    map.SelectedWall.Boundary.WorldY = mouseWorldY - (map.SelectedWall.Boundary.WorldHeight / 2f);
                    ChangeHistory.FinishTracking();
                }
            }
        }
    }
}
