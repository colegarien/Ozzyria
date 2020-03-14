using System.Text;

namespace Ozzyria.Networking.Model
{
    public class PlayerState
    {
        public int Id { get; set; } = -1;
        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Speed { get; set; } = 0f;
        public float Direction { get; set; } = 0f;

        public byte[] Serialize()
        {
            return Encoding.ASCII.GetBytes($"{Id}|{X}|{Y}|{Speed}|{Direction}");
        }

        public static PlayerState Deserialize(byte[] buffer)
        {
            var data = Encoding.ASCII.GetString(buffer).Split('|');

            return new PlayerState()
            {
                Id = data.Length > 0 ? int.Parse(data[0]) : -1,
                X = data.Length > 1 ? float.Parse(data[1]) : 0f,
                Y = data.Length > 2 ? float.Parse(data[2]) : 0f,
                Speed = data.Length > 3 ? float.Parse(data[3]) : 0f,
                Direction = data.Length > 4 ? float.Parse(data[4]) : 0f,
            };
        }
    }
}
