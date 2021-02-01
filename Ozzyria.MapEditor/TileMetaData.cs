using Ozzyria.Game.Component;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MapEditor
{
    // TODO OZ-18 : try to get rid of hard-coded "TileType"
    // TODO OZ-18 : link meta-data with specific Tile Sheet graphics, maybe have 'resources' entry
    // TODO OZ-18 : Make a Content project to manage all this data?
    // TODO OZ-18 : Make a tool or stored data in JSON format for easy tweaking?
    class TileMetaData
    {
        private Dictionary<TileType, int> baseX;
        private Dictionary<TileType, int> baseY;
        private Dictionary<TileType, int> baseZ;

        // ordered lowest precedence to highest precedence
        private List<TileType> canTransition;
        private Dictionary<TileType, bool> isPathable;

        public int GetZIndex(TileType type)
        {
            InitializeMetaData();
            return baseZ.ContainsKey(type)
                ? baseZ[type]
                : Renderable.Z_BACKGROUND;
        }

        public Vector2i GetTextureCoordinates(TileType type, PathDirection direction)
        {
            InitializeMetaData();
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
            InitializeMetaData();
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
            InitializeMetaData();
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
            InitializeMetaData();
            var toIndex = canTransition.IndexOf(toType);
            var fromIndex = canTransition.IndexOf(fromType);
            /* 
             * Is not tranistioning into self
             *  AND both tile types are transitionable
             *  AND tile transitioned INTO is lower precedence 
             */
            return toType != fromType
                && fromIndex != -1 && toIndex != -1
                && toIndex < fromIndex;
        }

        public TileType[] GetTransitionTypesPrecedenceAscending()
        {
            InitializeMetaData();
            return canTransition.ToArray();
        }

        public bool IsPathable(TileType type)
        {
            InitializeMetaData();
            return isPathable.ContainsKey(type) && isPathable[type];
        }


        private void InitializeMetaData()
        {
            if (baseX != null && baseX.Count <= 0)
            {
                // if something is already initialized, don't bother re-intializing
                return;
            }

            // TODO OZ-18 : load from relevant file
            baseX = new Dictionary<TileType, int> {
                { TileType.None, 0},
                { TileType.Ground, 0},
                { TileType.Water, 0},
                { TileType.Fence, 4},
                { TileType.Road, 8},
                { TileType.Stone, 0},
            };
            baseY = new Dictionary<TileType, int> {
                { TileType.None, 0},
                { TileType.Ground, 4},
                { TileType.Water, 5},
                { TileType.Fence, 0},
                { TileType.Road, 0},
                { TileType.Stone, 6},
            };

            // ordered lowest precedence to highest precedence
            canTransition = new List<TileType>
            {
                TileType.Water, // note: lowest in the list doesn't need transition images
                TileType.Ground,
                TileType.Stone,
            };

            isPathable = new Dictionary<TileType, bool>
            {
                { TileType.Fence, true},
                { TileType.Road, true},
            };

            baseZ = new Dictionary<TileType, int>
            {
                {TileType.Fence,  Renderable.Z_FOREGROUND}
            };
        }
    }
}
