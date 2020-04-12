using Ozzyria.Game.Component;

namespace Ozzyria.Game
{
    public class Player
    {
        public int Id { get; set; } = -1;
        public Movement Movement { get; set; } = new Movement();
        public Stats Stats { get; set; } = new Stats();
        public Combat Combat { get; set; } = new Combat();

        public void Update(float deltaTime, Input input)
        {
            HandleInput(deltaTime, input);

            Combat.Update(deltaTime, input.Attack);
            Movement.Update(deltaTime);
        }

        private void HandleInput(float deltaTime, Input input)
        {
            if (input.TurnLeft)
            {
                Movement.TurnLeft(deltaTime);
            }
            if (input.TurnRight)
            {
                Movement.TurnRight(deltaTime);
            }

            if (input.MoveUp || input.MoveDown || input.MoveLeft || input.MoveRight)
            {
                Movement.SpeedUp(deltaTime);
            }
            if (!input.MoveDown && !input.MoveUp && !input.MoveRight && !input.MoveLeft)
            {
                Movement.SlowDown(deltaTime);
            }

            if (input.MoveUp && !input.MoveLeft && !input.MoveRight && !input.MoveDown)
            {
                Movement.MoveForward(deltaTime);
            }
            else if (input.MoveDown && !input.MoveLeft && !input.MoveRight && !input.MoveUp)
            {
                Movement.MoveBackward(deltaTime);
            }
            else if (!input.MoveUp && !input.MoveDown)
            {
                if (input.MoveRight)
                    Movement.MoveRight(deltaTime);
                else if (input.MoveLeft)
                    Movement.MoveLeft(deltaTime);
            }
            else if (input.MoveUp && !input.MoveDown)
            {
                if (input.MoveRight)
                    Movement.MoveForwardRight(deltaTime);
                else if (input.MoveLeft)
                    Movement.MoveForwardLeft(deltaTime);
            }
            else if (input.MoveDown && !input.MoveUp)
            {
                if (input.MoveRight)
                    Movement.MoveBackwardRight(deltaTime);
                else if (input.MoveLeft)
                    Movement.MoveBackwardLeft(deltaTime);
            }
        }
    }
}
