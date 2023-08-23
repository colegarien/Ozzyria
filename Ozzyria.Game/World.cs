using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game
{
    public class WorldState
    {
        public Dictionary<int, Input> PlayerInputBuffer = new Dictionary<int, Input>();
        public Dictionary<int, string> PlayerAreaTracker = new Dictionary<int, string>();
        public Dictionary<string, Area> Areas = new Dictionary<string, Area>();
    }

    public class World
    {
        public WorldPersistence WorldLoader = new WorldPersistence();
        public WorldState WorldState = new WorldState();

        public World()
        {
            WorldState.Areas["test_a"] = new Area(this, "test_a");
            WorldState.Areas["test_b"] = new Area(this, "test_b");
        }

        public void PlayerJoin(int playerId)
        {
            // TODO figure out what would to start player in
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

            if(WorldState.PlayerInputBuffer.ContainsKey(0) && WorldState.PlayerInputBuffer[0].Attack && WorldState.PlayerAreaTracker[0] == "test_a")
            {
                foreach(var entity in WorldState.Areas["test_a"]._context.GetEntities())
                {
                    if (entity.HasComponent(typeof(Components.Player)))
                    {
                        var areaChange = (AreaChange)entity.CreateComponent(typeof(AreaChange));
                        areaChange.NewArea = "test_b";
                        areaChange.NewX = 140;
                        areaChange.NewY = 140;
                        entity.AddComponent(areaChange);
                        break;
                    }
                }
            }
        }
    }
}
