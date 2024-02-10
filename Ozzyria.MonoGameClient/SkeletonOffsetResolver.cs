using Ozzyria.Game.Components;
using System.Collections.Generic;

namespace Ozzyria.MonoGameClient
{
    internal class SkeletonOffsetResolver
    {
        private static Dictionary<string, SkeletonOffsets> offsets = new Dictionary<string, SkeletonOffsets>
        {
            { "0", SkeletonOffsets.HUMANOID_IDLE },
            { "0_2_1_0", SkeletonOffsets.HUMANOID_ATTACK_UP_0 },
            { "0_2_1_1", SkeletonOffsets.HUMANOID_ATTACK_UP_1 },
            { "0_2_1_2", SkeletonOffsets.HUMANOID_ATTACK_UP_2 },
            { "0_2_2_0", SkeletonOffsets.HUMANOID_ATTACK_DOWN_0 },
            { "0_2_2_1", SkeletonOffsets.HUMANOID_ATTACK_DOWN_1 },
            { "0_2_2_2", SkeletonOffsets.HUMANOID_ATTACK_DOWN_2 },
            { "0_2_3_0", SkeletonOffsets.HUMANOID_ATTACK_LEFT_0 },
            { "0_2_3_1", SkeletonOffsets.HUMANOID_ATTACK_LEFT_1 },
            { "0_2_3_2", SkeletonOffsets.HUMANOID_ATTACK_LEFT_2 },
            { "0_2_4_0", SkeletonOffsets.HUMANOID_ATTACK_RIGHT_0 },
            { "0_2_4_1", SkeletonOffsets.HUMANOID_ATTACK_RIGHT_1 },
            { "0_2_4_2", SkeletonOffsets.HUMANOID_ATTACK_RIGHT_2 },

            { "1", SkeletonOffsets.SLIME_IDLE }
        };


        public static SkeletonOffsets Get(SkeletonType type, SkeletonPose pose, Direction direction, int frame)
        {
            var guessA = $"{(int)type}_{(int)pose}_{(int)direction}_{frame}";
            var guessB = $"{(int)type}_{(int)pose}_{(int)direction}";
            var guessC = $"{(int)type}_{(int)pose}";
            var guessD = $"{(int)type}";

            if (offsets.ContainsKey(guessA))
            {
                return offsets[guessA];
            }
            else if (offsets.ContainsKey(guessB))
            {
                return offsets[guessB];
            }
            else if (offsets.ContainsKey(guessC))
            {
                return offsets[guessC];
            }
            else if (offsets.ContainsKey(guessD))
            {
                return offsets[guessD];
            }

            return new SkeletonOffsets();
        }

    }
}
