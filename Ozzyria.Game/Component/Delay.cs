using Ozzyria.Game.Component.Attribute;

namespace Ozzyria.Game.Component
{
    [Options(Name = "Delay")]
    public class Delay : Component
    {
        [Savable]
        public float DelayInSeconds { get; set; } = 0.5f;
        [Savable]
        public float Timer { get; set; } = 0.5f;
        public bool Ready { get; set; } = false;

        public void Update(float deltaTime)
        {
            if (Ready)
            {
                return;
            }

            // accumulate time
            Timer += deltaTime;
            Ready = Timer >= DelayInSeconds;
        }

        public bool IsReady()
        {
            if (!Ready)
            {
                return false;
            }

            // Reset Delay
            Timer = 0;
            Ready = false;

            return true;
        }
    }
}
