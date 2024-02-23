using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game
{
    public abstract class AreaEvent{
        public string SourceArea { get; set; }
    }
    public class EntityLeaveAreaEvent : AreaEvent
    {
        public uint EntityId { get; set; }
        public int PlayerId { get; set; } = -1;
        public string NewArea { get; set; } = "";
    }

    public class WorldState
    {
        public Dictionary<int, Input> PlayerInputBuffer = new Dictionary<int, Input>();
        public Dictionary<int, string> PlayerAreaTracker = new Dictionary<int, string>();
        public Dictionary<string, Area> Areas = new Dictionary<string, Area>();
        public List<AreaEvent> AreaEvents = new List<AreaEvent>();
    }

    public class World
    {
        public WorldPersistence WorldLoader = new WorldPersistence();
        public WorldState WorldState = new WorldState();

        public World()
        {
            // load in all area templates
            foreach (var template in WorldLoader.RetrieveAreaTemplates())
            {
                WorldState.Areas[template.Name] = new Area(this, template);
            }
        }

        public void PlayerJoin(int playerId)
        {
            // TODO figure out what world to start player in
            WorldState.PlayerAreaTracker[playerId] = "test_a";
            WorldState.PlayerInputBuffer[playerId] = new Input();
            EntityFactory.CreatePlayer(WorldState.Areas[WorldState.PlayerAreaTracker[playerId]]._context, playerId);
        }

        public void PlayerLeave(int playerId)
        {
            var playerEntity = WorldState.Areas[WorldState.PlayerAreaTracker[playerId]]._context.GetEntities(new EntityQuery().And(typeof(Components.Player))).FirstOrDefault(e => ((Components.Player)e.GetComponent(typeof(Components.Player))).PlayerId == playerId);
            if (playerEntity != null)
            {
                WorldState.Areas[WorldState.PlayerAreaTracker[playerId]]._context.DestroyEntity(playerEntity);
                WorldState.PlayerAreaTracker.Remove(playerId);
                WorldState.PlayerInputBuffer.Remove(playerId);
            }
        }

        public EntityContext GetLocalContext(int playerId)
        {
            return WorldState.Areas[WorldState.PlayerAreaTracker[playerId]]._context;
        }

        public void Update(float deltaTime)
        {
            foreach(var kv in WorldState.Areas)
            {
                kv.Value.Update(deltaTime);
            }
        }
    }
}
