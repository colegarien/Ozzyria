using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Event;

namespace Ozzyria.Gryp.Models
{

    internal struct TileChange
    {
        internal int TileX { get; set; }
        internal int TileY { get; set; }
        internal List<string> DrawableIds { get; set; }
    }

    internal struct LayerChange
    {
        internal int Layer { get; set; }
    }

    internal struct EntitySelectionChange
    {
        internal string InternalId { get; set; }
    }

    internal struct WallSelectionChange
    {
        internal string InternalId { get; set; }
    }

    internal struct AddEnityChange
    {
        internal string InternalId { get; set; }
    }

    internal struct RemoveEntityChange
    {
        internal Entity Entity { get; set; }
    }

    internal struct AddWallChange
    {
        internal string InternalId { get; set; }
    }

    internal struct RemoveWallChange
    {
        internal Wall Wall { get; set; }
    }

    internal struct EditEntityChange
    {
        internal string InternalId { get; set; }
        public string PrefabId { get; set; }
        public float WorldX { get; set; }
        public float WorldY { get; set; }
        public Dictionary<string, string> Attributes;
    }

    internal struct EditWallChange
    {
        internal string InternalId { get; set; }
        public WorldBoundary Boundary { get; set; }
    }

    internal class ChangeHistory
    {
        private const int HISTORY_LIMIT = 69;

        private static bool Tracking = false;
        private static List<List<object>> Undos = new List<List<object>>();
        private static List<List<object>> Redos = new List<List<object>>();

        public static void StartTracking()
        {
            if (!Tracking)
            {
                Tracking = true;
                Undos.Add(new List<object>());
            }
        }

        public static void FinishTracking()
        {
            if (Tracking)
            {
                Tracking = false;
                if(Undos[Undos.Count - 1].Count <= 0)
                {
                    Undos.RemoveAt(Undos.Count - 1);
                }
                else
                {
                    // We clear redos after a new change is done
                    Redos.Clear();
                    while(Undos.Count > HISTORY_LIMIT)
                        Undos.RemoveAt(0);
                }
            }
        }

        public static void TrackChange(object change)
        {
            if (Undos.Count <= 0)
                return;

            if (Tracking)
            {
                Undos[Undos.Count - 1].Add(change);
            }
        }

        public static void Undo(Map map)
        {
            if (Undos.Count <= 0)
                return;

            if (!Tracking)
            {
                var changes = Undos[Undos.Count - 1];

                Redos.Add(new List<object>());
                foreach (var change in changes)
                {
                    if (change is TileChange)
                    {
                        var tileChange = (TileChange)change;

                        var tile = map.GetTile(tileChange.TileX, tileChange.TileY);
                        Redos[Redos.Count - 1].Add(new TileChange
                        {
                            TileX = tileChange.TileX,
                            TileY = tileChange.TileY,
                            DrawableIds = tile?.DrawableIds ?? []
                        });

                        map.PushTile(new Tile
                        {
                            DrawableIds = tileChange.DrawableIds
                        }, tileChange.TileX, tileChange.TileY);
                    }
                    else if (change is LayerChange)
                    {
                        var layerChange = (LayerChange)change;
                        Redos[Redos.Count - 1].Add(new LayerChange
                        {
                            Layer = map.ActiveLayer
                        });

                        map.ActiveLayer = layerChange.Layer;
                        EventBus.Notify(new ActiveLayerChangedEvent { });
                    }
                    else if (change is EntitySelectionChange)
                    {
                        var selectionChange = (EntitySelectionChange)change;
                        Redos[Redos.Count - 1].Add(new EntitySelectionChange
                        {
                            InternalId = map.SelectedEntity?.InternalId ?? ""
                        });

                        var selectedEntity = map.GetEntity(selectionChange.InternalId);
                        if (selectedEntity != null)
                        {
                            // TODO might need to make unselect separate?
                            map.SelectedEntity = selectedEntity;
                            EventBus.Notify(new SelectedEntityChangeEvent { });
                        }
                    }
                    else if (change is WallSelectionChange)
                    {
                        var selectionChange = (WallSelectionChange)change;
                        Redos[Redos.Count - 1].Add(new WallSelectionChange
                        {
                            InternalId = map.SelectedWall?.InternalId ?? ""
                        });

                        map.SelectedWall = map.GetWall(selectionChange.InternalId);
                    }
                    else if (change is AddEnityChange)
                    {
                        var entityChange = (AddEnityChange)change;
                        var entity = map.GetEntity(entityChange.InternalId);
                        if (entity != null)
                        {
                            Redos[Redos.Count - 1].Add(new RemoveEntityChange
                            {
                                Entity = entity
                            });

                            map.RemoveEntity(entityChange.InternalId);
                        }
                    }
                    else if (change is RemoveEntityChange)
                    {
                        var entityChange = (RemoveEntityChange)change;
                        if (entityChange.Entity != null)
                        {
                            Redos[Redos.Count - 1].Add(new AddEnityChange
                            {
                                InternalId = entityChange.Entity.InternalId
                            });

                            map.AddEntity(entityChange.Entity);
                        }
                    }
                    else if (change is AddWallChange)
                    {
                        var wallChange = (AddWallChange)change;
                        var wall = map.GetWall(wallChange.InternalId);
                        if (wall != null)
                        {
                            Redos[Redos.Count - 1].Add(new RemoveWallChange
                            {
                                Wall = wall
                            });

                            map.RemoveWall(wallChange.InternalId);
                        }
                    }
                    else if (change is RemoveWallChange)
                    {
                        var wallChange = (RemoveWallChange)change;
                        if (wallChange.Wall != null)
                        {
                            Redos[Redos.Count - 1].Add(new AddWallChange
                            {
                                InternalId = wallChange.Wall.InternalId
                            });

                            map.AddWall(wallChange.Wall);
                        }
                    }
                    else if (change is EditEntityChange)
                    {
                        var entityChange = (EditEntityChange)change;
                        var currentEntity = map.GetEntity(entityChange.InternalId);
                        if (currentEntity != null)
                        {
                            Redos[Redos.Count - 1].Add(new EditEntityChange
                            {
                                InternalId = currentEntity.InternalId,
                                PrefabId = currentEntity.PrefabId,
                                WorldX = currentEntity.WorldX,
                                WorldY = currentEntity.WorldY,
                                Attributes = currentEntity.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>(),
                            });

                            currentEntity.PrefabId = entityChange.PrefabId;
                            currentEntity.WorldX = entityChange.WorldX;
                            currentEntity.WorldY = entityChange.WorldY;
                            currentEntity.Attributes = entityChange.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>();
                        }
                    }
                    else if (change is EditWallChange)
                    {
                        var wallChange = (EditWallChange)change;
                        var currentWall = map.GetWall(wallChange.InternalId);
                        if (currentWall != null)
                        {
                            Redos[Redos.Count - 1].Add(new EditWallChange
                            {
                                InternalId = currentWall.InternalId,
                                Boundary = new WorldBoundary
                                {
                                    WorldX = currentWall.Boundary.WorldX,
                                    WorldY = currentWall.Boundary.WorldY,
                                    WorldWidth = currentWall.Boundary.WorldWidth,
                                    WorldHeight = currentWall.Boundary.WorldHeight
                                }
                            });

                            currentWall.Boundary.WorldX = wallChange.Boundary.WorldX;
                            currentWall.Boundary.WorldY = wallChange.Boundary.WorldY;
                            currentWall.Boundary.WorldWidth = wallChange.Boundary.WorldWidth;
                            currentWall.Boundary.WorldHeight = wallChange.Boundary.WorldHeight;
                        }
                    }
                }
                Undos.RemoveAt(Undos.Count - 1);
                
                // clean up redos
                if (Redos[Redos.Count - 1].Count <= 0)
                {
                    Redos.RemoveAt(Undos.Count - 1);
                }
                else
                {
                    while (Redos.Count > HISTORY_LIMIT)
                        Redos.RemoveAt(0);
                }
            }
        }

        public static void Redo(Map map)
        {
            if (Redos.Count <= 0)
                return;

            if (!Tracking)
            {
                var changes = Redos[Redos.Count - 1];
                Undos.Add(new List<object>());
                foreach (var change in changes)
                {
                    if (change is TileChange)
                    {
                        var tileChange = (TileChange)change;
                        var tile = map.GetTile(tileChange.TileX, tileChange.TileY);
                        Undos[Undos.Count - 1].Add(new TileChange
                        {
                            TileX = tileChange.TileX,
                            TileY = tileChange.TileY,
                            DrawableIds = tile?.DrawableIds ?? []
                        });

                        map.PushTile(new Tile
                        {
                            DrawableIds = tileChange.DrawableIds
                        }, tileChange.TileX, tileChange.TileY);
                    }
                    else if (change is LayerChange)
                    {
                        var layerChange = (LayerChange)change;
                        Undos[Undos.Count - 1].Add(new LayerChange
                        {
                            Layer = map.ActiveLayer,
                        });

                        map.ActiveLayer = layerChange.Layer;
                        EventBus.Notify(new ActiveLayerChangedEvent { });
                    }
                    else if (change is EntitySelectionChange)
                    {
                        var selectionChange = (EntitySelectionChange)change;
                        Undos[Undos.Count - 1].Add(new EntitySelectionChange
                        {
                            InternalId = map.SelectedEntity?.InternalId ?? ""
                        });

                        var selectedEntity = map.GetEntity(selectionChange.InternalId);
                        if (selectedEntity != null)
                        {
                            map.SelectedEntity = selectedEntity;
                            EventBus.Notify(new SelectedEntityChangeEvent { });
                        }
                    }
                    else if (change is WallSelectionChange)
                    {
                        var selectionChange = (WallSelectionChange)change;
                        Undos[Undos.Count - 1].Add(new WallSelectionChange
                        {
                            InternalId = map.SelectedWall?.InternalId ?? ""
                        });

                        map.SelectedWall = map.GetWall(selectionChange.InternalId);
                    }
                    else if (change is AddEnityChange)
                    {
                        var entityChange = (AddEnityChange)change;
                        var entity = map.GetEntity(entityChange.InternalId);
                        if (entity != null) {
                            Undos[Undos.Count - 1].Add(new RemoveEntityChange
                            {
                                Entity = entity
                            });

                            map.RemoveEntity(entityChange.InternalId);
                        }
                    }
                    else if (change is RemoveEntityChange)
                    {
                        var entityChange = (RemoveEntityChange)change;
                        if (entityChange.Entity != null)
                        {
                            Undos[Undos.Count - 1].Add(new AddEnityChange
                            {
                                InternalId = entityChange.Entity.InternalId
                            });

                            map.AddEntity(entityChange.Entity);
                        }
                    }
                    else if (change is AddWallChange)
                    {
                        var wallChange = (AddWallChange)change;
                        var wall = map.GetWall(wallChange.InternalId);
                        if (wall != null)
                        {
                            Undos[Undos.Count - 1].Add(new RemoveWallChange
                            {
                                Wall = wall
                            });

                            map.RemoveWall(wallChange.InternalId);
                        }
                    }
                    else if (change is RemoveWallChange)
                    {
                        var wallChange = (RemoveWallChange)change;
                        if (wallChange.Wall != null)
                        {
                            Undos[Undos.Count - 1].Add(new AddWallChange
                            {
                                InternalId = wallChange.Wall.InternalId
                            });

                            map.AddWall(wallChange.Wall);
                        }
                    }
                    else if (change is EditEntityChange)
                    {
                        var entityChange = (EditEntityChange)change;
                        var currentEntity = map.GetEntity(entityChange.InternalId);
                        if (currentEntity != null)
                        {
                            Undos[Undos.Count - 1].Add(new EditEntityChange
                            {
                                InternalId = currentEntity.InternalId,
                                PrefabId = currentEntity.PrefabId,
                                WorldX = currentEntity.WorldX,
                                WorldY = currentEntity.WorldY,
                                Attributes = currentEntity.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>(),
                            });

                            currentEntity.PrefabId = entityChange.PrefabId;
                            currentEntity.WorldX = entityChange.WorldX;
                            currentEntity.WorldY = entityChange.WorldY;
                            currentEntity.Attributes = entityChange.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>();
                        }
                    }
                    else if (change is EditWallChange)
                    {
                        var wallChange = (EditWallChange)change;
                        var currentWall = map.GetWall(wallChange.InternalId);
                        if (currentWall != null)
                        {
                            Undos[Undos.Count - 1].Add(new EditWallChange
                            {
                                InternalId = currentWall.InternalId,
                                Boundary = new WorldBoundary
                                {
                                    WorldX = currentWall.Boundary.WorldX,
                                    WorldY = currentWall.Boundary.WorldY,
                                    WorldWidth = currentWall.Boundary.WorldWidth,
                                    WorldHeight = currentWall.Boundary.WorldHeight
                                }
                            });

                            currentWall.Boundary.WorldX = wallChange.Boundary.WorldX;
                            currentWall.Boundary.WorldY = wallChange.Boundary.WorldY;
                            currentWall.Boundary.WorldWidth = wallChange.Boundary.WorldWidth;
                            currentWall.Boundary.WorldHeight = wallChange.Boundary.WorldHeight;
                        }
                    }
                }
                Redos.RemoveAt(Redos.Count - 1);
            }
        }

        public static void Clear()
        {
            Undos.Clear();
            Redos.Clear();
            Tracking = false;
        }

        public static string DebugDump()
        {
            var dump = "<<REDOS ("+Redos.Count+")>>\r\n";
            foreach(var redo in Redos)
            {
                dump += "- [";
                foreach(var change in redo)
                {
                    dump += change.GetType().ToString() + ",";
                }
                dump += "]\r\n";
            }

            dump += "\r\n<<UNDOS ("+Undos.Count+")>>\r\n";
            foreach (var undo in Undos)
            {
                dump += "- [";
                foreach (var change in undo)
                {
                    dump += change.GetType().ToString() + ",";
                }
                dump += "]\r\n";
            }

            return dump;
        }
    }
}
