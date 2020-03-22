using System.Text;

namespace Ozzyria.Game
{
    public class Player
    {
        public int Id { get; set; } = -1;
        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Speed { get; set; } = 0f;
        public float MoveDirection { get; set; } = 0f;
        public float LookDirection { get; set; } = 0f;

        public byte[] Serialize()
        {
            return Encoding.ASCII.GetBytes($"{Id}|{X}|{Y}|{Speed}|{MoveDirection}|{LookDirection}");
        }

        public static Player Deserialize(byte[] buffer)
        {
            var data = Encoding.ASCII.GetString(buffer).Split('|');

            return new Player()
            {
                Id = data.Length > 0 ? int.Parse(data[0]) : -1,
                X = data.Length > 1 ? float.Parse(data[1]) : 0f,
                Y = data.Length > 2 ? float.Parse(data[2]) : 0f,
                Speed = data.Length > 3 ? float.Parse(data[3]) : 0f,
                MoveDirection = data.Length > 4 ? float.Parse(data[4]) : 0f,
                LookDirection = data.Length > 5 ? float.Parse(data[5]) : 0f,
            };
        }
    }
}
