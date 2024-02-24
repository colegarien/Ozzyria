using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
    public enum WeaponType
    {
        Empty,
        Sword,
    }

    public class Weapon: Component
    {
        private WeaponType _weaponType;
        private string _weaponId;

        [Savable]
        public WeaponType WeaponType
        {
            get => _weaponType; set
            {
                if (_weaponType != value)
                {
                    _weaponType = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        [Savable]
        public string WeaponId
        {
            get => _weaponId; set
            {
                if (_weaponId != value)
                {
                    _weaponId = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        // TODO actually make these component propertly, load/save from file
        public float AttackAngle { get; set; } = 0.78f; // forty-five degrees-ish
        public float AttackRange { get; set; } = 21f;
        public int AttackDamage { get; set; } = 5;
    }
}
