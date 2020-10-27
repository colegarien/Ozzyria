using Ozzyria.Game.Component.Attribute;

namespace Ozzyria.Game.Component
{
    [Options(Name = "Combat")]
    public class Combat : Component
    {
        public override ComponentType Type() => ComponentType.Combat;

        [Savable]
        public Delay Delay { get; set; } = new Delay { DelayInSeconds = 0.5f };
        [Savable]
        public bool Attacking { get; set; } = false;
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
