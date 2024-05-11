namespace Ozzyria.Gryp.Models.Data
{
    internal class TextureCoords
    {
        public int TextureX { get; set; }
        public int TextureY { get; set; }
    }

    internal class TileData
    {
        public List<TextureCoords> Images { get; set; } = new List<TextureCoords>();

        public void Render(Graphics graphics, int x, int y)
        {
            graphics.FillRectangle(Brushes.Violet, x, y, 32, 32);
            graphics.DrawRectangle(Pens.Yellow, x, y, 32, 32);

            foreach (var image in Images)
            {
                // TODO get master image from somewhere for textures of tiles?
                graphics.DrawImage(null, new Rectangle(x, y, 32, 32), new Rectangle(image.TextureX, image.TextureY, 32, 32), GraphicsUnit.Pixel);
            }
        }
    }
}
