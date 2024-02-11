using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;

namespace Ozzyria.MonoGameClient.Rendering.Resolver
{
    internal class BodyDrawableResolver
    {
        public static Drawable Get(BodyType type, SkeletonPose pose, Direction direction, int frame)
        {
            var resourceRegistry = Registry.GetInstance();

            var typeKey = type.ToString();
            var poseKey = pose.ToString();
            var directionKey = direction.ToString();

            var guessA = $"{typeKey}_{poseKey}_{directionKey}_{frame}";
            var guessB = $"{typeKey}_{poseKey}_{directionKey}";
            var guessC = $"{typeKey}_{poseKey}";
            var guessD = $"{typeKey}";

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
            else if (resourceRegistry.Drawables.ContainsKey(guessD))
            {
                return resourceRegistry.Drawables[guessD];
            }

            return new Drawable();
        }
    }
}
