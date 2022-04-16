using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Utility;
using System.Linq;

namespace Ozzyria.Game.Components
{
    [Options(Name = "SlimeSpawner")]
    class SlimeSpawner : Thought
    {
        private float _x = 500f;
        private float _y = 400f;

        public int SLIME_LIMIT { get; set; } = 3;

        [Savable]
        public float X { get => _x; set
            {
                if (_x != value)
                {
                    _x = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public float Y { get => _y; set
            {
                if (_y != value)
                {
                    _y = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
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
