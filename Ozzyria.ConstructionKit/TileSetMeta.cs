using Ozzyria.Game;
using Ozzyria.Game.Components;
using Ozzyria.Game.Persistence;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Ozzyria.ConstructionKit
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


        public Tile CreateTile(int tileType)
        {
            return new Tile
            {
                X = 0,
                Y = 0,
                Z = BaseTileZ.ContainsKey(tileType) ? BaseTileZ[tileType] : (int)ZLayer.Background,
                TextureCoordX = BaseTileX[tileType],
                TextureCoordY = BaseTileY[tileType],
                Decals = new TileDecal[] { },

                Type = tileType,
                EdgeTransition = new Dictionary<int, EdgeTransitionType>(),
                CornerTransition = new Dictionary<int, CornerTransitionType>(),
                Direction = PathDirection.None,
            };
        }

        public bool CanTransition(int toType, int fromType)
        {
            var toIndex = TilesThatSupportTransitions.IndexOf(toType);
            var fromIndex = TilesThatSupportTransitions.IndexOf(fromType);
            /* 
             * Is not tranistioning into self
             *  AND both tile types are transitionable
             *  AND tile transitioned INTO is lower precedence 
             */
            return toType != fromType
                && fromIndex != -1 && toIndex != -1
                && toIndex < fromIndex;
        }

        public TileDecal CreateEdgeTransitionDecal(int type, EdgeTransitionType edgeTransition)
        {
            int baseTx = BaseTileX.ContainsKey(type) ? BaseTileX[type] : 0;
            int baseTy = BaseTileY.ContainsKey(type) ? BaseTileY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (TilesThatSupportTransitions.Any(t => t == type))
            {
                offsetX = (int)edgeTransition; // cause the fancy bit-mask
            }

            return new TileDecal
            {
                TextureCoordX = baseTx + offsetX,
                TextureCoordY = baseTy + offsetY
            };
        }

        public TileDecal CreateCornerTransitionDecal(int type, CornerTransitionType cornerTransition)
        {
            int baseTx = BaseTileX.ContainsKey(type) ? BaseTileX[type] : 0;
            int baseTy = BaseTileY.ContainsKey(type) ? BaseTileY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (TilesThatSupportTransitions.Any(t => t == type))
            {
                offsetY = 1;
                offsetX = (int)cornerTransition; // cause the fancy bit-mask
            }

            return new TileDecal
            {
                TextureCoordX = baseTx + offsetX,
                TextureCoordY = baseTy + offsetY
            };
        }

        public void NormalizeTextureCoordinates(Tile tile)
        {
            int baseTx = BaseTileX.ContainsKey(tile.Type) ? BaseTileX[tile.Type] : 0;
            int baseTy = BaseTileY.ContainsKey(tile.Type) ? BaseTileY[tile.Type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (TilesThatSupportPathing.Contains(tile.Type))
            {
                switch (tile.Direction)
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

            tile.TextureCoordX = baseTx + offsetX;
            tile.TextureCoordY = baseTy + offsetY;
        }

        public int GetWallableCenterXOffset(int type)
        {
            return WallingCenterXOffset.ContainsKey(type)
                ? WallingCenterXOffset[type]
                : 0;
        }

        public int GetWallableCenterYOffset(int type)
        {
            return WallingCenterYOffset.ContainsKey(type)
                ? WallingCenterYOffset[type]
                : 0;
        }

        public int GetWallableThickness(int type)
        {
            return WallingThickness.ContainsKey(type)
                ? WallingThickness[type]
                : 32;
        }
    }

    class TileSetMetaDataFactory
    {
        public static IDictionary<string, TileSetMetaData> tileSetMetaDatas;

        public static void AddNewTileSet(string id)
        {
            tileSetMetaDatas[id] = new TileSetMetaData
            {
                TileTypes = new List<int>(),
                TileNames = new Dictionary<int, string>(),
                BaseTileX = new Dictionary<int, int>(),
                BaseTileY = new Dictionary<int, int>(),
                BaseTileZ = new Dictionary<int, int>(),

                TilesThatSupportTransitions = new List<int>(),
                TilesThatSupportPathing = new List<int>(),

                TilesThatSupportWalling = new List<int>(),
                WallingCenterXOffset = new Dictionary<int, int>(),
                WallingCenterYOffset = new Dictionary<int, int>(),
                WallingThickness = new Dictionary<int, int>()
            };
        }

        public static void AddNewTileType(string tileSetId, int type, string name)
        {
            tileSetMetaDatas[tileSetId].TileTypes.Add(type);
            tileSetMetaDatas[tileSetId].TileNames[type] = name;
            tileSetMetaDatas[tileSetId].BaseTileX[type] = 0;
            tileSetMetaDatas[tileSetId].BaseTileY[type] = 0;
            tileSetMetaDatas[tileSetId].BaseTileZ[type] = (int)ZLayer.Background;
        }

        public static void EnsureInitializedMetaData()
        {
            if (tileSetMetaDatas != null)
            {
                // if something is already initialized, don't bother re-intializing
                return;
            }

            InitializeMetaData();
        }

        public static void InitializeMetaData()
        {
            tileSetMetaDatas = JsonSerializer.Deserialize<IDictionary<string, TileSetMetaData>>(File.ReadAllText(Content.Loader.Root() + "/TileSets/tileset_metadata.json"), JsonOptionsFactory.GetOptions());
        }

        public static void SaveMetaData()
        {
            if (tileSetMetaDatas == null)
            {
                // if nothing is already initialized, don't bother saving
                return;
            }

            File.WriteAllText(Content.Loader.Root() + "/TileSets/tileset_metadata.json", JsonSerializer.Serialize(tileSetMetaDatas, JsonOptionsFactory.GetOptions()));
        }
    }
}
