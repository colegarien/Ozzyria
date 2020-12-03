using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MapEditor
{
    class TileMetaData
    {
        private Dictionary<TileType, int> baseX = new Dictionary<TileType, int> {
            { TileType.None, 0},
            { TileType.Ground, 0},
            { TileType.Water, 1},
            { TileType.Fence, 4},
            { TileType.Road, 8},
        };
        private Dictionary<TileType, int> baseY = new Dictionary<TileType, int> {
            { TileType.None, 0},
            { TileType.Ground, 0},
            { TileType.Water, 0},
            { TileType.Fence, 0},
            { TileType.Road, 0},
        };

        private Dictionary<TileType, bool> isTransitionable = new Dictionary<TileType, bool>
        {
            { TileType.None, false},
            { TileType.Ground, false},
            { TileType.Water, true},
            { TileType.Fence, false},
            { TileType.Road, false},
        };
        private Dictionary<TileType, TileType[]> supportedTransitions = new Dictionary<TileType, TileType[]> {
            {TileType.Water, new TileType[]{TileType.Ground } }
        };

        private Dictionary<TileType, bool> isPathable = new Dictionary<TileType, bool>
        {
            { TileType.None, false},
            { TileType.Ground, false},
            { TileType.Water, false},
            { TileType.Fence, true},
            { TileType.Road, true},
        };

        public int GetZIndex(TileType type)
        {
            if(type == TileType.Fence)
            {
                return 10; // TODO OZ-13 : standardize Z-indexes
            }

            return 0;
        }

        public Vector2i GetTextureCoordinates(TileType type, TransitionType transition, PathDirection direction)
        {
            int baseTx = baseX.ContainsKey(type) ? baseX[type] : 0;
            int baseTy = baseY.ContainsKey(type) ? baseY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (IsTransitionable(type))
            {
                switch (transition)
                {
                    case TransitionType.UpLeft:
                        offsetX = 0;
                        offsetY = 0;
                        break;
                    case TransitionType.UpRight:
                        offsetX = 2;
                        offsetY = 0;
                        break;
                    case TransitionType.DownLeft:
                        offsetX = 0;
                        offsetY = 2;
                        break;
                    case TransitionType.DownRight:
                        offsetX = 2;
                        offsetY = 2;
                        break;
                    case TransitionType.Up:
                        offsetX = 1;
                        offsetY = 0;
                        break;
                    case TransitionType.Down:
                        offsetX = 1;
                        offsetY = 2;
                        break;
                    case TransitionType.Left:
                        offsetX = 0;
                        offsetY = 1;
                        break;
                    case TransitionType.Right:
                        offsetX = 2;
                        offsetY = 1;
                        break;
                    case TransitionType.DownRightDiagonal:
                        offsetX = 1;
                        offsetY = 3;
                        break;
                    case TransitionType.DownLeftDiagonal:
                        offsetX = 2;
                        offsetY = 3;
                        break;
                    case TransitionType.UpLeftDiagonal:
                        offsetX = 2;
                        offsetY = 4;
                        break;
                    case TransitionType.UpRightDiagonal:
                        offsetX = 1;
                        offsetY = 4;
                        break;
                    default:
                        offsetX = 1;
                        offsetY = 1;
                        break;
                }
            }
            else if (isPathable.ContainsKey(type) && isPathable[type])
            {
                switch (direction)
                {
                    case PathDirection.Left:
                        offsetX = 2;
                        offsetY = 3;
                        break;
                    case PathDirection.Right:
                        offsetX = 3;
                        offsetY = 3;
                        break;
                    case PathDirection.Up:
                        offsetX = 0;
                        offsetY = 3;
                        break;
                    case PathDirection.Down:
                        offsetX = 1;
                        offsetY = 3;
                        break;
                    case PathDirection.LeftRight:
                        offsetX = 1;
                        offsetY = 0;
                        break;
                    case PathDirection.UpT:
                        offsetX = 3;
                        offsetY = 0;
                        break;
                    case PathDirection.DownT:
                        offsetX = 2;
                        offsetY = 0;
                        break;
                    case PathDirection.UpDown:
                        offsetX = 2;
                        offsetY = 2;
                        break;
                    case PathDirection.LeftT:
                        offsetX = 3;
                        offsetY = 2;
                        break;
                    case PathDirection.RightT:
                        offsetX = 3;
                        offsetY = 1;
                        break;
                    case PathDirection.UpLeft:
                        offsetX = 1;
                        offsetY = 2;
                        break;
                    case PathDirection.UpRight:
                        offsetX = 0;
                        offsetY = 2;
                        break;
                    case PathDirection.DownRight:
                        offsetX = 0;
                        offsetY = 1;
                        break;
                    case PathDirection.DownLeft:
                        offsetX = 1;
                        offsetY = 1;
                        break;
                    case PathDirection.All:
                        offsetX = 0;
                        offsetY = 0;
                        break;
                    default:
                        offsetX = 2;
                        offsetY = 1;
                        break;
                }
            }

            return new Vector2i
            {
                X = baseTx + offsetX,
                Y = baseTy + offsetY
            };
        }

        public bool IsTransitionable(TileType type)
        {
            return isTransitionable.ContainsKey(type) && isTransitionable[type];
        }

        public bool IsSupportedTransition(TileType fromType, TileType toType)
        {
            return IsTransitionable(fromType) && supportedTransitions.ContainsKey(fromType) && supportedTransitions[fromType].Contains(toType);
        }

        public bool IsPathable(TileType type)
        {
            return isPathable.ContainsKey(type) && isPathable[type];
        }
    }
}
