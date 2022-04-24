using Ozzyria.Game.Components;
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

        public Game()
        {
            context = new EntityContext();
            coordinator = new SystemCoordinator();
            // TODO OZ-14 rethink some systems with the new changes listener!
            // TODO OZ-14 chunk entity updates sent back
            // TODO OZ-14 possible move sending/reading entity updates into separate tasks on the client/server
            coordinator
                .Add(new Systems.Player())
                .Add(new Systems.Slime())
                .Add(new Systems.Spawner())
                .Add(new Systems.ExperieneOrb())
                .Add(new Systems.Physics())
                .Add(new Systems.Combat())
                .Add(new Systems.Death(context));

            var worldLoader = new WorldPersistence();
            worldLoader.LoadContext(context, "test_e"); // TODO OZ-14 only load map metadata to get the current "entities" to load and send mapname over to client (i.e. don't load actual tilemap on server)

        }

        public void OnPlayerJoin(int playerId)
        {
            EntityFactory.CreatePlayer(context, playerId);
        }

        public void OnPlayerInput(int playerId, Input input)
        {
            // TODO OZ-14 trashy, don't do this
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

            EntityFactory.CreateExperienceOrb(context, 400, 400, 3);
        }

    }
}
