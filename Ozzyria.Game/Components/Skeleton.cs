using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }

    public struct SkeletonOffsets
    {
        public int RootOffsetX { get; set; }
        public int RootOffsetY { get; set; }
        public int RenderOffsetY { get; set; }
        public int WeaponOffsetX { get; set; }
        public int WeaponOffsetY { get; set; }
        public float WeaponOffsetAngle { get; set; }

        // TODO pull this from a registry of some kind instead, one for each skeleton?
        public static SkeletonOffsets SLIME_IDLE = new SkeletonOffsets
        {
            RootOffsetX = 0,
            RootOffsetY = 0,
            RenderOffsetY = 20,
        };
        public static SkeletonOffsets HUMANOID_IDLE = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 24 - 16,
            WeaponOffsetY = 14 - 32,
            WeaponOffsetAngle = -1.570796f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_RIGHT_0 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 24 - 16,
            WeaponOffsetY = 14 - 32,
            WeaponOffsetAngle = -1.570796f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_RIGHT_1 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 29 - 16,
            WeaponOffsetY = 14 - 32,
            WeaponOffsetAngle = -0.5323254f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_RIGHT_2 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 28 - 16,
            WeaponOffsetY = 21 - 32,
            WeaponOffsetAngle = 0.20944f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_LEFT_0 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 8 - 16,
            WeaponOffsetY = 14 - 32,
            WeaponOffsetAngle = -1.580796f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_LEFT_1 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 3 - 16,
            WeaponOffsetY = 14 - 32,
            WeaponOffsetAngle = 3.7001f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_LEFT_2 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 4 - 16,
            WeaponOffsetY = 21 - 32,
            WeaponOffsetAngle = 2.9059732f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_DOWN_0 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 22 - 16,
            WeaponOffsetY = 19 - 32,
            WeaponOffsetAngle = 0.418879f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_DOWN_1 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 18 - 16,
            WeaponOffsetY = 25 - 32,
            WeaponOffsetAngle = 2.0612338f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_DOWN_2 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 8 - 16,
            WeaponOffsetY = 20 - 32,
            WeaponOffsetAngle = 2.58309f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_UP_0 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 22 - 16,
            WeaponOffsetY = 19 - 32,
            WeaponOffsetAngle = 3.56047f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_UP_1 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 18 - 16,
            WeaponOffsetY = 25 - 32,
            WeaponOffsetAngle = 5.202826f,
        };
        public static SkeletonOffsets HUMANOID_ATTACK_UP_2 = new SkeletonOffsets
        {
            RootOffsetX = -16,
            RootOffsetY = 0,
            RenderOffsetY = 0,
            WeaponOffsetX = 8 - 16,
            WeaponOffsetY = 20 - 32,
            WeaponOffsetAngle = 5.724682f,
        };
    }
}
