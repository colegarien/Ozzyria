using Ozzyria.Model.Components;
using Grecs;
using Ozzyria.Model.Extensions;

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
                if (entity.HasComponent(typeof(Stats)) && entity.GetComponent<Stats>().IsDead())
                {
                    continue;
                }

                var playerId = entity.GetComponent<Model.Components.Player>().PlayerId;
                var input = _world.WorldState.PlayerInputBuffer.ContainsKey(playerId) ? _world.WorldState.PlayerInputBuffer[playerId] : new Input();
                var movement = entity.GetComponent<Movement>();

                if (input.MoveUp || input.MoveDown || input.MoveLeft || input.MoveRight || movement.IsMoving())
                {
                    // if player is trying to move OR they are trying not too move but are moving
                    var intent = MovementIntent.GetInstance();
                    intent.MoveLeft = input.MoveLeft;
                    intent.MoveRight = input.MoveRight;
                    intent.MoveUp = input.MoveUp;
                    intent.MoveDown = input.MoveDown;
                    entity.AddComponent(intent);
                }

                if (input.Attack && !entity.HasComponent(typeof(AttackIntent)))
                {
                    var intent = AttackIntent.GetInstance();
                    intent.Frame = 0;
                    intent.DecayFrame = 3;
                    intent.DamageFrame = 1;
                    intent.FrameTimer = 0f;
                    entity.AddComponent(intent);
                }
            }
        }

    }
}
