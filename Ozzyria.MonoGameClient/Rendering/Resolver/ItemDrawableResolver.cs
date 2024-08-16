using Ozzyria.Content;
using Ozzyria.Content.Models;
using Ozzyria.Model.Types;

namespace Ozzyria.MonoGameClient.Rendering.Resolver
{
    internal class ItemDrawableResolver
    {
        public static Drawable Get(string itemId, Direction direction, int frame)
        {
            var resourceRegistry = Registry.GetInstance();

            var directionKey = direction.ToString();
            var guessA = $"{itemId}_{directionKey}_{frame}";
            var guessB = $"{itemId}_{directionKey}";
            var guessC = $"{itemId}";

            if (resourceRegistry.Drawables.ContainsKey(guessA))
            {
                return resourceRegistry.Drawables[guessA];
            }
            else if (resourceRegistry.Drawables.ContainsKey(guessB))
            {
                return resourceRegistry.Drawables[guessB];
            }
            else if (resourceRegistry.Drawables.ContainsKey(guessC))
            {
                return resourceRegistry.Drawables[guessC];
            }

            return new Drawable();
        }
    }
}
