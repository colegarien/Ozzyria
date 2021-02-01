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
        public IDictionary<int, int> BaseTileX { get; set; }
        public IDictionary<int, int> BaseTileY { get; set; }
        public IDictionary<int, int> BaseTileZ { get; set; }

        // ordered lowest precedence to highest precedence
        public IList<int> TilesThatSupportTransitions { get; set; }
        public IList<int> TilesThatSupportPathing { get; set; }
    }


    // TODO OZ-18 : try to get rid of hard-coded "TileType"
    // TODO OZ-18 : link meta-data with specific Tile Sheet graphics, maybe have 'resources' entry
    // TODO OZ-18 : Make a Content project to manage all this data?
    // TODO OZ-18 : Make a tool or stored data in JSON format for easy tweaking?
    class TileMetaDataFactory
    {
        private TileMetaData currentMetadata;

        public int GetZIndex(TileType type)
        {
            InitializeMetaData();
            return currentMetadata.BaseTileZ.ContainsKey((int)type)
                ? currentMetadata.BaseTileZ[(int)type]
                : Renderable.Z_BACKGROUND;
        }

        public Vector2i GetTextureCoordinates(TileType type, PathDirection direction)
        {
            InitializeMetaData();
            int baseTx = currentMetadata.BaseTileX.ContainsKey((int)type) ? currentMetadata.BaseTileX[(int)type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey((int)type) ? currentMetadata.BaseTileY[(int)type] : 0;
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

        public Vector2i GetEdgeTransitionTextureCoordinates(TileType type, EdgeTransitionType edgeTransition)
        {
            InitializeMetaData();
            int baseTx = currentMetadata.BaseTileX.ContainsKey((int)type) ? currentMetadata.BaseTileX[(int)type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey((int)type) ? currentMetadata.BaseTileY[(int)type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (currentMetadata.TilesThatSupportTransitions.Any(t => t == (int)type))
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
            int baseTx = currentMetadata.BaseTileX.ContainsKey((int)type) ? currentMetadata.BaseTileX[(int)type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey((int)type) ? currentMetadata.BaseTileY[(int)type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (currentMetadata.TilesThatSupportTransitions.Any(t => t == (int)type))
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
            var toIndex = currentMetadata.TilesThatSupportTransitions.IndexOf((int)toType);
            var fromIndex = currentMetadata.TilesThatSupportTransitions.IndexOf((int)fromType);
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
            return currentMetadata.TilesThatSupportTransitions.Select(t => (TileType)t).ToArray();
        }

        public bool IsPathable(TileType type)
        {
            InitializeMetaData();
            return currentMetadata.TilesThatSupportPathing.Any(t => t == (int)type);
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
            currentMetadata.BaseTileX = new Dictionary<int, int> {
                { (int)TileType.None, 0},
                { (int)TileType.Ground, 0},
                { (int)TileType.Water, 0},
                { (int)TileType.Fence, 4},
                { (int)TileType.Road, 8},
                { (int)TileType.Stone, 0},
            };
            currentMetadata.BaseTileY = new Dictionary<int, int> {
                { (int)TileType.None, 0},
                { (int)TileType.Ground, 4},
                { (int)TileType.Water, 5},
                { (int)TileType.Fence, 0},
                { (int)TileType.Road, 0},
                { (int)TileType.Stone, 6},
            };
            currentMetadata.BaseTileZ = new Dictionary<int, int>
            {
                { (int)TileType.Fence,  Renderable.Z_FOREGROUND }
            };

            // ordered lowest precedence to highest precedence
            currentMetadata.TilesThatSupportTransitions = new List<int>
            {
                (int)TileType.Water, // note: lowest in the list doesn't need transition images
                (int)TileType.Ground,
                (int)TileType.Stone,
            };

            currentMetadata.TilesThatSupportPathing = new List<int>
            {
                (int)TileType.Fence,
                (int)TileType.Road
            };
        }
    }
}
