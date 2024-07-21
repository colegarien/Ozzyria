﻿using Ozzyria.Content.Models.Area;

namespace Ozzyria.Gryp.Models.Data
{
    internal class Map
    {
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
            Layers.Add(new Layer(new TileBoundary
            {
                TileWidth = Width,
                TileHeight = Height
            }));
        }

        public void PushTile(TileData tileData, int x, int y)
        {
            bool isInSelection = SelectedRegion == null
                || SelectedRegion.TileWidth <= 0
                || SelectedRegion.TileHeight <= 0
                || SelectedRegion.Contains(x, y);

            if (isInSelection && ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                Layers[ActiveLayer].PushTile(tileData, x, y);
            }
        }

        public void PaintArea(TileData tileData, int originX, int originY)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                Layers[ActiveLayer].PaintArea(SelectedRegion, tileData, originX, originY);
            }
        }

        public void AddWall(WorldBoundary wall)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                Layers[ActiveLayer].AddWall(wall);
            }
        }

        public void RemoveWalls(float worldX, float worldY)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                Layers[ActiveLayer].RemoveWalls(worldX, worldY);
            }
        }

        public AreaData ToAreaData()
        {
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
            };
            areaData.TileData.Layers = new string[Layers.Count][][][];
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
            for (var layer = 0; layer < areaData.TileData.Layers.Length; layer++)
            {
                PushLayer();
                ActiveLayer = layer;
                for (var x = 0; x < areaData.TileData.Layers[layer].Length; x++)
                {
                    for (var y = 0; y < areaData.TileData.Layers[layer][x].Length; y++)
                    {
                        PushTile(new Models.Data.TileData
                        {
                            DrawableIds = areaData.TileData.Layers[layer][x][y].ToList(),
                        }, x, y);
                    }
                }
            }
            ActiveLayer = -1;
        }
    }

}
