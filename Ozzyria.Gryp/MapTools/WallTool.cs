using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal enum ResizeHandleMode
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
    }
    internal class ResizeHandle
    {
        public const float HANDLE_DIMENSION = 12;

        public ResizeHandleMode Mode { get; set; }
        public WorldBoundary ActivationArea { get; set; } = new WorldBoundary { WorldWidth = HANDLE_DIMENSION, WorldHeight = HANDLE_DIMENSION };

        public void Attach(WorldBoundary boundary)
        {
            switch (Mode)
            {
                case ResizeHandleMode.Up:
                    ActivationArea.MoveCenterTo(boundary.WorldX + (boundary.WorldWidth / 2f), boundary.WorldY);
                    break;
                case ResizeHandleMode.Down:
                    ActivationArea.MoveCenterTo(boundary.WorldX + (boundary.WorldWidth / 2f), boundary.WorldY + boundary.WorldHeight);
                    break;
                case ResizeHandleMode.Left:
                    ActivationArea.MoveCenterTo(boundary.WorldX, boundary.WorldY + (boundary.WorldHeight / 2f));
                    break;
                case ResizeHandleMode.Right:
                    ActivationArea.MoveCenterTo(boundary.WorldX + boundary.WorldWidth, boundary.WorldY + (boundary.WorldHeight / 2f));
                    break;
                case ResizeHandleMode.UpLeft:
                    ActivationArea.MoveCenterTo(boundary.WorldX, boundary.WorldY);
                    break;
                case ResizeHandleMode.UpRight:
                    ActivationArea.MoveCenterTo(boundary.WorldX + boundary.WorldWidth, boundary.WorldY);
                    break;
                case ResizeHandleMode.DownLeft:
                    ActivationArea.MoveCenterTo(boundary.WorldX, boundary.WorldY + boundary.WorldHeight);
                    break;
                case ResizeHandleMode.DownRight:
                    ActivationArea.MoveCenterTo(boundary.WorldX + boundary.WorldWidth, boundary.WorldY + boundary.WorldHeight);
                    break;

            }
        }

    }

    internal class WallTool : IAreaTool
    {
        public bool isResizing = false;
        private bool isSelecting = false;

        public ResizeHandle? SelectedHandle = null;
        public ResizeHandle[] Handles =
        {
            new ResizeHandle{ Mode = ResizeHandleMode.Up},
            new ResizeHandle{ Mode = ResizeHandleMode.Down},
            new ResizeHandle{ Mode = ResizeHandleMode.Left},
            new ResizeHandle{ Mode = ResizeHandleMode.Right},
            new ResizeHandle{ Mode = ResizeHandleMode.UpLeft},
            new ResizeHandle{ Mode = ResizeHandleMode.UpRight},
            new ResizeHandle{ Mode = ResizeHandleMode.DownLeft},
            new ResizeHandle{ Mode = ResizeHandleMode.DownRight},
        };
        
        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
            var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
            if (mouseState.IsLeftDown && map.SelectedWall != null && !isResizing && Handles.Any(h => h.ActivationArea.Contains(mouseWorldX, mouseWorldY)))
            {
                // handle resize handles
                isResizing = true;
                trackingAreaSelect = true;

                Area.WorldX = map.SelectedWall.Boundary.WorldX;
                Area.WorldY = map.SelectedWall.Boundary.WorldY;
                Area.WorldWidth = map.SelectedWall.Boundary.WorldWidth;
                Area.WorldHeight = map.SelectedWall.Boundary.WorldHeight;
            }
            else
            {
                base.OnMouseDown(mouseState, camera, map);
            }

            if(mouseState.IsRightDown && !isSelecting)
            {
                isSelecting = true;
            }

            AttachHandles(mouseState, camera, map);
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && isResizing && SelectedHandle != null)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);

                // horizontal resize
                switch (SelectedHandle.Mode) {
                    case ResizeHandleMode.Right:
                    case ResizeHandleMode.UpRight:
                    case ResizeHandleMode.DownRight:
                        // expand right edge
                        Area.WorldWidth = mouseWorldX - Area.WorldX;
                        break;
                    case ResizeHandleMode.Left:
                    case ResizeHandleMode.UpLeft:
                    case ResizeHandleMode.DownLeft:
                        // expand left edge
                        var currentRightEdge = Area.WorldX + Area.WorldWidth;
                        Area.WorldX = mouseWorldX;
                        Area.WorldWidth = currentRightEdge - Area.WorldX;
                        break;
                }

                // vertical resize
                switch (SelectedHandle.Mode) {
                    case ResizeHandleMode.Down:
                    case ResizeHandleMode.DownLeft:
                    case ResizeHandleMode.DownRight:
                        Area.WorldHeight = mouseWorldY - Area.WorldY;
                        break;
                    case ResizeHandleMode.Up:
                    case ResizeHandleMode.UpLeft:
                    case ResizeHandleMode.UpRight:
                        var currentBottomEdge = Area.WorldY + Area.WorldHeight;
                        Area.WorldY = mouseWorldY;
                        Area.WorldHeight = currentBottomEdge - Area.WorldY;
                        break;
                }
            }
            else
            {
                base.OnMouseMove(mouseState, camera, map);
            }

            AttachHandles(mouseState, camera, map);
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            base.OnMouseUp(mouseState, camera, map);

            if(!mouseState.IsRightDown && isSelecting)
            {
                ChangeHistory.StartTracking();
                isSelecting = false;

                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                map.SelectWall(mouseWorldX, mouseWorldY);
                ChangeHistory.FinishTracking();
            }

            AttachHandles(mouseState, camera, map);
        }


        protected override void OnCancel(MouseState mouseState, Camera camera, Map map)
        {
            if (isResizing)
            {
                ChangeHistory.StartTracking();
                isResizing = false;
                if (map.SelectedWall != null)
                {
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
                    map.SelectedWall.Boundary = GetWorldArea();
                }
                ChangeHistory.FinishTracking();
            }
            else
            {
                ChangeHistory.StartTracking();
                map.UnselectWall();
                ChangeHistory.FinishTracking();
            }
        }

        protected override void OnComplete(MouseState mouseState, Camera camera, Map map)
        {
            ChangeHistory.StartTracking();
            if (isResizing)
            {
                isResizing = false;
                if (map.SelectedWall != null)
                {
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
                    map.SelectedWall.Boundary = GetWorldArea();
                }
            }
            else
            {
                map.AddWall(new Wall { Boundary = GetWorldArea() });
            }
            ChangeHistory.FinishTracking();
        }


        public void PlaceHandles(Map map)
        {
            if(map.SelectedWall != null)
            {
                foreach (var handle in Handles)
                {
                    handle.Attach(isResizing ? Area : map.SelectedWall.Boundary);
                }
            }
        }
        private void AttachHandles(MouseState mouseState, Camera camera, Map map)
        {
            if (map.SelectedWall != null)
            {
                PlaceHandles(map);
                if (!isResizing)
                {
                    var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                    var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                    SelectedHandle = Handles
                        .Where(h => h.ActivationArea.Contains(mouseWorldX, mouseWorldY))
                        .OrderBy(h => Math.Sqrt(Math.Pow(h.ActivationArea.WorldX + (h.ActivationArea.WorldWidth / 2f) - mouseWorldX, 2) + Math.Pow(h.ActivationArea.WorldY + (h.ActivationArea.WorldHeight / 2f) - mouseWorldY, 2)))
                        .FirstOrDefault();
                }
            }
        }
    }
}
