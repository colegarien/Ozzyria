using Ozzyria.Content;

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
        private static Dictionary<uint, Image> textures = new Dictionary<uint, Image>();

        public List<TextureCoords> Images { get; set; } = new List<TextureCoords>();

        public void Render(Graphics graphics, int x, int y)
        {
            if (textures.Count <= 0)
            {
                // TODO store this somewhere for reuse somewhere else
                var registry = Registry.GetInstance();
                foreach (var kv in registry.Resources)
                {
                    textures[kv.Key] = Bitmap.FromFile(Content.Loader.Root() + "/Resources/Sprites/"+kv.Value+".png");
                }
            }

            foreach (var image in Images)
            {
                if (textures.ContainsKey(image.Resource))
                {
                    graphics.DrawImage(textures[image.Resource], new Rectangle(x, y, 32, 32), new Rectangle(image.TextureX, image.TextureY, 32, 32), GraphicsUnit.Pixel);
                } else
                {
                    graphics.FillRectangle(Brushes.Pink, new Rectangle(x, y, 32, 32));
                }
            }
        }
    }
}
