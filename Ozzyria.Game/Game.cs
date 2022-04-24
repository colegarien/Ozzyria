using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using System.Linq;

namespace Ozzyria.Game
{
    public class Game // TODO OZ-28 delete game and just do it in Server?
    {
        public EntityContext context;
        public SystemCoordinator coordinator;

        public Game()
        {
            context = new EntityContext();
            coordinator = new SystemCoordinator();
            coordinator
                .Add(new Systems.Player())
                .Add(new Systems.Slime())
                .Add(new Systems.Spawner())
                .Add(new Systems.ExperieneOrb())
                .Add(new Systems.Physics())
                .Add(new Systems.Combat())
                .Add(new Systems.Death(context));

            var worldLoader = new WorldPersistence();
            worldLoader.LoadContext(context, "test_e"); // TODO OZ-27 manages contexts as players change maps

        }

        public void OnPlayerJoin(int playerId)
        {
            EntityFactory.CreatePlayer(context, playerId);
        }

        public void OnPlayerInput(int playerId, Input input)
        {
            var playerEntity = context.GetEntities(new EntityQuery().And(typeof(Player))).FirstOrDefault(e => ((Player)e.GetComponent(typeof(Player))).PlayerId == playerId);
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
            var playerEntity = context.GetEntities(new EntityQuery().And(typeof(Player))).FirstOrDefault(e => ((Player)e.GetComponent(typeof(Player))).PlayerId == playerId);
            if(playerEntity != null)
                context.DestroyEntity(playerEntity);
        }

        public void Update(float deltaTime)
        {
            coordinator.Execute(deltaTime, context);
        }

    }
}
