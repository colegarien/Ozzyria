using Ozzyria.Gryp.Models.Data;
using System.Windows.Forms;

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

    internal class ChangeHistory
    {
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
                    }
                    else if(change is EntitySelectionChange)
                    {
                        var selectionChange = (EntitySelectionChange)change;
                        Redos[Redos.Count - 1].Add(new EntitySelectionChange
                        {
                            InternalId = map.SelectedEntity?.InternalId ?? ""
                        });

                        var selectedEntity = map.GetEntity(selectionChange.InternalId);
                        if (selectedEntity != null) {
                            map.SelectedEntity = selectedEntity;
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
                    else if(change is AddEnityChange)
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
                    else if(change is RemoveEntityChange)
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
                }
                Undos.RemoveAt(Undos.Count - 1);
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
            var dump = "<<REDOS>>\r\n";
            foreach(var redo in Redos)
            {
                dump += "- [";
                foreach(var change in redo)
                {
                    dump += change.GetType().ToString() + ",";
                }
                dump += "]\r\n";
            }

            dump += "\r\n<<UNDOS>>\r\n";
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
