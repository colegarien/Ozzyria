using Grecs;
using Ozzyria.Game.Components;
using Movement = Ozzyria.Model.Components.Movement;
using MovementIntent = Ozzyria.Model.Components.MovementIntent;
using Ozzyria.Model.Extensions;

namespace Ozzyria.Game.Systems
{
    internal class MovementSystem : TickSystem
    {
        EntityQuery query;
        public MovementSystem(){
            query = new EntityQuery();
            query.And(typeof(MovementIntent), typeof(Movement));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var intent = (MovementIntent)entity.GetComponent(typeof(MovementIntent));

                var isAttacking = entity.HasComponent(typeof(Ozzyria.Model.Components.AttackIntent));
                if (!isAttacking && (intent.MoveUp || intent.MoveDown || intent.MoveLeft || intent.MoveRight))
                {
                    movement.SpeedUp(deltaTime);
                }
                else 
                {
                    movement.SlowDown(deltaTime);
                }

                if (intent.MoveUp && !intent.MoveLeft && !intent.MoveRight && !intent.MoveDown)
                {
                    movement.MoveUp(deltaTime);
                }
                else if (intent.MoveDown && !intent.MoveLeft && !intent.MoveRight && !intent.MoveUp)
                {
                    movement.MoveDown(deltaTime);
                }
                else if (!intent.MoveUp && !intent.MoveDown)
                {
                    if (intent.MoveRight)
                        movement.MoveRight(deltaTime);
                    else if (intent.MoveLeft)
                        movement.MoveLeft(deltaTime);
                }
                else if (intent.MoveUp && !intent.MoveDown)
                {
                    if (intent.MoveRight)
                        movement.MoveUpRight(deltaTime);
                    else if (intent.MoveLeft)
                        movement.MoveUpLeft(deltaTime);
                }
                else if (intent.MoveDown && !intent.MoveUp)
                {
                    if (intent.MoveRight)
                        movement.MoveDownRight(deltaTime);
                    else if (intent.MoveLeft)
                        movement.MoveDownLeft(deltaTime);
                }


                movement.Update(deltaTime);
                entity.RemoveComponent(intent);
            }
        }
    }
}
