using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Player : Grecs.Component, ISerializable, IHydrateable
    {
        private int _playerId = -1;
        public int PlayerId
        {
            get => _playerId; set
            {
                if (!_playerId.Equals(value))
                {
                    _playerId = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "player";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(PlayerId);
        }

        public void Read(System.IO.BinaryReader r)
        {
            PlayerId = r.ReadInt32();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("playerId"))
            {
                PlayerId = int.Parse(values["playerId"]);
            }
        }
    }
}
