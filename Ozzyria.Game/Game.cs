using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game
{
    public class Game // TODO OZ-28 delete game and just do it in Server?
    {
        public Dictionary<int, Input> playerInputBuffer = new Dictionary<int, Input>();
        public EntityContext context;
        public SystemCoordinator coordinator;

        public Game()
        {
            context = new EntityContext();
            coordinator = new SystemCoordinator();
            coordinator
                .Add(new Systems.Player(this))
                .Add(new Systems.Slime())
                .Add(new Systems.Spawner())
                .Add(new Systems.ExperieneOrb())
                .Add(new Systems.Physics())
                .Add(new Systems.Combat())
                .Add(new Systems.Death(context))
                .Add(new Systems.AnimationStateSync(context))
                .Add(new Systems.Animation());

            var worldLoader = new WorldPersistence();
            worldLoader.LoadContext(context, "test_a_template"); // TODO OZ-27 manages contexts as players change maps

        }

        public void OnPlayerJoin(int playerId)
        {
            EntityFactory.CreatePlayer(context, playerId);
            playerInputBuffer[playerId] = new Input();
        }

        public void OnPlayerInput(int playerId, Input input)
        {
            playerInputBuffer[playerId].MoveUp = input.MoveUp;
            playerInputBuffer[playerId].MoveDown = input.MoveDown;
            playerInputBuffer[playerId].MoveLeft = input.MoveLeft;
            playerInputBuffer[playerId].MoveRight = input.MoveRight;
            playerInputBuffer[playerId].TurnLeft = input.TurnLeft;
            playerInputBuffer[playerId].TurnRight = input.TurnRight;
            playerInputBuffer[playerId].Attack = input.Attack;
        }

        public void OnPlayerLeave(int playerId)
        {
            var playerEntity = context.GetEntities(new EntityQuery().And(typeof(Player))).FirstOrDefault(e => ((Player)e.GetComponent(typeof(Player))).PlayerId == playerId);
            if (playerEntity != null)
            {
                context.DestroyEntity(playerEntity);
                playerInputBuffer.Remove(playerId);
            }
        }

        public void Update(float deltaTime)
        {
            coordinator.Execute(deltaTime, context);
        }

    }
}
