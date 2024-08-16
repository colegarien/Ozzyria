using Ozzyria.Model.Types;

namespace Ozzyria.Model.Extensions
{
    public static class DelayExtension
    {
        public static void Update(this Delay delay, float deltaTime)
        {
            if (delay.Ready)
            {
                return;
            }

            // accumulate time
            delay.Timer += deltaTime;
            delay.Ready = delay.Timer >= delay.DelayInSeconds;
        }

        public static bool IsReady(this Delay delay)
        {
            if (!delay.Ready)
            {
                return false;
            }

            // Reset Delay
            delay.Timer = 0;
            delay.Ready = false;

            return true;
        }
    }
}
