using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Dead : Grecs.PooledComponent<Dead>, ISerializable, IHydrateable
    {
        
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
