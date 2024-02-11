using Ozzyria.Game.Components;

namespace Ozzyria.MonoGameClient
{
    internal class SkeletonOffsetResolver
    {
        public static SkeletonOffsets Get(SkeletonType type, SkeletonPose pose, Direction direction, int frame)
        {
            var resourceRegistry = Game.Animation.Registry.GetInstance();

            var typeKey = type.ToString();
            var poseKey = pose.ToString();
            var directionKey = direction.ToString();

            var guessA = $"{typeKey}_{poseKey}_{directionKey}_{frame}";
            var guessB = $"{typeKey}_{poseKey}_{directionKey}";
            var guessC = $"{typeKey}_{poseKey}";
            var guessD = $"{typeKey}";

            if (resourceRegistry.SkeletonOffsets.ContainsKey(guessA))
            {
                return resourceRegistry.SkeletonOffsets[guessA];
            }
            else if (resourceRegistry.SkeletonOffsets.ContainsKey(guessB))
            {
                return resourceRegistry.SkeletonOffsets[guessB];
            }
            else if (resourceRegistry.SkeletonOffsets.ContainsKey(guessC))
            {
                return resourceRegistry.SkeletonOffsets[guessC];
            }
            else if (resourceRegistry.SkeletonOffsets.ContainsKey(guessD))
            {
                return resourceRegistry.SkeletonOffsets[guessD];
            }

            return new SkeletonOffsets();
        }

    }
}
