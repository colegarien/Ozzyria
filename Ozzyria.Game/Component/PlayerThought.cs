using Ozzyria.Game.Component.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Component
{
    [Options(Name = "PlayerThought")]
    public class PlayerThought : Thought
    {
        public override void Update(float deltaTime, EntityContext context)
        {
            var input = (Input)Owner.GetComponent(typeof(Input));
            var movement = (Movement)Owner.GetComponent(typeof(Movement));
            var combat = (Combat)Owner.GetComponent(typeof(Combat));

            HandleInput(deltaTime, input, movement);

            combat.Update(deltaTime, input.Attack);
            movement.Update(deltaTime);
        }


        private void HandleInput(float deltaTime, Input input, Movement movement)
        {
            if (input.TurnLeft)
            {
                movement.TurnLeft(deltaTime);
            }
            if (input.TurnRight)
            {
                movement.TurnRight(deltaTime);
            }

            if (input.MoveUp || input.MoveDown || input.MoveLeft || input.MoveRight)
            {
                movement.SpeedUp(deltaTime);
            }
            if (!input.MoveDown && !input.MoveUp && !input.MoveRight && !input.MoveLeft)
            {
                movement.SlowDown(deltaTime);
            }

            if (input.MoveUp && !input.MoveLeft && !input.MoveRight && !input.MoveDown)
            {
                movement.MoveForward(deltaTime);
            }
            else if (input.MoveDown && !input.MoveLeft && !input.MoveRight && !input.MoveUp)
            {
                movement.MoveBackward(deltaTime);
            }
            else if (!input.MoveUp && !input.MoveDown)
            {
                if (input.MoveRight)
                    movement.MoveRight(deltaTime);
                else if (input.MoveLeft)
                    movement.MoveLeft(deltaTime);
            }
            else if (input.MoveUp && !input.MoveDown)
            {
                if (input.MoveRight)
                    movement.MoveForwardRight(deltaTime);
                else if (input.MoveLeft)
                    movement.MoveForwardLeft(deltaTime);
            }
            else if (input.MoveDown && !input.MoveUp)
            {
                if (input.MoveRight)
                    movement.MoveBackwardRight(deltaTime);
                else if (input.MoveLeft)
                    movement.MoveBackwardLeft(deltaTime);
            }
        }
    }
}
