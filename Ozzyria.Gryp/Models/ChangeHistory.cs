using Ozzyria.Gryp.Models.Data;

namespace Ozzyria.Gryp.Models
{

    internal struct TileChange
    {
        internal int TileX { get; set; }
        internal int TileY { get; set; }
        internal List<string> DrawableIds { get; set; }
    }

    internal class ChangeHistory
    {
        private static bool Tracking = false;
        private static List<List<TileChange>> TileUndos = new List<List<TileChange>>();
        private static List<List<TileChange>> TileRedos = new List<List<TileChange>>();

        public static void StartTracking()
        {
            if (!Tracking)
            {
                Tracking = true;
                TileUndos.Add(new List<TileChange>());
            }
        }

        public static void FinishTracking()
        {
            if (Tracking)
            {
                Tracking = false;
                if(TileUndos[TileUndos.Count - 1].Count <= 0)
                {
                    TileUndos.RemoveAt(TileUndos.Count - 1);
                }
                else
                {
                    // We clear redos after a new change is done
                    TileRedos.Clear();
                }
            }
        }

        public static void TrackChange(TileChange change)
        {
            if (TileUndos.Count <= 0)
                return;

            if (Tracking)
            {
                TileUndos[TileUndos.Count - 1].Add(change);
            }
        }

        public static void Undo(Map map)
        {
            if (TileUndos.Count <= 0)
                return;

            if (!Tracking)
            {
                var changes = TileUndos[TileUndos.Count - 1];

                TileRedos.Add(new List<TileChange>());
                foreach (var change in changes)
                {
                    var tile = map.GetTile(change.TileX, change.TileY);
                    TileRedos[TileRedos.Count - 1].Add(new TileChange
                    {
                        TileX = change.TileX,
                        TileY = change.TileY,
                        DrawableIds = tile?.DrawableIds ?? []
                    });

                    map.PushTile(new Tile
                    {
                        DrawableIds = change.DrawableIds
                    }, change.TileX, change.TileY);
                }
                TileUndos.RemoveAt(TileUndos.Count - 1);
            }
        }

        public static void Redo(Map map)
        {
            if (TileRedos.Count <= 0)
                return;

            if (!Tracking)
            {
                var changes = TileRedos[TileRedos.Count - 1];

                TileUndos.Add(new List<TileChange>());
                foreach (var change in changes)
                {
                    var tile = map.GetTile(change.TileX, change.TileY);
                    TileUndos[TileUndos.Count - 1].Add(new TileChange
                    {
                        TileX = change.TileX,
                        TileY = change.TileY,
                        DrawableIds = tile?.DrawableIds ?? []
                    });

                    map.PushTile(new Tile
                    {
                        DrawableIds = change.DrawableIds
                    }, change.TileX, change.TileY);
                }
                TileRedos.RemoveAt(TileRedos.Count - 1);
            }
        }

        public static void Clear()
        {
            TileUndos.Clear();
            TileRedos.Clear();
            Tracking = false;
        }
    }
}
