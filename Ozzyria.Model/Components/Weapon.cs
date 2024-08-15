using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Weapon : Grecs.Component, ISerializable, IHydrateable
    {
        private WeaponType _weaponType;
        public WeaponType WeaponType
        {
            get => _weaponType; set
            {
                if (!_weaponType.Equals(value))
                {
                    _weaponType = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private string _weaponId = "";
        public string WeaponId
        {
            get => _weaponId; set
            {
                if (!_weaponId?.Equals(value) ?? (value != null))
                {
                    _weaponId = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _attackAngle = 0.78f;
        public float AttackAngle
        {
            get => _attackAngle; set
            {
                if (!_attackAngle.Equals(value))
                {
                    _attackAngle = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _attackRange = 21f;
        public float AttackRange
        {
            get => _attackRange; set
            {
                if (!_attackRange.Equals(value))
                {
                    _attackRange = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _attackDamage = 5;
        public int AttackDamage
        {
            get => _attackDamage; set
            {
                if (!_attackDamage.Equals(value))
                {
                    _attackDamage = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "weapon";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((int)WeaponType);
            w.Write(WeaponId);
            w.Write(AttackAngle);
            w.Write(AttackRange);
            w.Write(AttackDamage);
        }

        public void Read(System.IO.BinaryReader r)
        {
            WeaponType = (WeaponType)r.ReadInt32();
            WeaponId = r.ReadString();
            AttackAngle = r.ReadSingle();
            AttackRange = r.ReadSingle();
            AttackDamage = r.ReadInt32();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("weaponType"))
            {
                WeaponType = (WeaponType)Enum.Parse(typeof(WeaponType), values["weaponType"], true);
            }
            if (values.HasValueFor("weaponId"))
            {
                WeaponId = values["weaponId"].Trim('"');
            }
            if (values.HasValueFor("attackAngle"))
            {
                AttackAngle = float.Parse(values["attackAngle"].Trim('f'));
            }
            if (values.HasValueFor("attackRange"))
            {
                AttackRange = float.Parse(values["attackRange"].Trim('f'));
            }
            if (values.HasValueFor("attackDamage"))
            {
                AttackDamage = int.Parse(values["attackDamage"]);
            }
        }
    }
}
