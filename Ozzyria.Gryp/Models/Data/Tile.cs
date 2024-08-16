using Ozzyria.Content;
using SkiaSharp;

namespace Ozzyria.Gryp.Models.Data
{
    internal class Tile
    {
        public List<string> DrawableIds { get; set; } = new List<string>();

        public Tile Clone()
        {
            Tile tile = new Tile();
            tile.DrawableIds.AddRange(DrawableIds);
            return tile;
        }

        public bool Equal(Tile other)
        {
            if(DrawableIds.Count <= 0 && other.DrawableIds.Count <= 0) return true;
            if (DrawableIds.Count <= 0 || other.DrawableIds.Count <= 0) return false;

            // same if the bottom image is the same
            return DrawableIds[0] == other.DrawableIds[0];
        }

        public bool Same(Tile other)
        {
            if (DrawableIds.Count != other.DrawableIds.Count)
            {
                return false;
            }

            for(int i = 0; i < DrawableIds.Count; i++)
            {
                if (DrawableIds[i] != other.DrawableIds[i])
                    return false;
            }

            return true;
        }

        public void Render(SKCanvas canvas, float x, float y, float width=32, float height=32)
        {
            var registry = Registry.GetInstance();
            foreach (var id in DrawableIds)
            {
                if (registry.Drawables.ContainsKey(id) && TextureManager.HasImageForResource(registry.Drawables[id].Resource))
                {
                    var drawable = registry.Drawables[id];
                    var texture = TextureManager.GetImageForResource(drawable.Resource);
                    canvas.DrawImage(texture, new SKRect(drawable.Left, drawable.Top, drawable.Left + drawable.Width, drawable.Top + drawable.Height), new SKRect(x, y, x + width, y + height));
                }
                else
                {
                    canvas.DrawRect(new SKRect(x, y, x+ width, y+ height), Paints.MissingGraphicPaint);
                }
            }
        }
    }
}
