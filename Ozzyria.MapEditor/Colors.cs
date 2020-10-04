﻿using SFML.Graphics;

namespace Ozzyria.MapEditor
{
    class Colors
    {
        public static Color TileColor(TileType type)
        {
            return type switch
            {
                TileType.Ground => Color.Green,
                TileType.Water => Color.Blue,
                TileType.Fence => Color.Red,
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
