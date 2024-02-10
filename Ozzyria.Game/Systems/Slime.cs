using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Utility;
using System.Linq;

namespace Ozzyria.Game.Systems
{
    internal class Slime : TickSystem
    {
        const float MAX_FOLLOW_DISTANCE = 200;

        protected EntityQuery query;
        protected EntityQuery playerQuery;
        public Slime()
        {
            query = new EntityQuery();
            query.And(typeof(SlimeThought));

            playerQuery = new EntityQuery().And(typeof(Movement), typeof(Stats), typeof(PlayerThought));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            var players = context.GetEntities(playerQuery);
            foreach (var entity in entities)
            {
                // Death Check
                if (entity.HasComponent(typeof(Stats)) && ((Stats)entity.GetComponent(typeof(Stats))).IsDead())
                {
                    continue;
                }

                var thought = (SlimeThought)entity.GetComponent(typeof(SlimeThought));
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var combat = (Components.Combat)entity.GetComponent(typeof(Components.Combat));

                var closestPlayer = players
                    .OrderBy(p => movement.DistanceTo((Movement)p.GetComponent(typeof(Movement))))
                    .FirstOrDefault();
                if (closestPlayer == null)
                {
                    Think(deltaTime, thought, movement);
                    combat.WantToAttack = false;
                    movement.Update(deltaTime);
                    continue;
                }

                var playerMovement = (Movement)closestPlayer.GetComponent(typeof(Movement));

                var distance = movement.DistanceTo(playerMovement);
                if (distance > MAX_FOLLOW_DISTANCE)
                {
                    Think(deltaTime, thought, movement);
                    combat.WantToAttack = false;
                    movement.Update(deltaTime);
                    continue;
                }

                var attack = false;
                if (distance <= combat.AttackRange)
                {
                    movement.SlowDown(deltaTime);
                    attack = true;
                }
                else
                {
                    movement.SpeedUp(deltaTime);
                }
                movement.TurnToward(deltaTime, playerMovement.X, playerMovement.Y);


                combat.WantToAttack = attack;
                movement.Update(deltaTime);
            }
        }

        private void Think(float deltaTime, SlimeThought thought, Movement movement)
        {
            thought.ThinkDelay.Update(deltaTime);
            if (thought.ThinkDelay.IsReady())
            {
                thought.ThinkDelay.DelayInSeconds = RandomHelper.Random(0f, 1.5f);
                thought.ThinkAction = RandomHelper.Random(0, 5);
            }

            switch (thought.ThinkAction)
            {
                case 0:
                    movement.SlowDown(deltaTime);
                    break;
                case 1:
                    movement.TurnLeft(deltaTime);
                    break;
                case 2:
                    movement.TurnRight(deltaTime);
                    break;
                case 3:
                    movement.SpeedUp(deltaTime);
                    break;
                default:
                    movement.SlowDown(deltaTime);
                    break;
            }
        }
    }
}
