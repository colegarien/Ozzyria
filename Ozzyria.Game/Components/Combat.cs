using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    [Options(Name = "Combat")]
    public class Combat : Component
    {
        private bool _attacking = false;

        [Savable]
        public Delay Delay { get; set; } = new Delay { DelayInSeconds = 0.5f };
        [Savable]
        public bool Attacking { get => _attacking; set
            {
                if (_attacking != value)
                {
                    _attacking = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        public float AttackAngle { get; set; } = 0.78f; // forty-five degrees-ish
        public float AttackRange { get; set; } = 21f;
        public int AttackDamage { get; set; } = 5;

        public void Update(float deltaTime, bool doAttack)
        {
            Delay.Update(deltaTime);

            if (doAttack)
            {
                // Attacking when delay is ready
                Attacking = Delay.IsReady();
            }
            else
            {
                // not currently attacking
                Attacking = false;
            }
        }
    }
}
