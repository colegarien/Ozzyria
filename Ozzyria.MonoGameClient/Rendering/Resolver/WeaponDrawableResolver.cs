using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;

namespace Ozzyria.MonoGameClient.Rendering.Resolver
{
    internal class WeaponDrawableResolver
    {
        public static Drawable GetWeapon(WeaponType type, string weaponId)
        {
            var resourceRegistry = Registry.GetInstance();
            var typeKey = type.ToString();
            var guessA = $"{typeKey}_{weaponId}";

            if (resourceRegistry.Drawables.ContainsKey(guessA))
            {
                return resourceRegistry.Drawables[guessA];
            }

            return new Drawable();
        }
        public static Drawable GetWeaponTrail(WeaponType type, string weaponId)
        {
            var resourceRegistry = Registry.GetInstance();
            var typeKey = type.ToString();
            var guessA = $"{typeKey}_Trail_{weaponId}";
            var guessB = $"{typeKey}_Trail_Basic";
            var guessC = $"BasicTrail";

            if (resourceRegistry.Drawables.ContainsKey(guessA))
            {
                return resourceRegistry.Drawables[guessA];
            }
            else if(resourceRegistry.Drawables.ContainsKey(guessB))
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
