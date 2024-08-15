using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Skeleton : Grecs.Component, ISerializable, IHydrateable
    {
        private SkeletonType _type;
        public SkeletonType Type
        {
            get => _type; set
            {
                if (!_type.Equals(value))
                {
                    _type = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private SkeletonPose _pose;
        public SkeletonPose Pose
        {
            get => _pose; set
            {
                if (!_pose.Equals(value))
                {
                    _pose = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _frame;
        public int Frame
        {
            get => _frame; set
            {
                if (!_frame.Equals(value))
                {
                    _frame = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _layer;
        public int Layer
        {
            get => _layer; set
            {
                if (!_layer.Equals(value))
                {
                    _layer = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _subLayer;
        public int SubLayer
        {
            get => _subLayer; set
            {
                if (!_subLayer.Equals(value))
                {
                    _subLayer = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private Direction _direction;
        public Direction Direction
        {
            get => _direction; set
            {
                if (!_direction.Equals(value))
                {
                    _direction = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _rootX;
        public int RootX
        {
            get => _rootX; set
            {
                if (!_rootX.Equals(value))
                {
                    _rootX = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _rootY;
        public int RootY
        {
            get => _rootY; set
            {
                if (!_rootY.Equals(value))
                {
                    _rootY = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _renderOffsetY;
        public int RenderOffsetY
        {
            get => _renderOffsetY; set
            {
                if (!_renderOffsetY.Equals(value))
                {
                    _renderOffsetY = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _weaponOffsetX;
        public int WeaponOffsetX
        {
            get => _weaponOffsetX; set
            {
                if (!_weaponOffsetX.Equals(value))
                {
                    _weaponOffsetX = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _weaponOffsetY;
        public int WeaponOffsetY
        {
            get => _weaponOffsetY; set
            {
                if (!_weaponOffsetY.Equals(value))
                {
                    _weaponOffsetY = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _weaponOffsetAngle;
        public float WeaponOffsetAngle
        {
            get => _weaponOffsetAngle; set
            {
                if (!_weaponOffsetAngle.Equals(value))
                {
                    _weaponOffsetAngle = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _armorOffsetX;
        public int ArmorOffsetX
        {
            get => _armorOffsetX; set
            {
                if (!_armorOffsetX.Equals(value))
                {
                    _armorOffsetX = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _armorOffsetY;
        public int ArmorOffsetY
        {
            get => _armorOffsetY; set
            {
                if (!_armorOffsetY.Equals(value))
                {
                    _armorOffsetY = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _armorOffsetAngle;
        public float ArmorOffsetAngle
        {
            get => _armorOffsetAngle; set
            {
                if (!_armorOffsetAngle.Equals(value))
                {
                    _armorOffsetAngle = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _maskOffsetX;
        public int MaskOffsetX
        {
            get => _maskOffsetX; set
            {
                if (!_maskOffsetX.Equals(value))
                {
                    _maskOffsetX = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _maskOffsetY;
        public int MaskOffsetY
        {
            get => _maskOffsetY; set
            {
                if (!_maskOffsetY.Equals(value))
                {
                    _maskOffsetY = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _maskOffsetAngle;
        public float MaskOffsetAngle
        {
            get => _maskOffsetAngle; set
            {
                if (!_maskOffsetAngle.Equals(value))
                {
                    _maskOffsetAngle = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _hatOffsetX;
        public int HatOffsetX
        {
            get => _hatOffsetX; set
            {
                if (!_hatOffsetX.Equals(value))
                {
                    _hatOffsetX = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _hatOffsetY;
        public int HatOffsetY
        {
            get => _hatOffsetY; set
            {
                if (!_hatOffsetY.Equals(value))
                {
                    _hatOffsetY = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _hatOffsetAngle;
        public float HatOffsetAngle
        {
            get => _hatOffsetAngle; set
            {
                if (!_hatOffsetAngle.Equals(value))
                {
                    _hatOffsetAngle = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "Skeleton";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((int)Type);
            w.Write((int)Pose);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Type = (SkeletonType)r.ReadInt32();
            Pose = (SkeletonPose)r.ReadInt32();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("type"))
            {
                Type = (SkeletonType)Enum.Parse(typeof(SkeletonType), values["type"], true);
            }
            if (values.HasValueFor("pose"))
            {
                Pose = (SkeletonPose)Enum.Parse(typeof(SkeletonPose), values["pose"], true);
            }
            if (values.HasValueFor("frame"))
            {
                Frame = int.Parse(values["frame"]);
            }
            if (values.HasValueFor("layer"))
            {
                Layer = int.Parse(values["layer"]);
            }
            if (values.HasValueFor("subLayer"))
            {
                SubLayer = int.Parse(values["subLayer"]);
            }
            if (values.HasValueFor("direction"))
            {
                Direction = (Direction)Enum.Parse(typeof(Direction), values["direction"], true);
            }
            if (values.HasValueFor("rootX"))
            {
                RootX = int.Parse(values["rootX"]);
            }
            if (values.HasValueFor("rootY"))
            {
                RootY = int.Parse(values["rootY"]);
            }
            if (values.HasValueFor("renderOffsetY"))
            {
                RenderOffsetY = int.Parse(values["renderOffsetY"]);
            }
            if (values.HasValueFor("weaponOffsetX"))
            {
                WeaponOffsetX = int.Parse(values["weaponOffsetX"]);
            }
            if (values.HasValueFor("weaponOffsetY"))
            {
                WeaponOffsetY = int.Parse(values["weaponOffsetY"]);
            }
            if (values.HasValueFor("weaponOffsetAngle"))
            {
                WeaponOffsetAngle = float.Parse(values["weaponOffsetAngle"]);
            }
            if (values.HasValueFor("armorOffsetX"))
            {
                ArmorOffsetX = int.Parse(values["armorOffsetX"]);
            }
            if (values.HasValueFor("armorOffsetY"))
            {
                ArmorOffsetY = int.Parse(values["armorOffsetY"]);
            }
            if (values.HasValueFor("armorOffsetAngle"))
            {
                ArmorOffsetAngle = float.Parse(values["armorOffsetAngle"]);
            }
            if (values.HasValueFor("maskOffsetX"))
            {
                MaskOffsetX = int.Parse(values["maskOffsetX"]);
            }
            if (values.HasValueFor("maskOffsetY"))
            {
                MaskOffsetY = int.Parse(values["maskOffsetY"]);
            }
            if (values.HasValueFor("maskOffsetAngle"))
            {
                MaskOffsetAngle = float.Parse(values["maskOffsetAngle"]);
            }
            if (values.HasValueFor("hatOffsetX"))
            {
                HatOffsetX = int.Parse(values["hatOffsetX"]);
            }
            if (values.HasValueFor("hatOffsetY"))
            {
                HatOffsetY = int.Parse(values["hatOffsetY"]);
            }
            if (values.HasValueFor("hatOffsetAngle"))
            {
                HatOffsetAngle = float.Parse(values["hatOffsetAngle"]);
            }
        }
    }
}
