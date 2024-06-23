using Ozzyria.Content;
using SkiaSharp;

namespace Ozzyria.Gryp.Models.Data
{
    internal struct TextureCoords
    {
        public uint Resource { get; set; }
        public int TextureX { get; set; }
        public int TextureY { get; set; }
    }

    internal class TileData
    {
        private static Dictionary<uint, SKImage> textures = new Dictionary<uint, SKImage>();

        public List<TextureCoords> Images { get; set; } = new List<TextureCoords>();

        public bool Equal(TileData other)
        {
            if(Images.Count <= 0 && other.Images.Count <= 0) return true;
            if (Images.Count <= 0 || other.Images.Count <= 0) return false;

            // same if the bottom image is the same
            return Images[0].Resource == other.Images[0].Resource
                && Images[0].TextureX == other.Images[0].TextureX
                && Images[0].TextureY == other.Images[0].TextureY;
        }

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
                    canvas.DrawRect(new SKRect(x, y, x+ width, y+ height), Paints.MissingGraphicPaint);
                }
            }
        }
    }
}
