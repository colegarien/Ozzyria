using SkiaSharp;

namespace Ozzyria.Gryp.Models
{
    internal static class Paints
    {
        public static SKColor CanvasColor = new SKColor(
            red: (byte)48,
            green: (byte)59,
            blue: (byte)61,
            alpha: (byte)255
        );

        public static SKPaint CanvasGridPaint = new SKPaint
        {
            Color = new SKColor(
                red: (byte)92,
                green: (byte)128,
                blue: (byte)188,
                alpha: (byte)255
            ),
            StrokeWidth = 1,
            IsAntialias = false
        };

        public static SKPaint MapBackingPaint = new SKPaint
        {
            Color = new SKColor(
                red: (byte)77,
                green: (byte)80,
                blue: (byte)97,
                alpha: (byte)255
            ),
            StrokeWidth = 1,
            IsAntialias = false
        };

        public static SKPaint MapGridOverlayPaint = new SKPaint
        {
            Color = new SKColor(
                        red: (byte)205,
                        green: (byte)209,
                        blue: (byte)196,
                        alpha: (byte)255
            ),
            StrokeWidth = 1,
            IsStroke = true,
            IsAntialias = false
        };

        public static SKPaint TileHighlightPaint = new SKPaint
        {
            Color = new SKColor(
                red: (byte)232,
                green: (byte)197,
                blue: (byte)71,
                alpha: (byte)255
            ),
            StrokeWidth = 2,
            IsStroke = true,
            IsAntialias = false
        };

        public static SKPaint TileSelectionPaint = new SKPaint
        {
            Color = new SKColor(
                red: (byte)124,
                green: (byte)232,
                blue: (byte)72,
                alpha: (byte)255
            ),
            StrokeWidth = 2,
            IsStroke = true,
            IsAntialias = false
        };

        public static SKPaint WallEntityPaint = new SKPaint
        {
            Color = new SKColor(
                red: (byte)232,
                green: (byte)86,
                blue: (byte)215,
                alpha: (byte)255
            ),
            StrokeWidth = 2,
            IsStroke = true,
            IsAntialias = false
        };

        public static SKPaint MissingGraphicPaint = new SKPaint
        {
            Color = new SKColor(
                    red: (byte)255,
                    green: (byte)0,
                    blue: (byte)255,
                    alpha: (byte)255
            ),
            StrokeWidth = 1,
            IsAntialias = false
        };
    }
}
