using Ozzyria.Game.Component;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MapEditor
{
    class TileMetaData
    {
        public string TileSetName { get; set; }
        public List<int> TileTypes { get; set; }
        public IDictionary<int, string> TileNames { get; set; }
        public IDictionary<int, int> BaseTileX { get; set; }
        public IDictionary<int, int> BaseTileY { get; set; }
        public IDictionary<int, int> BaseTileZ { get; set; }

        // ordered lowest precedence to highest precedence
        public IList<int> TilesThatSupportTransitions { get; set; }
        public IList<int> TilesThatSupportPathing { get; set; }
    }


    // TODO OZ-18 : link meta-data with specific Tile Sheet graphics, maybe have 'resources' entry
    // TODO OZ-18 : Make a Content project to manage all this data?
    // TODO OZ-18 : Make a tool or stored data in JSON format for easy tweaking?
    class TileMetaDataFactory
    {
        private TileMetaData currentMetadata;

        public int[] GetTypes()
        {
            InitializeMetaData();
            return currentMetadata.TileTypes.ToArray();
        }

        public int GetZIndex(int type)
        {
            InitializeMetaData();
            return currentMetadata.BaseTileZ.ContainsKey(type)
                ? currentMetadata.BaseTileZ[type]
                : Renderable.Z_BACKGROUND;
        }

        public Vector2i GetTextureCoordinates(int type, PathDirection direction)
        {
            InitializeMetaData();
            int baseTx = currentMetadata.BaseTileX.ContainsKey(type) ? currentMetadata.BaseTileX[type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey(type) ? currentMetadata.BaseTileY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (IsPathable(type))
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

        public Vector2i GetEdgeTransitionTextureCoordinates(int type, EdgeTransitionType edgeTransition)
        {
            InitializeMetaData();
            int baseTx = currentMetadata.BaseTileX.ContainsKey(type) ? currentMetadata.BaseTileX[type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey(type) ? currentMetadata.BaseTileY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (currentMetadata.TilesThatSupportTransitions.Any(t => t == type))
            {
                offsetX = (int)edgeTransition; // cause the fancy bit-mask
            }

            return new Vector2i
            {
                X = baseTx + offsetX,
                Y = baseTy + offsetY
            };
        }

        public Vector2i GetCornerTransitionTextureCoordinates(int type, CornerTransitionType cornerTransition)
        {
            InitializeMetaData();
            int baseTx = currentMetadata.BaseTileX.ContainsKey(type) ? currentMetadata.BaseTileX[type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey(type) ? currentMetadata.BaseTileY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (currentMetadata.TilesThatSupportTransitions.Any(t => t == type))
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

        public bool CanTransition(int toType, int fromType)
        {
            InitializeMetaData();
            var toIndex = currentMetadata.TilesThatSupportTransitions.IndexOf(toType);
            var fromIndex = currentMetadata.TilesThatSupportTransitions.IndexOf(fromType);
            /* 
             * Is not tranistioning into self
             *  AND both tile types are transitionable
             *  AND tile transitioned INTO is lower precedence 
             */
            return toType != fromType
                && fromIndex != -1 && toIndex != -1
                && toIndex < fromIndex;
        }

        public int[] GetTransitionTypesPrecedenceAscending()
        {
            InitializeMetaData();
            return currentMetadata.TilesThatSupportTransitions.Select(t => t).ToArray();
        }

        public bool IsPathable(int type)
        {
            InitializeMetaData();
            return currentMetadata.TilesThatSupportPathing.Any(t => t == type);
        }


        private void InitializeMetaData()
        {
            if (currentMetadata != null)
            {
                // if something is already initialized, don't bother re-intializing
                return;
            }

            // TODO OZ-18 : load from relevant file
            currentMetadata = new TileMetaData();
            currentMetadata.TileTypes = new List<int>
            {
                0, // TODO OZ-18 : make "0" an unexplicit reserved type for "none" universally
                1,
                2,
                3,
                4,
                5
            };
            currentMetadata.TileNames = new Dictionary<int, string>
            {
                { 0, "None" },
                { 1,  "Ground" },
                { 2, "Water" },
                { 3, "Fence" },
                { 4,  "Road" },
                { 5,  "Stone" }
            };
            currentMetadata.BaseTileX = new Dictionary<int, int> {
                { 0, 0},
                { 1, 0},
                { 2, 0},
                { 3, 4},
                { 4, 8},
                { 5, 0},
            };
            currentMetadata.BaseTileY = new Dictionary<int, int> {
                { 0, 0},
                { 1, 4},
                { 2, 5},
                { 3, 0},
                { 4, 0},
                { 5, 6},
            };
            currentMetadata.BaseTileZ = new Dictionary<int, int>
            {
                { 3,  Renderable.Z_FOREGROUND }
            };

            // ordered lowest precedence to highest precedence
            currentMetadata.TilesThatSupportTransitions = new List<int>
            {
                2, // note: lowest in the list doesn't need transition images
                1,
                5,
            };

            currentMetadata.TilesThatSupportPathing = new List<int>
            {
                3,
                4
            };
        }
    }
}
