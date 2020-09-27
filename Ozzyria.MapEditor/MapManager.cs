using Ozzyria.MapEditor.EventSystem;

namespace Ozzyria.MapEditor
{
    class MapManager
    {
        protected static Map _map;

        public static bool MapIsLoaded()
        {
            return _map != null;
        }

        public static void LoadMap(Map map)
        {
            _map = map;

            EventQueue.Queue(new MapLoadedEvent
            {
                TileDimension = _map.TileDimension,
                Width = _map.Width,
                Height = _map.Height,
                NumberOfLayers = _map.layers.Count
            });
        }

        public static void PaintTile(int layer, int x, int y, TileType type)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.SetTileType(layer, x, y, type);
        }

        public static void AddLayer()
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.AddLayer();
        }

        public static void RemoveLayer(int layer)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.RemoveLayer(layer);
        }

        public static int GetTileDimension()
        {
            if (!MapIsLoaded())
            {
                return 1;
            }

            return _map.TileDimension;
        }

        public static int GetWidth()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.Width;
        }

        public static int GetHeight()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.Height;
        }
        public static TileType GetTileType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return TileType.None;
            }

            return _map.GetTileType(layer, x, y);
        }

        public static int GetNumberOfLayers()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.layers.Count;
        }
    }
}
