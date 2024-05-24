using Ozzyria.Content;
using SkiaSharp;
using System.Drawing;

namespace Ozzyria.Gryp.Models.Data
{
    internal class TextureCoords
    {
        public uint Resource { get; set; }
        public int TextureX { get; set; }
        public int TextureY { get; set; }
    }

    internal class TileData
    {
        private static Dictionary<uint, SKImage> textures = new Dictionary<uint, SKImage>();

        public List<TextureCoords> Images { get; set; } = new List<TextureCoords>
        {
            new TextureCoords
            {
                Resource = 1,
                TextureX= 0,
                TextureY=0
            }
        };

        public void Render(SKCanvas canvas, float x, float y, float width=32, float height=32)
        {
            if (textures.Count <= 0)
            {
                // TODO store this somewhere for reuse somewhere else
                var registry = Registry.GetInstance();
                foreach (var kv in registry.Resources)
                {
                    textures[kv.Key] = SKImage.FromEncodedData(SKData.Create(Content.Loader.Root() + "/Resources/Sprites/"+kv.Value+".png"));
                }
            }

            foreach (var image in Images)
            {
                if (textures.ContainsKey(image.Resource))
                {
                    canvas.DrawImage(textures[image.Resource], new SKRect(image.TextureX, image.TextureY, image.TextureX+32, image.TextureY+32), new SKRect(x, y, x + width, y + height));
                } else
                {
                    canvas.DrawRect(new SKRect(x, y, x+ width, y+ height), new SKPaint
                    {
                        Color = new SKColor(
                        red: (byte)255,
                        green: (byte)0,
                        blue: (byte)255,
                        alpha: (byte)255),
                        StrokeWidth = 1,
                        IsAntialias = true
                    });
                }
            }
        }
    }
}
