namespace Ozzyria.Gryp.Models.Data
{
    internal class TextureCoords
    {
        public int TextureX { get; set; }
        public int TextureY { get; set; }
    }

    internal class TileData
    {
        private static Image texture = null;

        public List<TextureCoords> Images { get; set; } = new List<TextureCoords>();

        public void Render(Graphics graphics, int x, int y)
        {
            if (texture == null)
            {
                texture = Bitmap.FromFile(Content.Loader.Root() + "/Resources/Sprites/outside_tileset_001.png");
            }

            foreach (var image in Images)
            {
                // TODO get master image from somewhere for textures of tiles?
                graphics.DrawImage(texture, new Rectangle(x, y, 32, 32), new Rectangle(image.TextureX, image.TextureY, 32, 32), GraphicsUnit.Pixel);
            }
        }
    }
}
