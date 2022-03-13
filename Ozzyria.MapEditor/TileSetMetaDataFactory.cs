using Ozzyria.Game.Component;
using Ozzyria.Game.Persistence;
using SFML.System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ozzyria.MapEditor
{
    class TileSetMetaData
    {
        public List<int> TileTypes { get; set; }
        public IDictionary<int, string> TileNames { get; set; }
        public IDictionary<int, int> BaseTileX { get; set; }
        public IDictionary<int, int> BaseTileY { get; set; }
        public IDictionary<int, int> BaseTileZ { get; set; }

        // ordered lowest precedence to highest precedence
        public IList<int> TilesThatSupportTransitions { get; set; }
        public IList<int> TilesThatSupportPathing { get; set; }

        public IList<int> TilesThatSupportWalling { get; set; }
        public IDictionary<int, int> WallingCenterXOffset { get; set; }
        public IDictionary<int, int> WallingCenterYOffset { get; set; }
        public IDictionary<int, int> WallingThickness { get; set; }
    }

    class TileSetMetaDataFactory
    {
        private IDictionary<string, TileSetMetaData> tileSetMetaDatas;

        private string currentTileSet = "outside_tileset_001";
        private TileSetMetaData currentMetadata;

        public void SetCurrentTileSet(string tileSet)
        {
            InitializeMetaData();
            if (tileSetMetaDatas.ContainsKey(tileSet))
            {
                currentTileSet = tileSet;
                currentMetadata = tileSetMetaDatas[currentTileSet];
            }
        }

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

            // OZ-17 : move these text coordinates (and cooridnates for transtions) into the meta data!!!!!!
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

        public bool IsWallable(int type)
        {
            InitializeMetaData();
            return currentMetadata.TilesThatSupportWalling.Any(t => t == type);
        }

        public int GetWallableCenterXOffset(int type)
        {
            InitializeMetaData();
            return currentMetadata.WallingCenterXOffset.ContainsKey(type)
                ? currentMetadata.WallingCenterXOffset[type]
                : 0;
        }

        public int GetWallableCenterYOffset(int type)
        {
            InitializeMetaData();
            return currentMetadata.WallingCenterYOffset.ContainsKey(type)
                ? currentMetadata.WallingCenterYOffset[type]
                : 0;
        }

        public int GetWallableThickness(int type)
        {
            InitializeMetaData();
            return currentMetadata.WallingThickness.ContainsKey(type)
                ? currentMetadata.WallingThickness[type]
                : 32;
        }


        public int GetTileType(int textureCoordinateX, int textureCoordinateY)
        {
            InitializeMetaData();
            var possibleXTypes = new List<int>();
            foreach(var kv in currentMetadata.BaseTileX)
            {
                if(textureCoordinateX == kv.Value)
                {
                    possibleXTypes.Add(kv.Key);
                }
            }

            foreach (var kv in currentMetadata.BaseTileY)
            {
                if (textureCoordinateY == kv.Value && possibleXTypes.Contains(kv.Key))
                {
                    return kv.Key;
                }
            }


            // try for nasty pathables
            possibleXTypes = new List<int>();
            foreach (var kv in currentMetadata.BaseTileX)
            {
                if (kv.Value <= textureCoordinateX && textureCoordinateX <= kv.Value + 3)
                {
                    possibleXTypes.Add(kv.Key);
                }
            }

            foreach (var kv in currentMetadata.BaseTileY)
            {
                if (kv.Value <= textureCoordinateY && textureCoordinateY <= kv.Value + 3 && possibleXTypes.Contains(kv.Key))
                {
                    return kv.Key;
                }
            }

            return 0;
        }


        private void InitializeMetaData()
        {
            if (tileSetMetaDatas != null)
            {
                // if something is already initialized, don't bother re-intializing
                return;
            }

            tileSetMetaDatas = JsonSerializer.Deserialize<IDictionary<string, TileSetMetaData>>(File.ReadAllText(Content.Loader.Root() + "/TileSets/tileset_metadata.json"), JsonOptionsFactory.GetOptions());
            if (tileSetMetaDatas.ContainsKey(currentTileSet))
            {
                currentMetadata = tileSetMetaDatas[currentTileSet];
            }
        }
    }

}
