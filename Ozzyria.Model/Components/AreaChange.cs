using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class AreaChange : Grecs.Component, ISerializable, IHydrateable
    {
        private string _newArea = "";
        public string NewArea
        {
            get => _newArea; set
            {
                if (!_newArea.Equals(value))
                {
                    _newArea = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _newX = 0f;
        public float NewX
        {
            get => _newX; set
            {
                if (!_newX.Equals(value))
                {
                    _newX = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _newY = 0f;
        public float NewY
        {
            get => _newY; set
            {
                if (!_newY.Equals(value))
                {
                    _newY = value;
                    
                    TriggerChange();
                }
            }
        }
        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(NewArea);
            w.Write(NewX);
            w.Write(NewY);
        }

        public void Read(System.IO.BinaryReader r)
        {
            NewArea = r.ReadString();
            NewX = r.ReadSingle();
            NewY = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("newArea"))
            {
                NewArea = values["newArea"].Trim('"');
            }
            if (values.HasValueFor("newX"))
            {
                NewX = float.Parse(values["newX"]);
            }
            if (values.HasValueFor("newY"))
            {
                NewY = float.Parse(values["newY"]);
            }
        }
    }
}
