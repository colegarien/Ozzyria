using Ozzyria.Game.Component;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using System.Linq;

namespace Ozzyria.Game
{
    public class Game
    {
        public EntityContext context;
        public SystemCoordinator coordinator;
        public TileMap tileMap;

        public Game()
        {
            var worldLoader = new WorldPersistence();
            tileMap = worldLoader.LoadMap("test_m");

            context = new EntityContext();
            worldLoader.LoadContext(context, "test_e");

            coordinator = new SystemCoordinator();
            coordinator
                .Add(new Systems.Thought())
                .Add(new Systems.Physics())
                .Add(new Systems.Combat())
                .Add(new Systems.Death(context));
        }

        public void OnPlayerJoin(int playerId)
        {
            EntityFactory.CreatePlayer(context, playerId);
        }

        public void OnPlayerInput(int playerId, Input input)
        {
            // TODO OZ-14 trashy, don't do this
            var playerEntity = context.GetEntities().FirstOrDefault(e => e.HasComponent(typeof(Player)) && ((Player)e.GetComponent(typeof(Player))).PlayerId == playerId);
            if (playerEntity == null)
                return;

            var i = playerEntity.GetComponent(typeof(Input));
            if (i == null)
            {
                i = playerEntity.CreateComponent<Input>();
                playerEntity.AddComponent(i);
            }

            ((Input)i).MoveUp = input.MoveUp;
            ((Input)i).MoveDown = input.MoveDown;
            ((Input)i).MoveLeft = input.MoveLeft;
            ((Input)i).MoveRight = input.MoveRight;
            ((Input)i).TurnLeft = input.TurnLeft;
            ((Input)i).TurnRight = input.TurnRight;
            ((Input)i).Attack = input.Attack;
        }

        public void OnPlayerLeave(int playerId)
        {
            var playerEntity = context.GetEntities().FirstOrDefault(e => e.HasComponent(typeof(Player)) && ((Player)e.GetComponent(typeof(Player))).PlayerId == playerId);
            if(playerEntity != null)
                context.DestroyEntity(playerEntity);
        }

        public void Update(float deltaTime)
        {
            coordinator.Execute(deltaTime, context);
        }

    }
}
