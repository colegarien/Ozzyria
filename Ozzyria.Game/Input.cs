using System.Text;

namespace Ozzyria.Game
{
    public class Input
    {
        public bool MoveUp { get; set; } = false;
        public bool MoveDown { get; set; } = false;
        public bool MoveLeft { get; set; } = false;
        public bool MoveRight { get; set; } = false;
        public bool TurnLeft { get; set; } = false;
        public bool TurnRight { get; set; } = false;

        public byte[] Serialize()
        {
            return Encoding.ASCII.GetBytes($"{MoveUp}|{MoveDown}|{MoveLeft}|{MoveRight}|{TurnLeft}|{TurnRight}");
        }

        public static Input Deserialize(byte[] buffer)
        {
            var data = Encoding.ASCII.GetString(buffer).Split('|');

            return new Input()
            {
                MoveUp = data.Length > 0 && bool.Parse(data[0]),
                MoveDown = data.Length > 1 && bool.Parse(data[1]),
                MoveLeft = data.Length > 2 && bool.Parse(data[2]),
                MoveRight = data.Length > 3 && bool.Parse(data[3]),
                TurnLeft = data.Length > 4 && bool.Parse(data[4]),
                TurnRight = data.Length > 5 && bool.Parse(data[5]),
            };
        }
    }
}
