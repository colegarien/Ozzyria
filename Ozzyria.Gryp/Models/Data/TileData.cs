using Ozzyria.Content;
using SkiaSharp;

namespace Ozzyria.Gryp.Models.Data
{
    internal class TileData
    {
        private static Dictionary<uint, SKImage> textures = new Dictionary<uint, SKImage>();

        public List<string> DrawableIds { get; set; } = new List<string>();

        public bool Equal(TileData other)
        {
            if(DrawableIds.Count <= 0 && other.DrawableIds.Count <= 0) return true;
            if (DrawableIds.Count <= 0 || other.DrawableIds.Count <= 0) return false;

            // same if the bottom image is the same
            return DrawableIds[0] == other.DrawableIds[0];
        }

        public void Render(SKCanvas canvas, float x, float y, float width=32, float height=32)
        {
            var registry = Registry.GetInstance();
            if (textures.Count <= 0)
            {
                // TODO store this somewhere for reuse somewhere else
                foreach (var kv in registry.Resources)
                {
                    textures[kv.Key] = SKImage.FromEncodedData(SKData.Create(Content.Loader.Root() + "/Resources/Sprites/"+kv.Value+".png"));
                }
            }

            foreach (var id in DrawableIds)
            {
                if (registry.Drawables.ContainsKey(id))
                {
                    var drawable = registry.Drawables[id];
                    canvas.DrawImage(textures[drawable.Resource], new SKRect(drawable.Left, drawable.Top, drawable.Left + drawable.Width, drawable.Top + drawable.Height), new SKRect(x, y, x + width, y + height));
                } else
                {
                    canvas.DrawRect(new SKRect(x, y, x+ width, y+ height), Paints.MissingGraphicPaint);
                }
            }
        }
    }
}
