using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class MovementIntent : Grecs.PooledComponent<MovementIntent>, ISerializable, IHydrateable
    {
        private bool _moveRight = false;
        public bool MoveRight
        {
            get => _moveRight; set
            {
                if (!_moveRight.Equals(value))
                {
                    _moveRight = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private bool _moveLeft = false;
        public bool MoveLeft
        {
            get => _moveLeft; set
            {
                if (!_moveLeft.Equals(value))
                {
                    _moveLeft = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private bool _moveDown = false;
        public bool MoveDown
        {
            get => _moveDown; set
            {
                if (!_moveDown.Equals(value))
                {
                    _moveDown = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private bool _moveUp = false;
        public bool MoveUp
        {
            get => _moveUp; set
            {
                if (!_moveUp.Equals(value))
                {
                    _moveUp = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "movement_intent";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(MoveRight);
            w.Write(MoveLeft);
            w.Write(MoveDown);
            w.Write(MoveUp);
        }

        public void Read(System.IO.BinaryReader r)
        {
            MoveRight = r.ReadBoolean();
            MoveLeft = r.ReadBoolean();
            MoveDown = r.ReadBoolean();
            MoveUp = r.ReadBoolean();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("moveRight"))
            {
                MoveRight = bool.Parse(values["moveRight"]);
            }
            if (values.HasValueFor("moveLeft"))
            {
                MoveLeft = bool.Parse(values["moveLeft"]);
            }
            if (values.HasValueFor("moveDown"))
            {
                MoveDown = bool.Parse(values["moveDown"]);
            }
            if (values.HasValueFor("moveUp"))
            {
                MoveUp = bool.Parse(values["moveUp"]);
            }
        }
    }
}
