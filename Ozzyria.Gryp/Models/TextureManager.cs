using Ozzyria.Content;
using SkiaSharp;

namespace Ozzyria.Gryp.Models
{
    internal class TextureManager
    {
        private static Dictionary<uint, SKImage>? _textures;

        private static void VerifyCache()
        {
            if (_textures == null)
            {
                var registry = Registry.GetInstance();
                _textures = new Dictionary<uint, SKImage>();
                foreach (var kv in registry.Resources)
                {
                    _textures[kv.Key] = SKImage.FromEncodedData(SKData.Create(Content.Loader.Root() + "/Resources/Sprites/" + kv.Value + ".png"));
                }
            }
        }

        public static bool HasImageForResource(uint resourceId)
        {
            VerifyCache();
            return _textures != null && _textures.ContainsKey(resourceId);
        }

        public static SKImage? GetImageForResource(uint resourceId)
        {
            return HasImageForResource(resourceId) && _textures != null
                ? _textures[resourceId]
                : null;
        }

    }
}
