using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
    public enum SkeletonPose
    {
        Idle,
        Walk,
        Attack,
    }
    public enum SkeletonType
    {
        Humanoid,
        Slime,
        Static,
    }

    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    public class Skeleton: Component
    {
        private SkeletonType _type { get; set; }

        private SkeletonPose _pose;
        private int _frame { get; set; }

        private int _layer { get; set; }
        private int _subLayer { get; set; }

        private Direction _direction { get; set; }

        private int _rootX { get; set; }
        private int _rootY { get; set; }

        private int _renderOffsetY { get; set; }

        private int _weaponOffsetX { get; set; }
        private int _weaponOffsetY { get; set; }
        private float _weaponOffsetAngle { get; set; }

        private int _armorOffsetX { get; set; }
        private int _armorOffsetY { get; set; }
        private float _armorOffsetAngle { get; set; }

        private int _maskOffsetX { get; set; }
        private int _maskOffsetY { get; set; }
        private float _maskOffsetAngle { get; set; }

        private int _hatOffsetX { get; set; }
        private int _hatOffsetY { get; set; }
        private float _hatOffsetAngle { get; set; }

        [Savable]
        public SkeletonType Type
        {
            get => _type; set
            {
                if (_type != value)
                {
                    _type = value;
                    TriggerChange();
                }
            }
        }

        [Savable]
        public SkeletonPose Pose
        {
            get => _pose; set
            {
                if (_pose != value)
                {
                    _pose = value;
                    TriggerChange();
                }
            }
        }

        public int Frame
        {
            get => _frame; set
            {
                if (_frame != value)
                {
                    _frame = value;
                    TriggerChange();
                }
            }
        }

        public int Layer
        {
            get => _layer; set
            {
                if (_layer != value)
                {
                    _layer = value;
                    TriggerChange();
                }
            }
        }

        public int SubLayer
        {
            get => _subLayer; set
            {
                if (_subLayer != value)
                {
                    _subLayer = value;
                    TriggerChange();
                }
            }
        }
        
        public Direction Direction
        {
            get => _direction; set
            {
                if (_direction != value)
                {
                    _direction = value;
                    TriggerChange();
                }
            }
        }

        public int RootX
        {
            get => _rootX; set
            {
                if (_rootX != value)
                {
                    _rootX = value;
                    TriggerChange();
                }
            }
        }

        public int RootY
        {
            get => _rootY; set
            {
                if (_rootY != value)
                {
                    _rootY = value;
                    TriggerChange();
                }
            }
        }

        public int RenderOffsetY
        {
            get => _renderOffsetY; set
            {
                if (_renderOffsetY != value)
                {
                    _renderOffsetY = value;
                    TriggerChange();
                }
            }
        }

        public int WeaponOffsetX
        {
            get => _weaponOffsetX; set
            {
                if (_weaponOffsetX != value)
                {
                    _weaponOffsetX = value;
                    TriggerChange();
                }
            }
        }

        public int WeaponOffsetY
        {
            get => _weaponOffsetY; set
            {
                if (_weaponOffsetY != value)
                {
                    _weaponOffsetY = value;
                    TriggerChange();
                }
            }
        }

        public float WeaponOffsetAngle
        {
            get => _weaponOffsetAngle; set
            {
                if (_weaponOffsetAngle != value)
                {
                    _weaponOffsetAngle = value;
                    TriggerChange();
                }
            }
        }

        public int ArmorOffsetX
        {
            get => _armorOffsetX; set
            {
                if (_armorOffsetX != value)
                {
                    _armorOffsetX = value;
                    TriggerChange();
                }
            }
        }

        public int ArmorOffsetY
        {
            get => _armorOffsetY; set
            {
                if (_armorOffsetY != value)
                {
                    _armorOffsetY = value;
                    TriggerChange();
                }
            }
        }

        public float ArmorOffsetAngle
        {
            get => _armorOffsetAngle; set
            {
                if (_armorOffsetAngle != value)
                {
                    _armorOffsetAngle = value;
                    TriggerChange();
                }
            }
        }

        public int MaskOffsetX
        {
            get => _maskOffsetX; set
            {
                if (_maskOffsetX != value)
                {
                    _maskOffsetX = value;
                    TriggerChange();
                }
            }
        }

        public int MaskOffsetY
        {
            get => _maskOffsetY; set
            {
                if (_maskOffsetY != value)
                {
                    _maskOffsetY = value;
                    TriggerChange();
                }
            }
        }

        public float MaskOffsetAngle
        {
            get => _maskOffsetAngle; set
            {
                if (_maskOffsetAngle != value)
                {
                    _maskOffsetAngle = value;
                    TriggerChange();
                }
            }
        }

        public int HatOffsetX
        {
            get => _hatOffsetX; set
            {
                if (_hatOffsetX != value)
                {
                    _hatOffsetX = value;
                    TriggerChange();
                }
            }
        }

        public int HatOffsetY
        {
            get => _hatOffsetY; set
            {
                if (_hatOffsetY != value)
                {
                    _hatOffsetY = value;
                    TriggerChange();
                }
            }
        }

        public float HatOffsetAngle
        {
            get => _hatOffsetAngle; set
            {
                if (_hatOffsetAngle != value)
                {
                    _hatOffsetAngle = value;
                    TriggerChange();
                }
            }
        }
    }
}
