﻿using Ozzyria.Gryp.Models.Data;

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
    }
}
