using System.Text;

namespace Ozzyria.Networking.Model
{
    public class PlayerInput
    {
        public bool Up { get; set; } = false;
        public bool Down { get; set; } = false;
        public bool Left { get; set; } = false;
        public bool Right { get; set; } = false;
        public bool Quit { get; set; } = false;

        public byte[] Serialize()
        {
            return Encoding.ASCII.GetBytes($"{Up}|{Down}|{Left}|{Right}|{Quit}");
        }

        public static PlayerInput Deserialize(byte[] buffer)
        {
            var data = Encoding.ASCII.GetString(buffer).Split('|');

            return new PlayerInput()
            {
                Up = data.Length > 0 && bool.Parse(data[0]),
                Down = data.Length > 1 && bool.Parse(data[1]),
                Left = data.Length > 2 && bool.Parse(data[2]),
                Right = data.Length > 3 && bool.Parse(data[3]),
                Quit = data.Length > 4 && bool.Parse(data[4]),
            };
        }
    }
}
