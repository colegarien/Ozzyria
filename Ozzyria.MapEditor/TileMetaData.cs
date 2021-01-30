using Ozzyria.Game.Component;
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
            { TileType.Water, 0},
            { TileType.Fence, 4},
            { TileType.Road, 8},
            { TileType.Stone, 0},
        };
        private Dictionary<TileType, int> baseY = new Dictionary<TileType, int> {
            { TileType.None, 0},
            { TileType.Ground, 4},
            { TileType.Water, 5},
            { TileType.Fence, 0},
            { TileType.Road, 0},
            { TileType.Stone, 6},
        };

        // higher in the list takes precedence
        private List<TileType> canTransition = new List<TileType>
        {
            TileType.Ground,
            TileType.Stone,
        };

        private Dictionary<TileType, bool> isPathable = new Dictionary<TileType, bool>
        {
            { TileType.None, false},
            { TileType.Ground, false},
            { TileType.Water, false},
            { TileType.Fence, true},
            { TileType.Road, true},
            { TileType.Stone, false},
        };

        public int GetZIndex(TileType type)
        {
            if (type == TileType.Fence)
            {
                return Renderable.Z_FOREGROUND;
            }

            return Renderable.Z_BACKGROUND;
        }

        public Vector2i GetTextureCoordinates(TileType type, PathDirection direction)
        {
            int baseTx = baseX.ContainsKey(type) ? baseX[type] : 0;
            int baseTy = baseY.ContainsKey(type) ? baseY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (isPathable.ContainsKey(type) && isPathable[type])
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

        public Vector2i GetEdgeTransitionTextureCoordinates(TileType type, EdgeTransitionType edgeTransition)
        {
            int baseTx = baseX.ContainsKey(type) ? baseX[type] : 0;
            int baseTy = baseY.ContainsKey(type) ? baseY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (canTransition.Any(t => t == type))
            {
                offsetX = (int)edgeTransition; // cause the fancy bit-mask
            }

            return new Vector2i
            {
                X = baseTx + offsetX,
                Y = baseTy + offsetY
            };
        }

        public Vector2i GetCornerTransitionTextureCoordinates(TileType type, CornerTransitionType cornerTransition)
        {
            int baseTx = baseX.ContainsKey(type) ? baseX[type] : 0;
            int baseTy = baseY.ContainsKey(type) ? baseY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (canTransition.Any(t => t == type))
            {
                offsetY = 1;
                offsetX = (int)cornerTransition; // cause the fancy bit-mask
            }

            return new Vector2i
            {
                X = baseTx + offsetX,
                Y = baseTy + offsetY
            };
        }

        public bool CanTransition(TileType toType, TileType fromType)
        {
            var toIndex = canTransition.IndexOf(toType);
            var fromIndex = canTransition.IndexOf(fromType);
            /* 
             * Is not tranistioning into self
             *  AND the tile being transitioned from is transitionable
             *  AND the tile being transitioned to is not transitionable OR is of lower precedence 
             */
            return toType != fromType && fromIndex != -1
                && (toIndex == -1 || toIndex < fromIndex);
        }

        public TileType[] GetTransitionTypesInPrecedenceOrder()
        {
            return canTransition.ToArray();
        }

        public bool IsPathable(TileType type)
        {
            return isPathable.ContainsKey(type) && isPathable[type];
        }
    }
}
