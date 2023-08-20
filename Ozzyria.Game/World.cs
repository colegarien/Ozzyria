using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game
{
    public class World
    {
        public WorldPersistence WorldLoader = new WorldPersistence();
        public Dictionary<int, Input> PlayerInputBuffer = new Dictionary<int, Input>();

        private Dictionary<int, string> _playerAreaTracker = new Dictionary<int, string>();
        private Dictionary<string, Area> _areas = new Dictionary<string, Area>();

        public World()
        {
            _areas["test_a"] = new Area(this, "test_a");
        }

        public void PlayerJoin(int playerId)
        {
            // TODO figure out what would to start player in
            _playerAreaTracker[playerId] = "test_a";
            PlayerInputBuffer[playerId] = new Input();
            EntityFactory.CreatePlayer(_areas[_playerAreaTracker[playerId]]._context, playerId);
        }

        public void PlayerLeave(int playerId)
        {
            var playerEntity = _areas[_playerAreaTracker[playerId]]._context.GetEntities(new EntityQuery().And(typeof(Components.Player))).FirstOrDefault(e => ((Components.Player)e.GetComponent(typeof(Components.Player))).PlayerId == playerId);
            if (playerEntity != null)
            {
                _areas[_playerAreaTracker[playerId]]._context.DestroyEntity(playerEntity);
                _playerAreaTracker.Remove(playerId);
                PlayerInputBuffer.Remove(playerId);
            }
        }

        public EntityContext GetLocalContext(int playerId)
        {
            return _areas[_playerAreaTracker[playerId]]._context;
        }

        public void Update(float deltaTime)
        {
            _areas["test_a"].Update(deltaTime);
        }
    }
}
