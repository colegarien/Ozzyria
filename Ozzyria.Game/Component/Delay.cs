﻿namespace Ozzyria.Game.Component
{
    public class Delay : Component
    {
        public float DelayInSeconds { get; set; } = 0.5f;
        public float Timer { get; set; } = 0f;
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