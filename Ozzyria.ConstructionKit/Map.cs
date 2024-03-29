﻿using Ozzyria.Game;
using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ozzyria.ConstructionKit
{
    static class MapExtensions
    {
        public static void AddLayer(this TileMap map)
        {
            var newLayer = 0;
            if (map.Layers.Keys.Count != 0)
                newLayer = map.Layers.Keys.Max() + 1;

            if (newLayer >= 256)
                return;

            map.Layers[newLayer] = new List<Tile>();
        }

        public static void RemoveLayer(this TileMap map, int layer)
        {
            if (!map.Layers.ContainsKey(layer) || map.Layers.Keys.Count <= 1)
                return;

            var lastLayer = map.Layers.Keys.Max();
            map.Layers.Remove(layer);
            for (var i = layer + 1; i <= lastLayer; i++)
            {
                var currentLayer = map.Layers[i];
                map.Layers[i - 1] = currentLayer;
                map.Layers.Remove(i);
            }
        }

        public static void RemoveTile(this TileMap map, int layer, int x, int y)
        {
            if (map.HasLayer(layer))
                map.Layers[layer].RemoveAll(t => t.X == x && t.Y == y);
        }

        public static void PaintTile(this TileMap map, MapMetaData mapMeta, TileSetMetaData tileSetMeta, int layer, int x, int y, int tileType)
        {
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height
                || layer < 0 || layer >= mapMeta.Layers
                || !tileSetMeta.TileTypes.Any(t => t == tileType))
            {
                return;
            }

            if (tileType == 0)
            {
                map.RemoveTile(layer, x, y);
                return;
            }

            if (!map.HasLayer(layer))
                map.Layers[layer] = new List<Tile>();

            var tileIndex = map.Layers[layer].FindIndex(t => t.X == x && t.Y == y);
            if(tileIndex == -1)
            {
                var newTile = tileSetMeta.CreateTile(tileType);
                newTile.X = x;
                newTile.Y = y;
                map.Layers[layer].Add(newTile);

                return;
            }

            var tile = map.Layers[layer][tileIndex];

            tile.Type = tileType;
            tile.TextureCoordX = tileSetMeta.BaseTileX[tileType];
            tile.TextureCoordY = tileSetMeta.BaseTileY[tileType];
            tile.Z = tileSetMeta.BaseTileZ.ContainsKey(tileType) ? tileSetMeta.BaseTileZ[tileType] : 0;
            
            tile.Decals = new TileDecal[] { };
            tile.EdgeTransition = new Dictionary<int, EdgeTransitionType>();
            tile.CornerTransition = new Dictionary<int, CornerTransitionType>();
            tile.Direction = PathDirection.None;
        }

        public static void FillTile(this TileMap map, MapMetaData mapMeta, TileSetMetaData tileSetMeta, int layer, int x, int y, int tileType)
        {
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height
                || layer < 0 || layer >= mapMeta.Layers
                || !tileSetMeta.TileTypes.Any(t => t == tileType))
            {
                return;
            }

            var currentTileType = map.Layers[layer].FirstOrDefault(t => t.X == x && t.Y == y)?.Type ?? 0;
            if (currentTileType == tileType)
            {
                return;
            }

            map.FillRecursive(mapMeta, tileSetMeta, layer, x, y, tileType, currentTileType);
        }

        public static void FillRecursive(this TileMap map, MapMetaData mapMeta, TileSetMetaData tileSetMeta, int layer, int x, int y, int toFillWith, int toReplace)
        {
            var currentTileType = map.Layers[layer].FirstOrDefault(t => t.X == x && t.Y == y)?.Type ?? 0;
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height
                || layer < 0 || layer >= mapMeta.Layers
                || !tileSetMeta.TileTypes.Any(t => t == toFillWith)
                || currentTileType != toReplace
                || currentTileType == toFillWith)
            {
                return;
            }

            map.PaintTile(mapMeta, tileSetMeta, layer, x, y, toFillWith);
            map.FillRecursive(mapMeta, tileSetMeta, layer, x - 1, y, toFillWith, toReplace);
            map.FillRecursive(mapMeta, tileSetMeta, layer, x + 1, y, toFillWith, toReplace);
            map.FillRecursive(mapMeta, tileSetMeta, layer, x, y - 1, toFillWith, toReplace);
            map.FillRecursive(mapMeta, tileSetMeta, layer, x, y + 1, toFillWith, toReplace);
        }


        public static void Bake(this TileMap map, MapMetaData mapMeta, TileSetMetaData tileSetMeta)
        {
            for (var layer = 0; layer < mapMeta.Layers; layer++)
            {
                if (!map.HasLayer(layer))
                    continue;

                for (var x = 0; x < mapMeta.Width; x++)
                {
                    for (var y = 0; y < mapMeta.Height; y++)
                    {
                        var tileIndex = map.Layers[layer].FindIndex(t => t.X == x && t.Y == y);
                        if (tileIndex == -1)
                            continue;

                        var tile = map.Layers[layer][tileIndex];

                        // reset before recalculating
                        tile.EdgeTransition = new Dictionary<int, EdgeTransitionType>();
                        tile.CornerTransition = new Dictionary<int, CornerTransitionType>();
                        tile.Direction = PathDirection.None;

                        var tileType = tile.Type;
                        if (tileSetMeta.TilesThatSupportPathing.Contains(tileType))
                        {
                            var leftIsPath = map.Layers[layer].Any(t => t.Type == tileType && t.X == x - 1 && t.Y == y);
                            var rightIsPath = map.Layers[layer].Any(t => t.Type == tileType && t.X == x + 1 && t.Y == y);
                            var upIsPath = map.Layers[layer].Any(t => t.Type == tileType && t.X == x && t.Y == y - 1);
                            var downIsPath = map.Layers[layer].Any(t => t.Type == tileType && t.X == x && t.Y == y + 1);

                            if (leftIsPath && !rightIsPath && !upIsPath && !downIsPath)
                            {
                                tile.Direction = PathDirection.Left;
                            }
                            else if (!leftIsPath && rightIsPath && !upIsPath && !downIsPath)
                            {
                                tile.Direction = PathDirection.Right;
                            }
                            else if (!leftIsPath && !rightIsPath && upIsPath && !downIsPath)
                            {
                                tile.Direction = PathDirection.Up;
                            }
                            else if (!leftIsPath && !rightIsPath && !upIsPath && downIsPath)
                            {
                                tile.Direction = PathDirection.Down;
                            }
                            else if (leftIsPath && rightIsPath && !upIsPath && !downIsPath)
                            {
                                tile.Direction = PathDirection.LeftRight;
                            }
                            else if (leftIsPath && rightIsPath && upIsPath && !downIsPath)
                            {
                                tile.Direction = PathDirection.UpT;
                            }
                            else if (leftIsPath && rightIsPath && !upIsPath && downIsPath)
                            {
                                tile.Direction = PathDirection.DownT;
                            }
                            else if (!leftIsPath && !rightIsPath && upIsPath && downIsPath)
                            {
                                tile.Direction = PathDirection.UpDown;
                            }
                            else if (leftIsPath && !rightIsPath && upIsPath && downIsPath)
                            {
                                tile.Direction = PathDirection.LeftT;
                            }
                            else if (!leftIsPath && rightIsPath && upIsPath && downIsPath)
                            {
                                tile.Direction = PathDirection.RightT;
                            }
                            else if (leftIsPath && !rightIsPath && upIsPath && !downIsPath)
                            {
                                tile.Direction = PathDirection.UpLeft;
                            }
                            else if (!leftIsPath && rightIsPath && upIsPath && !downIsPath)
                            {
                                tile.Direction = PathDirection.UpRight;
                            }
                            else if (!leftIsPath && rightIsPath && !upIsPath && downIsPath)
                            {
                                tile.Direction = PathDirection.DownRight;
                            }
                            else if (leftIsPath && !rightIsPath && !upIsPath && downIsPath)
                            {
                                tile.Direction = PathDirection.DownLeft;
                            }
                            else if (leftIsPath && rightIsPath && upIsPath && downIsPath)
                            {
                                tile.Direction = PathDirection.All;
                            }

                            tileSetMeta.NormalizeTextureCoordinates(tile);
                        }
                        else
                        {
                            // This works via 'bit-mask' math, the enum is very particularlly crafted
                            var leftTileType = map.Layers[layer].FirstOrDefault(t => t.X == x - 1 && t.Y == y)?.Type ?? 0;
                            var leftIsTransitionable = tileSetMeta.CanTransition(tileType, leftTileType);

                            var rightTileType = map.Layers[layer].FirstOrDefault(t => t.X == x + 1 && t.Y == y)?.Type ?? 0;
                            var rightIsTransitionable = tileSetMeta.CanTransition(tileType, rightTileType);

                            var upTileType = map.Layers[layer].FirstOrDefault(t => t.X == x && t.Y == y - 1)?.Type ?? 0;
                            var upIsTransitionable = tileSetMeta.CanTransition(tileType, upTileType);

                            var downTileType = map.Layers[layer].FirstOrDefault(t => t.X == x && t.Y == y + 1)?.Type ?? 0;
                            var downIsTransitionable = tileSetMeta.CanTransition(tileType, downTileType);

                            if (upIsTransitionable)
                            {
                                if (!tile.EdgeTransition.ContainsKey(upTileType))
                                    tile.EdgeTransition[upTileType] = EdgeTransitionType.None;
                                tile.EdgeTransition[upTileType] = (EdgeTransitionType)((int)tile.EdgeTransition[upTileType] + (int)EdgeTransitionType.Up);
                            }
                            if (downIsTransitionable)
                            {
                                if (!tile.EdgeTransition.ContainsKey(downTileType))
                                    tile.EdgeTransition[downTileType] = EdgeTransitionType.None;
                                tile.EdgeTransition[downTileType] = (EdgeTransitionType)((int)tile.EdgeTransition[downTileType] + (int)EdgeTransitionType.Down);
                            }
                            if (leftIsTransitionable)
                            {
                                if (!tile.EdgeTransition.ContainsKey(leftTileType))
                                    tile.EdgeTransition[leftTileType] = EdgeTransitionType.None;
                                tile.EdgeTransition[leftTileType] = (EdgeTransitionType)((int)tile.EdgeTransition[leftTileType] + (int)EdgeTransitionType.Left);
                            }
                            if (rightIsTransitionable)
                            {
                                if (!tile.EdgeTransition.ContainsKey(rightTileType))
                                    tile.EdgeTransition[rightTileType] = EdgeTransitionType.None;
                                tile.EdgeTransition[rightTileType] = (EdgeTransitionType)((int)tile.EdgeTransition[rightTileType] + (int)EdgeTransitionType.Right);
                            }

                            // This works via 'bit-mask' math, the enum is very particularlly crafted 
                            // TODO : Consider adding checks for current edges transition to avoid rednundantly adding corner transitions on top of them 
                            var upLeftTileType = map.Layers[layer].FirstOrDefault(t => t.X == x - 1 && t.Y == y - 1)?.Type ?? 0;
                            var upLeftIsTransitionable = tileSetMeta.CanTransition(tileType, upLeftTileType);

                            var upRightTileType = map.Layers[layer].FirstOrDefault(t => t.X == x + 1 && t.Y == y - 1)?.Type ?? 0;
                            var upRightIsTransitionable = tileSetMeta.CanTransition(tileType, upRightTileType);

                            var downLeftTileType = map.Layers[layer].FirstOrDefault(t => t.X == x - 1 && t.Y == y + 1)?.Type ?? 0;
                            var downLeftIsTransitionable = tileSetMeta.CanTransition(tileType, downLeftTileType);

                            var downRightTileType = map.Layers[layer].FirstOrDefault(t => t.X == x + 1 && t.Y == y + 1)?.Type ?? 0;
                            var downRightIsTransitionable = tileSetMeta.CanTransition(tileType, downRightTileType);

                            if (upLeftIsTransitionable)
                            {
                                if (!tile.CornerTransition.ContainsKey(upLeftTileType))
                                    tile.CornerTransition[upLeftTileType] = CornerTransitionType.None;
                                tile.CornerTransition[upLeftTileType] = (CornerTransitionType)((int)tile.CornerTransition[upLeftTileType] + (int)CornerTransitionType.UpLeft);
                            }
                            if (upRightIsTransitionable)
                            {
                                if (!tile.CornerTransition.ContainsKey(upRightTileType))
                                    tile.CornerTransition[upRightTileType] = CornerTransitionType.None;
                                tile.CornerTransition[upRightTileType] = (CornerTransitionType)((int)tile.CornerTransition[upRightTileType] + (int)CornerTransitionType.UpRight);
                            }
                            if (downLeftIsTransitionable)
                            {
                                if (!tile.CornerTransition.ContainsKey(downLeftTileType))
                                    tile.CornerTransition[downLeftTileType] = CornerTransitionType.None;
                                tile.CornerTransition[downLeftTileType] = (CornerTransitionType)((int)tile.CornerTransition[downLeftTileType] + (int)CornerTransitionType.DownLeft);
                            }
                            if (downRightIsTransitionable)
                            {
                                if (!tile.CornerTransition.ContainsKey(downRightTileType))
                                    tile.CornerTransition[downRightTileType] = CornerTransitionType.None;
                                tile.CornerTransition[downRightTileType] = (CornerTransitionType)((int)tile.CornerTransition[downRightTileType] + (int)CornerTransitionType.DownRight);
                            }

                            tile.Decals = BuildTileDecals(tile, tileSetMeta);
                        }
                    }
                }
            }
        }

        public static TileDecal[] BuildTileDecals(Tile tile, TileSetMetaData tileSetMeta)
        {
            var decals = new List<TileDecal>();

            var cornerTransitions = tile.CornerTransition;
            var edgeTransitions = tile.EdgeTransition;

            foreach (var transitionTileType in tileSetMeta.TilesThatSupportTransitions)
            {
                if (cornerTransitions.ContainsKey(transitionTileType))
                {
                    var cornerTransition = cornerTransitions[transitionTileType];
                    if (cornerTransition != CornerTransitionType.None)
                        decals.Add(tileSetMeta.CreateCornerTransitionDecal(transitionTileType, cornerTransition));
                }

                if (edgeTransitions.ContainsKey(transitionTileType))
                {
                    var edgeTransition = edgeTransitions[transitionTileType];
                    if (edgeTransition != EdgeTransitionType.None)
                        decals.Add(tileSetMeta.CreateEdgeTransitionDecal(transitionTileType, edgeTransition));
                }
            }

            return decals.ToArray();
        }
    }

    class MapFactory
    {
        public static IDictionary<string, TileMap> loadedMaps = new Dictionary<string, TileMap>();
        public static string lastUsedMap = "";

        public static TileMap NewMap(string mapName)
        {
            loadedMaps[mapName] = new TileMap{ Name = mapName, };
            loadedMaps[mapName].AddLayer();

            lastUsedMap = mapName;
            return loadedMaps[mapName];
        }

        public static bool MapExists(string mapName)
        {
            return loadedMaps.ContainsKey(mapName) || File.Exists(Content.Loader.Root() + "/Maps/" + mapName + ".ozz");
        }

        public static void Reinitialize()
        {
            loadedMaps.Clear();
        }

        public static TileMap LoadMap(string mapName)
        {
            if (loadedMaps.ContainsKey(mapName))
            {
                return loadedMaps[mapName];
            }

            var persistence = new WorldPersistence();
            loadedMaps[mapName] = persistence.LoadMap(mapName);
            loadedMaps[mapName].Name = mapName;

            return loadedMaps[mapName];
        }

        public static void SaveMaps()
        {
            var persistence = new WorldPersistence();

            foreach(var mapGroup in loadedMaps)
            {
                var mapName = mapGroup.Key;
                var map = mapGroup.Value;
                persistence.SaveMap(mapGroup.Key, mapGroup.Value);

                // TODO OZ-6 stop doing this (everything below this line)

                var mapMeta = MapMetaDataFactory.mapMetaDatas[mapName];
                var tileSetMeta = TileSetMetaDataFactory.tileSetMetaDatas[map.TileSet];

                var context = new EntityContext();

                EntityFactory.CreateExperienceOrb(context, 400, 300, 30);
                EntityFactory.CreateSlimeSpawner(context, 500, 400);
                EntityFactory.CreateDoor(context, 550, 550, "test_b", 140, 140);

                // build colliders for collideable tiles
                for (var layer = 0; layer < mapMeta.Layers; layer++)
                {
                    if (!map.HasLayer(layer))
                        continue;

                    // build vertical collisions
                    for (var x = 0; x < mapMeta.Width; x++)
                    {
                        int startType = -1;
                        int startY = -1;
                        int endY = -1;
                        for (var y = 0; y < mapMeta.Height; y++)
                        {
                            var tile = map.Layers[layer].FirstOrDefault(t => t.X == x && t.Y == y);
                            var tileType = tile?.Type ?? 0;
                            if (tileSetMeta.TilesThatSupportWalling.Contains(tileType))
                            {
                                var direction = tile?.Direction ?? PathDirection.None;
                                if (direction == PathDirection.All
                                    || direction == PathDirection.Down
                                    || direction == PathDirection.DownLeft
                                    || direction == PathDirection.DownRight
                                    || direction == PathDirection.DownT
                                    || direction == PathDirection.UpT
                                    || direction == PathDirection.LeftT
                                    || direction == PathDirection.RightT
                                    || direction == PathDirection.Up
                                    || direction == PathDirection.UpDown
                                    || direction == PathDirection.UpLeft
                                    || direction == PathDirection.UpRight)
                                {
                                    if (tileType != startType && startY != -1 && endY != -1)
                                    {
                                        // starting new string of wall types (collision could be a different size)
                                        CreateVerticalBoxCollider(context, tileSetMeta, startType, x, startY, endY);
                                        startType = -1;
                                        startY = -1;
                                        endY = -1;
                                    }

                                    if (startY == -1)
                                    {
                                        startType = tileType;
                                        startY = y;
                                    }
                                    endY = y;
                                }
                                else if (startY != -1 && endY != -1)
                                {
                                    CreateVerticalBoxCollider(context, tileSetMeta, startType, x, startY, endY);
                                    startType = -1;
                                    startY = -1;
                                    endY = -1;
                                }
                            }
                            else
                            {
                                if (startY != -1 && endY != -1)
                                {
                                    CreateVerticalBoxCollider(context, tileSetMeta, startType, x, startY, endY);
                                }

                                startType = -1;
                                startY = -1;
                                endY = -1;
                            }
                        }

                        if (startY != -1 && endY != -1)
                        {
                            CreateVerticalBoxCollider(context, tileSetMeta, startType, x, startY, endY);
                        }
                    }

                    // build horizontal collisions
                    for (var y = 0; y < mapMeta.Height; y++)
                    {
                        int startType = -1;
                        int startX = -1;
                        int endX = -1;
                        for (var x = 0; x < mapMeta.Width; x++)
                        {
                            var tile = map.Layers[layer].FirstOrDefault(t => t.X == x && t.Y == y);
                            var tileType = tile?.Type ?? 0 ;
                            if (tileSetMeta.TilesThatSupportWalling.Contains(tileType))
                            {
                                var direction = tile?.Direction ?? PathDirection.None;
                                if (direction == PathDirection.None
                                    || direction == PathDirection.All
                                    || direction == PathDirection.Left
                                    || direction == PathDirection.DownLeft
                                    || direction == PathDirection.DownRight
                                    || direction == PathDirection.DownT
                                    || direction == PathDirection.UpT
                                    || direction == PathDirection.LeftT
                                    || direction == PathDirection.RightT
                                    || direction == PathDirection.Right
                                    || direction == PathDirection.LeftRight
                                    || direction == PathDirection.UpLeft
                                    || direction == PathDirection.UpRight)
                                {
                                    if (tileType != startType && startX != -1 && endX != -1)
                                    {
                                        // starting new string of wall types (collision could be a different size)
                                        CreateHorizontalBoxCollider(context, tileSetMeta, startType, y, startX, endX);
                                        startType = -1;
                                        startX = -1;
                                        endX = -1;
                                    }

                                    if (startX == -1)
                                    {
                                        startType = tileType;
                                        startX = x;
                                    }
                                    endX = x;
                                }
                                else if (startX != -1 && endX != -1)
                                {
                                    CreateHorizontalBoxCollider(context, tileSetMeta, startType, y, startX, endX);
                                    startType = -1;
                                    startX = -1;
                                    endX = -1;
                                }
                            }
                            else
                            {
                                if (startX != -1 && endX != -1)
                                {
                                    CreateHorizontalBoxCollider(context, tileSetMeta, startType, y, startX, endX);
                                }

                                startType = -1;
                                startX = -1;
                                endX = -1;
                            }
                        }

                        if (startX != -1 && endX != -1)
                        {
                            CreateHorizontalBoxCollider(context, tileSetMeta, startType, y, startX, endX);
                        }
                    }
                }
                persistence.SaveContext(mapMeta.EntityTemplate, context);
            }
        }

        private static void CreateVerticalBoxCollider(EntityContext context, TileSetMetaData tileSetMeta, int tileType, int x, int startY, int endY)
        {
            var tileDimension = Tile.DIMENSION;
            var centerXOffset = tileSetMeta.GetWallableCenterXOffset(tileType);
            var centerYOffset = tileSetMeta.GetWallableCenterYOffset(tileType);
            var colliderDimension = tileSetMeta.GetWallableThickness(tileType);

            var left = x * tileDimension;
            var tileCenterX = left + (tileDimension / 2);
            var centerLeft = tileCenterX + centerXOffset - (colliderDimension / 2);
            var centerRight = centerLeft + colliderDimension;

            var startTop = ((startY * tileDimension) + (tileDimension / 2)) + centerYOffset - (colliderDimension / 2);
            var endBottom = (((endY * tileDimension) + (tileDimension / 2)) + centerYOffset - (colliderDimension / 2)) + colliderDimension;

            EntityFactory.CreateBoxColliderArea(context, centerLeft, startTop, centerRight, endBottom);
        }

        private static void CreateHorizontalBoxCollider(EntityContext context, TileSetMetaData tileSetMeta, int tileType, int y, int startX, int endX)
        {
            var tileDimension = Tile.DIMENSION;
            var centerXOffset = tileSetMeta.GetWallableCenterXOffset(tileType);
            var centerYOffset = tileSetMeta.GetWallableCenterYOffset(tileType);
            var colliderDimension = tileSetMeta.GetWallableThickness(tileType);

            var top = y * tileDimension;
            var tileCenterY = top + (tileDimension / 2);
            var centerTop = tileCenterY + centerYOffset - (colliderDimension / 2);
            var centerBottom = centerTop + colliderDimension;

            var startLeft = (((startX * tileDimension) + (tileDimension / 2)) + centerXOffset - (colliderDimension / 2));
            var endRight = (((endX * tileDimension) + (tileDimension / 2)) + centerXOffset - (colliderDimension / 2)) + colliderDimension;

            EntityFactory.CreateBoxColliderArea(context, startLeft, centerTop, endRight, centerBottom);
        }
    }
}
