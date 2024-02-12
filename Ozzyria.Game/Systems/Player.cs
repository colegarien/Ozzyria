using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Systems
{
    internal class Player : TickSystem
    {
        protected World _world;
        protected EntityQuery query;
        public Player(World world)
        {
            _world = world;
            query = new EntityQuery();
            query.And(typeof(PlayerThought));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                // Death Check
                if (entity.HasComponent(typeof(Stats)) && ((Stats)entity.GetComponent(typeof(Stats))).IsDead())
                {
                    continue;
                }

                var playerId = ((Components.Player)entity.GetComponent(typeof(Components.Player))).PlayerId;
                var input = _world.WorldState.PlayerInputBuffer.ContainsKey(playerId) ? _world.WorldState.PlayerInputBuffer[playerId] : new Input();
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var combat = (Components.Combat)entity.GetComponent(typeof(Components.Combat));

                HandleInput(deltaTime, input, movement);

                combat.WantToAttack = input.Attack;
                movement.Update(deltaTime);
            }
        }

        private void HandleInput(float deltaTime, Input input, Movement movement)
        {
            if (input.TurnLeft)
            {
                movement.TurnLeft(deltaTime);
            }
            if (input.TurnRight)
            {
                movement.TurnRight(deltaTime);
            }

            if (input.MoveUp || input.MoveDown || input.MoveLeft || input.MoveRight)
            {
                movement.SpeedUp(deltaTime);
            }
            if (!input.MoveDown && !input.MoveUp && !input.MoveRight && !input.MoveLeft)
            {
                movement.SlowDown(deltaTime);
            }

            if (input.MoveUp && !input.MoveLeft && !input.MoveRight && !input.MoveDown)
            {
                movement.MoveForward(deltaTime);
            }
            else if (input.MoveDown && !input.MoveLeft && !input.MoveRight && !input.MoveUp)
            {
                movement.MoveBackward(deltaTime);
            }
            else if (!input.MoveUp && !input.MoveDown)
            {
                if (input.MoveRight)
                    movement.MoveRight(deltaTime);
                else if (input.MoveLeft)
                    movement.MoveLeft(deltaTime);
            }
            else if (input.MoveUp && !input.MoveDown)
            {
                if (input.MoveRight)
                    movement.MoveForwardRight(deltaTime);
                else if (input.MoveLeft)
                    movement.MoveForwardLeft(deltaTime);
            }
            else if (input.MoveDown && !input.MoveUp)
            {
                if (input.MoveRight)
                    movement.MoveBackwardRight(deltaTime);
                else if (input.MoveLeft)
                    movement.MoveBackwardLeft(deltaTime);
            }
        }
    }
}
