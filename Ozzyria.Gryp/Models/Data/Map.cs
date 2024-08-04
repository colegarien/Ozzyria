using Ozzyria.Content.Models.Area;

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

        public Entity CurrentEntityBrush { get; set; } = new Entity();

        public Entity? SelectedEntity { get; set; } = null;
        public Wall? SelectedWall { get; set; } = null;

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

        public Tile? GetTile(int x, int y)
        {
            bool isInSelection = SelectedRegion == null
                || SelectedRegion.TileWidth <= 0
                || SelectedRegion.TileHeight <= 0
                || SelectedRegion.Contains(x, y);

            if (isInSelection && ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                return Layers[ActiveLayer].GetTileData(x, y);
            }

            return null;
        }

        public Wall? GetWall(string internalId)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                return Layers[ActiveLayer].GetWall(internalId);
            }

            return null;
        }

        public string AddWall(Wall wall)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                var currentId = SelectedWall?.InternalId ?? "";
                SelectedWall = Layers[ActiveLayer].AddWall(wall);
                if (currentId != SelectedWall.InternalId)
                {
                    ChangeHistory.TrackChange(new WallSelectionChange{ InternalId = currentId });
                }

                return SelectedWall.InternalId;
            }

            return "";
        }

        public void UnselectWall()
        {
            if(SelectedWall != null)
            {
                ChangeHistory.TrackChange(new WallSelectionChange
                {
                    InternalId = SelectedWall.InternalId
                });
                SelectedWall = null;
            }
        }

        public void SelectWall(float worldX, float worldY)
        {
            var currentId = SelectedWall?.InternalId ?? "";
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                SelectedWall = Layers[ActiveLayer].SelectWall(worldX, worldY, SelectedWall);
            }
            else
            {
                SelectedWall = null;
            }

            if(currentId != (SelectedWall?.InternalId ?? ""))
            {
                ChangeHistory.TrackChange(new WallSelectionChange
                {
                    InternalId = currentId
                });
            }
        }

        public void RemoveSelectedWall()
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count && SelectedWall != null)
            {
                IsDirty = true;
                var internalId = SelectedWall?.InternalId ?? "";
                UnselectWall();
                Layers[ActiveLayer].RemoveWall(internalId);
            }
        }

        public Entity? GetEntity(string internalId)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                return Layers[ActiveLayer].GetEntity(internalId);
            }

            return null;
        }

        public string AddEntity(Entity entity)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                var currentId = SelectedEntity?.InternalId ?? "";
                SelectedEntity = Layers[ActiveLayer].AddEntity(entity);
                if(currentId != SelectedEntity.InternalId)
                {
                    ChangeHistory.TrackChange(new EntitySelectionChange { InternalId = currentId });
                }
                return SelectedEntity.InternalId;
            }

            return "";
        }

        public void RemoveEntity(string internalId)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                IsDirty = true;
                if (internalId == (SelectedEntity?.InternalId ?? ""))
                    UnselectEntity();
                Layers[ActiveLayer].RemoveEntity(internalId);
            }
        }

        public void UnselectEntity()
        {
            if (SelectedEntity != null)
            {
                ChangeHistory.TrackChange(new EntitySelectionChange
                {
                    InternalId = SelectedEntity.InternalId
                });
                SelectedEntity = null;
            }
        }

        public void SelectEntity(float worldX, float worldY)
        {
            var currentId = SelectedEntity?.InternalId ?? "";
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                SelectedEntity = Layers[ActiveLayer].SelectEntity(worldX, worldY, SelectedEntity);
            }
            else
            {
                SelectedEntity = null;
            }

            if (currentId != (SelectedWall?.InternalId ?? ""))
            {
                ChangeHistory.TrackChange(new EntitySelectionChange
                {
                    InternalId = currentId
                });
            }
        }

        public void RemoveSelectedEntity()
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count && SelectedEntity != null)
            {
                IsDirty = true;
                var internalId = SelectedEntity?.InternalId ?? "";
                UnselectEntity();
                Layers[ActiveLayer].RemoveEntity(internalId);
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
                    X = b.Boundary.WorldX,
                    Y = b.Boundary.WorldY,
                    Width = b.Boundary.WorldWidth,
                    Height = b.Boundary.WorldHeight,
                }).ToArray();

                areaData.PrefabData.Prefabs[layer] = Layers[layer].GetEntities().Select(e => new Content.Models.Area.PrefabEntry
                {
                    PrefabId = e.PrefabId,
                    X = e.WorldX,
                    Y = e.WorldY,
                    Attributes = e.Attributes,
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
                    AddWall(new Wall {
                        Boundary = new WorldBoundary
                        {
                            WorldX = wall.X,
                            WorldY = wall.Y,
                            WorldWidth = wall.Width,
                            WorldHeight = wall.Height,
                        }
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
                        Attributes = prefab.Attributes,
                    });
                }
            }
            SelectedEntity = null;
            SelectedWall = null;
            ActiveLayer = -1;
            IsDirty = false;
        }
    }

}
