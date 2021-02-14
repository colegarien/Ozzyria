using Ozzyria.Game.Persistence;
using Ozzyria.MapEditor.EventSystem;
using System.Collections.Generic;
using System.IO;

namespace Ozzyria.MapEditor
{
    class MapChangeHandler : IObserver
    {
        public bool CanHandle(IEvent e)
        {
            return e is MapChangeEvent;
        }

        public void Notify(IEvent e)
        {
            var mapChangeEvent = (MapChangeEvent)e;
            if (mapChangeEvent.SaveCurrentlyLoadedMap)
            {
                MapManager.SaveMap();
            }

            if (mapChangeEvent.IsNewMap)
            {
                MapManager.LoadMap(new Map(mapChangeEvent.MapName.Trim(), mapChangeEvent.NewMapTileSet.Trim(), mapChangeEvent.NewMapWidth, mapChangeEvent.NewMapHeight));
            }
            else
            {
                var worldPersistence = new WorldPersistence();

                var tileMap = worldPersistence.LoadMap(mapChangeEvent.MapName.Trim());

                var map = new Map(tileMap.MapName, tileMap.TileSet, tileMap.Width, tileMap.Height);
                foreach (var kv in tileMap.Layers)
                {
                    var layer = kv.Key;
                    var tiles = kv.Value;

                    map.layers[layer] = new Layer(tileMap.Width, tileMap.Height);
                    foreach (var tile in tiles)
                    {
                        // TODO OZ-17 aw crap, this aint gonna scale well (and needs to actually be finsihed up)
                        map.layers[layer].SetTileType(tile.X, tile.Y, 1);
                        map.layers[layer].SetEdgeTransitions(tile.X, tile.Y, new Dictionary<int, EdgeTransitionType>());
                        map.layers[layer].SetCornerTransitions(tile.X, tile.Y, new Dictionary<int, CornerTransitionType>());
                        map.layers[layer].SetPathDirection(tile.X, tile.Y, PathDirection.None);
                    }
                }
                MapManager.LoadMap(map);
            }
        }
    }
}
