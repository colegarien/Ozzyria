﻿using Ozzyria.Content.Models.Area;

namespace Ozzyria.Gryp.Models.Data
{
    internal class Map
    {
        public bool IsDirty { get; set; } = false;
        public AreaMetaData MetaData { get; set; } = new AreaMetaData();
        public int Width { get; set; }
        public int Height { get; set; }

        public List<Layer> Layers { get; set; } = new List<Layer>();

        public int ActiveLayer { get; set; } = -1;
        public Dictionary<int, bool> IsLayerHidden { get; set; } = new Dictionary<int, bool>();

        public TileBoundary? SelectedRegion { get; set; } = null;

        public List<string> CurrentBrush { get; set; } = new List<string>();

        public bool IsLayerVisible(int layer)
        {
            return !IsLayerHidden.ContainsKey(layer) || !IsLayerHidden[layer];
        }

        public void PushLayer()
        {
            IsDirty = true;
            Layers.Add(new Layer(new TileBoundary
            {
                TileWidth = Width,
                TileHeight = Height
            }));
        }

        public void PushTile(Tile tileData, int x, int y)
        {
            bool isInSelection = SelectedRegion == null
                || SelectedRegion.TileWidth <= 0
                || SelectedRegion.TileHeight <= 0
                || SelectedRegion.Contains(x, y);

            if (isInSelection && ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                Layers[ActiveLayer].PushTile(tileData, x, y);
            }
        }

        public void PaintArea(Tile tileData, int originX, int originY)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                Layers[ActiveLayer].PaintArea(SelectedRegion, tileData, originX, originY);
            }
        }

        public void AddWall(WorldBoundary wall)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                Layers[ActiveLayer].AddWall(wall);
            }
        }

        public void RemoveWalls(float worldX, float worldY)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                Layers[ActiveLayer].RemoveWalls(worldX, worldY);
            }
        }

        public void AddEntity(Entity entity)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                Layers[ActiveLayer].AddEntity(entity);
            }
        }

        public void RemoveEntities(float worldX, float worldY)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                Layers[ActiveLayer].RemoveEntities(worldX, worldY);
            }
        }

        public AreaData ToAreaData()
        {
            IsDirty = false;
            var areaData = new AreaData();
            areaData.AreaMetaData = new AreaMetaData
            {
                AreaId = MetaData.AreaId,
                DisplayName = MetaData.DisplayName,
                CreatedAt = MetaData.CreatedAt,
                UpdatedAt = DateTime.Now,
            };

            areaData.TileData = new Content.Models.Area.TileData
            {
                Width = Width,
                Height = Height,
                Layers = new string[Layers.Count][][][]
            };
            areaData.WallData = new WallData
            {
                Walls = new Content.Models.Area.Rectangle[Layers.Count][]
            };
            areaData.PrefabData = new PrefabData
            {
                Prefabs = new Content.Models.Area.PrefabEntry[Layers.Count][]
            };
            for (var layer = 0; layer < Layers.Count; layer++)
            {
                areaData.TileData.Layers[layer] = new string[Width][][];
                for (var x = 0; x < Width; x++)
                {
                    areaData.TileData.Layers[layer][x] = new string[Height][];
                    for (var y = 0; y < Height; y++)
                    {
                        areaData.TileData.Layers[layer][x][y] = Layers[layer]?.GetTileData(x, y)?.DrawableIds?.ToArray() ?? [];
                    }
                }

                areaData.WallData.Walls[layer] = Layers[layer].GetWalls().Select(b => new Content.Models.Area.Rectangle
                {
                    X = b.WorldX,
                    Y = b.WorldY,
                    Width = b.WorldWidth,
                    Height = b.WorldHeight,
                }).ToArray();

                areaData.PrefabData.Prefabs[layer] = Layers[layer].GetEntities().Select(e => new Content.Models.Area.PrefabEntry
                {
                    PrefabId = e.PrefabId,
                    X = e.WorldX,
                    Y = e.WorldY,
                }).ToArray();
            }

            return areaData;
        }

        public void FromAreaData(AreaData areaData)
        {
            SelectedRegion = null;

            MetaData.AreaId = areaData.AreaMetaData?.AreaId ?? "";
            MetaData.DisplayName = areaData.AreaMetaData?.DisplayName ?? "";
            MetaData.CreatedAt = areaData.AreaMetaData?.CreatedAt ?? DateTime.Now;
            MetaData.UpdatedAt = areaData.AreaMetaData?.UpdatedAt ?? DateTime.Now;

            Width = areaData.TileData.Width;
            Height = areaData.TileData.Height;

            Layers.Clear();
            for (var layer = 0; layer < (areaData.TileData?.Layers?.Length ?? 0); layer++)
            {
                PushLayer();
                ActiveLayer = layer;
                for (var x = 0; x < (areaData.TileData?.Layers[layer]?.Length ?? 0); x++)
                {
                    for (var y = 0; y < (areaData.TileData?.Layers[layer][x]?.Length ?? 0); y++)
                    {
                        PushTile(new Models.Data.Tile
                        {
                            DrawableIds = areaData.TileData?.Layers[layer][x][y]?.ToList() ?? [],
                        }, x, y);
                    }
                }
            }
            for (var layer = 0; layer < (areaData.WallData?.Walls?.Length ?? 0); layer++)
            {
                ActiveLayer = layer;
                foreach (var wall in areaData.WallData?.Walls[layer] ?? []) {
                    AddWall(new WorldBoundary
                    {
                        WorldX = wall.X,
                        WorldY = wall.Y,
                        WorldWidth = wall.Width,
                        WorldHeight = wall.Height,
                    });
                }
            }
            for (var layer = 0; layer < (areaData.PrefabData?.Prefabs?.Length ?? 0); layer++)
            {
                ActiveLayer = layer;
                foreach (var prefab in areaData.PrefabData?.Prefabs[layer] ?? [])
                {
                    AddEntity(new Entity
                    {
                        PrefabId = prefab.PrefabId,
                        WorldX = prefab.X,
                        WorldY = prefab.Y,
                    });
                }
            }
            ActiveLayer = -1;
            IsDirty = false;
        }
    }

}
