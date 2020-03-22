using System;

namespace Ozzyria.Game
{
    public class Player
    {
        const float ACCELERATION = 200f;
        const float MAX_SPEED = 100f;
        const float TURN_SPEED = 5f;

        public int Id { get; set; } = -1;
        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Speed { get; set; } = 0f;
        public float MoveDirection { get; set; } = 0f;
        public float LookDirection { get; set; } = 0f;

        public void Update(float deltaTime, Input input)
        {

            if (input.TurnLeft)
            {
                LookDirection += TURN_SPEED * deltaTime;
            }
            if (input.TurnRight)
            {
                LookDirection -= TURN_SPEED * deltaTime;
            }
            if (input.MoveUp || input.MoveDown || input.MoveLeft || input.MoveRight)
            {
                Speed += ACCELERATION * deltaTime;
                if (Speed > MAX_SPEED)
                {
                    Speed = MAX_SPEED;
                }
            }
            if (!input.MoveDown && !input.MoveUp && !input.MoveRight && !input.MoveLeft)
            {
                if (Speed > 0)
                {
                    Speed -= ACCELERATION * deltaTime;
                    if (Speed < 0.0f)
                    {
                        Speed = 0.0f;
                    }
                }
                else if (Speed < 0)
                {
                    Speed += ACCELERATION * deltaTime;
                    if (Speed > 0.0f)
                    {
                        Speed = 0.0f;
                    }
                }
            }

            if (input.MoveUp && !input.MoveLeft && !input.MoveRight && !input.MoveDown)
            {
                MoveDirection = 0;
            }
            else if (input.MoveDown && !input.MoveLeft && !input.MoveRight && !input.MoveUp)
            {
                MoveDirection = (float)(Math.PI);
            }
            else if (!input.MoveUp && !input.MoveDown)
            {
                var sideways = (float)(Math.PI / 2f);
                if (input.MoveRight)
                    MoveDirection = -sideways;
                else if (input.MoveLeft)
                    MoveDirection = sideways;
            }
            else if (input.MoveUp && !input.MoveDown)
            {
                var forwardFortyFive = (float)(Math.PI / 4f);
                if (input.MoveRight)
                    MoveDirection = -forwardFortyFive;
                else if (input.MoveLeft)
                    MoveDirection = forwardFortyFive;
            }
            else if (input.MoveDown && !input.MoveUp)
            {
                var backwardFortyFive = (float)((3f * Math.PI) / 4f);
                if (input.MoveRight)
                    MoveDirection = -backwardFortyFive;
                else if (input.MoveLeft)
                    MoveDirection = backwardFortyFive;
            }

            ///
            /// UPDATE LOGIC HERE
            ///
            X += Speed * deltaTime * (float)Math.Sin(LookDirection + MoveDirection);
            Y += Speed * deltaTime * (float)Math.Cos(LookDirection + MoveDirection);
        }
    }
}
