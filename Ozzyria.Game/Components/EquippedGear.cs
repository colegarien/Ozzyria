using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class EquippedGear : Component
    {

        private string _body = "";
        private string _hat = ""; // cowboy_hat || green_hat
        private string _armor = ""; // cyan_armor || biker_jacket
        private string _mask = ""; // shades
        private string _weapon = ""; // gladius || pink_sword || grey_sword
        private string _weaponEffect = ""; // basic_slash


        [Savable]
        public string Body
        {
            get => _body; set
            {
                if (_body != value)
                {
                    _body = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public string Hat
        {
            get => _hat; set
            {
                if (_hat != value)
                {
                    _hat = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public string Armor
        {
            get => _armor; set
            {
                if (_armor != value)
                {
                    _armor = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public string Mask
        {
            get => _mask; set
            {
                if (_mask != value)
                {
                    _mask = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public string Weapon
        {
            get => _weapon; set
            {
                if (_weapon != value)
                {
                    _weapon = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public string WeaponEffect
        {
            get => _weaponEffect; set
            {
                if (_weaponEffect != value)
                {
                    _weaponEffect = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
    }
}
