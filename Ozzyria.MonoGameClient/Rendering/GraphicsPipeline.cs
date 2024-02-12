using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ozzyria.MonoGameClient.Rendering
{
    internal class GraphicsPipeline
    {
        #region Singleton Stuff
        public static GraphicsPipeline _instance;

        public static GraphicsPipeline Get()
        {
            if (_instance == null)
                _instance = new GraphicsPipeline();

            return _instance;
        }
        #endregion

        private long nextTileIndex = -1;
        private long tick = 0;
        private Dictionary<long, Graphic> _graphicsPool = new Dictionary<long, Graphic>();
        private Dictionary<uint, long> _entityLastTick = new Dictionary<uint, long>();

        public Graphic GetEntityGraphic(uint entityId)
        {
            if (!_entityLastTick.ContainsKey(entityId) || _entityLastTick[entityId] != tick)
            {
                // Hide the graphics used in previous tick
                long hideIndex = entityId * 1000;
                while (_graphicsPool.ContainsKey(hideIndex) && !_graphicsPool[hideIndex].Hidden)
                {
                    _graphicsPool[hideIndex].Hidden = true;
                    hideIndex++;
                }
                _entityLastTick[entityId] = tick;
            }

            // find first open index
            long index = entityId * 1000;
            while (_graphicsPool.ContainsKey(index) && !_graphicsPool[index].Hidden)
            {
                index++;
            }

            if (!_graphicsPool.ContainsKey(index))
            {
                // instantiate new Graphic if there isn't an available hidden on in the pool
                _graphicsPool[index] = new Graphic();
            }

            // show the graphic and return it
            _graphicsPool[index].Hidden = false;
            return _graphicsPool[index];
        }

        public void ClearEntityGraphics(uint entityId)
        {
            long index = entityId * 1000;
            while (_graphicsPool.ContainsKey(index))
            {
                _graphicsPool.Remove(index);
                index++;
            }
            _entityLastTick.Remove(entityId);
        }

        public Graphic GetTileGraphic()
        {
            if (!_graphicsPool.ContainsKey(nextTileIndex))
            {
                // instantiate new Graphic if there isn't an available hidden on in the pool
                _graphicsPool[nextTileIndex] = new Graphic();
            }

            // show the graphic and return it
            _graphicsPool[nextTileIndex].Hidden = false;
            var graphic = _graphicsPool[nextTileIndex];
            graphic.Hidden = false;
            nextTileIndex--;

            return graphic;
        }

        public void ClearTileGraphics()
        {
            long index = -1;
            while (_graphicsPool.ContainsKey(index))
            {
                _graphicsPool.Remove(index);
                index--;
            }

            nextTileIndex = -1;
        }

        public IEnumerable<Graphic> GetGraphics(Camera camera)
        {
            return _graphicsPool
                .Where(kv => camera.IsInView(kv.Value.Destination.Left, kv.Value.Destination.Top, kv.Value.Destination.Width, kv.Value.Destination.Height))
                .OrderBy(kv => kv.Value.RenderPriority)
                .Select(kv => kv.Value);
        }

        public void SwapBuffer()
        {
            tick++;
        }
    }
}
