using SFML.Graphics;

namespace Ozzyria.MapEditor
{
    class Colors
    {
        public static Color TileColor(int type)
        {
            // TODO OZ-5 : Render the actual tiles used in the game
            return type switch
            {
                1 => Color.Green,
                2 => Color.Blue,
                3 => Color.Red,
                4 => new Color(178,152,0,255),
                5 => new Color(155, 155, 155, 255),
                _ => Color.Transparent,
            };
        }

        public static Color DefaultElement()
        {
            return Color.White;
        }

        public static Color HoverElement()
        {
            return Color.Cyan;
        }

        public static Color SelectedElement()
        {
            return Color.Magenta;
        }

        public static Color HoverSelectedElement()
        {
            return Color.Yellow;
        }

    }
}
