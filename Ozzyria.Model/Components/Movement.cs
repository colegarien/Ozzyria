using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Movement : Grecs.Component, ISerializable, IHydrateable
    {
        private float _previousX = 0f;
        public float PreviousX
        {
            get => _previousX; set
            {
                if (!_previousX.Equals(value))
                {
                    _previousX = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _previousY = 0f;
        public float PreviousY
        {
            get => _previousY; set
            {
                if (!_previousY.Equals(value))
                {
                    _previousY = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _layer = 1;
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

        
        private float _x = 0f;
        public float X
        {
            get => _x; set
            {
                if (!_x.Equals(value))
                {
                    _x = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _y = 0f;
        public float Y
        {
            get => _y; set
            {
                if (!_y.Equals(value))
                {
                    _y = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _collisionOffsetY = 0f;
        public float CollisionOffsetY
        {
            get => _collisionOffsetY; set
            {
                if (!_collisionOffsetY.Equals(value))
                {
                    _collisionOffsetY = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private CollisionShape _collisionShape = new CollisionShape{  };
        public CollisionShape CollisionShape
        {
            get => _collisionShape; set
            {
                if (!_collisionShape?.Equals(value) ?? (value != null))
                {
                    _collisionShape = value;
                    if (value != null) { _collisionShape.TriggerChange = TriggerChange; }
                    TriggerChange();
                }
            }
        }

        
        private float _speed = 0f;
        public float Speed
        {
            get => _speed; set
            {
                if (!_speed.Equals(value))
                {
                    _speed = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _moveDirection = 0f;
        public float MoveDirection
        {
            get => _moveDirection; set
            {
                if (!_moveDirection.Equals(value))
                {
                    _moveDirection = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _lookDirection = 0f;
        public float LookDirection
        {
            get => _lookDirection; set
            {
                if (!_lookDirection.Equals(value))
                {
                    _lookDirection = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _acceleration = 200f;
        public float ACCELERATION
        {
            get => _acceleration; set
            {
                if (!_acceleration.Equals(value))
                {
                    _acceleration = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _maxSpeed = 100f;
        public float MAX_SPEED
        {
            get => _maxSpeed; set
            {
                if (!_maxSpeed.Equals(value))
                {
                    _maxSpeed = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _turnSpeed = 5f;
        public float TURN_SPEED
        {
            get => _turnSpeed; set
            {
                if (!_turnSpeed.Equals(value))
                {
                    _turnSpeed = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "Movement";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(PreviousX);
            w.Write(PreviousY);
            w.Write(Layer);
            w.Write(X);
            w.Write(Y);
            w.Write(CollisionOffsetY);
            w.Write(CollisionShape.BoundingBox.Width);
            w.Write(CollisionShape.BoundingBox.Height);
            w.Write(CollisionShape.BoundingCircle.Radius);
            w.Write(Speed);
            w.Write(MoveDirection);
            w.Write(LookDirection);
            w.Write(ACCELERATION);
            w.Write(MAX_SPEED);
            w.Write(TURN_SPEED);
        }

        public void Read(System.IO.BinaryReader r)
        {
            PreviousX = r.ReadSingle();
            PreviousY = r.ReadSingle();
            Layer = r.ReadInt32();
            X = r.ReadSingle();
            Y = r.ReadSingle();
            CollisionOffsetY = r.ReadSingle();
            CollisionShape.BoundingBox.Width = r.ReadInt32();
            CollisionShape.BoundingBox.Height = r.ReadInt32();
            CollisionShape.BoundingCircle.Radius = r.ReadSingle();
            Speed = r.ReadSingle();
            MoveDirection = r.ReadSingle();
            LookDirection = r.ReadSingle();
            ACCELERATION = r.ReadSingle();
            MAX_SPEED = r.ReadSingle();
            TURN_SPEED = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("previousX"))
            {
                PreviousX = float.Parse(values["previousX"]);
            }
            if (values.HasValueFor("previousY"))
            {
                PreviousY = float.Parse(values["previousY"]);
            }
            if (values.HasValueFor("layer"))
            {
                Layer = int.Parse(values["layer"]);
            }
            if (values.HasValueFor("x"))
            {
                X = float.Parse(values["x"]);
            }
            if (values.HasValueFor("y"))
            {
                Y = float.Parse(values["y"]);
            }
            if (values.HasValueFor("collisionOffsetY"))
            {
                CollisionOffsetY = float.Parse(values["collisionOffsetY"]);
            }
            if (values.HasValueFor("collisionShape"))
            {
                var  values_collisionShape = values.Extract("collisionShape");
                if (values_collisionShape.HasValueFor("boundingBox"))
            {
                var  values_collisionShape_boundingBox = values_collisionShape.Extract("boundingBox");
                if (values_collisionShape_boundingBox.HasValueFor("width"))
            {
                CollisionShape.BoundingBox.Width = int.Parse(values_collisionShape_boundingBox["width"]);
            }
                if (values_collisionShape_boundingBox.HasValueFor("height"))
            {
                CollisionShape.BoundingBox.Height = int.Parse(values_collisionShape_boundingBox["height"]);
            }
            }
                if (values_collisionShape.HasValueFor("boundingCircle"))
            {
                var  values_collisionShape_boundingCircle = values_collisionShape.Extract("boundingCircle");
                if (values_collisionShape_boundingCircle.HasValueFor("radius"))
            {
                CollisionShape.BoundingCircle.Radius = float.Parse(values_collisionShape_boundingCircle["radius"]);
            }
            }
            }
            if (values.HasValueFor("speed"))
            {
                Speed = float.Parse(values["speed"]);
            }
            if (values.HasValueFor("moveDirection"))
            {
                MoveDirection = float.Parse(values["moveDirection"]);
            }
            if (values.HasValueFor("lookDirection"))
            {
                LookDirection = float.Parse(values["lookDirection"]);
            }
            if (values.HasValueFor("acceleration"))
            {
                ACCELERATION = float.Parse(values["acceleration"]);
            }
            if (values.HasValueFor("maxSpeed"))
            {
                MAX_SPEED = float.Parse(values["maxSpeed"]);
            }
            if (values.HasValueFor("turnSpeed"))
            {
                TURN_SPEED = float.Parse(values["turnSpeed"]);
            }
        }
    }
}
