using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class PlayerThought : Grecs.Component, ISerializable, IHydrateable
    {
        
        public string GetComponentIdentifier() {
            return "player_thought";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            
        }

        public void Read(System.IO.BinaryReader r)
        {
            
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            
        }
    }
}
