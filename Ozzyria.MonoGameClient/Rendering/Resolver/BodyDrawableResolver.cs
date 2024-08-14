using Ozzyria.Content;
using Ozzyria.Content.Models;
using Ozzyria.Game.Components;
using Ozzyria.Model.Types;

namespace Ozzyria.MonoGameClient.Rendering.Resolver
{
    internal class BodyDrawableResolver
    {
        public static Drawable Get(string bodyId, SkeletonPose pose, Direction direction, int frame)
        {
            var resourceRegistry = Registry.GetInstance();

            var poseKey = pose.ToString();
            var directionKey = direction.ToString();

            var guessA = $"{bodyId}_{poseKey}_{directionKey}_{frame}";
            var guessB = $"{bodyId}_{poseKey}_{directionKey}";
            var guessC = $"{bodyId}_{poseKey}";
            var guessD = $"{bodyId}";

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
