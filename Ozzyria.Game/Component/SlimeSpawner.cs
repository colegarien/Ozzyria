using Ozzyria.Game.Component.Attribute;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Utility;
using System.Linq;

namespace Ozzyria.Game.Component
{
    [Options(Name = "SlimeSpawner")]
    class SlimeSpawner : Thought
    {
        public int SLIME_LIMIT { get; set; } = 3;
        [Savable]
        public float X { get; set; } = 500f;
        [Savable]
        public float Y { get; set; } = 400f;
        [Savable]
        public Delay ThinkDelay { get; set; } = new Delay
        {
            DelayInSeconds = 5f
        };

        public override void Update(float deltaTime, EntityContext context)
        {
            ThinkDelay.Update(deltaTime);

            var numberOfSlimes = context.GetEntities(new EntityQuery().And(typeof(SlimeThought))).Count();
            if (numberOfSlimes < SLIME_LIMIT && ThinkDelay.IsReady())
            {
                // OZ-22 : check if spawner is block before spawning things
                EntityFactory.CreateSlime(context, X, Y);
            }
        }
    }
}
