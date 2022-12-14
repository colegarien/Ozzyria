using Microsoft.Xna.Framework.Input;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class Network : TickSystem
    {
        private Input lastSentInput = new Input();

        public override void Execute(float deltaTime, EntityContext context)
        {
            ProcessInput();
            MainGame._client?.HandleIncomingMessages(context);
        }

        private void ProcessInput()
        {
            var inputChanged = false;
            if(Keyboard.GetState().IsKeyDown(Keys.W) != lastSentInput.MoveUp)
            {
                inputChanged = true;
                lastSentInput.MoveUp = !lastSentInput.MoveUp;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) != lastSentInput.MoveDown)
            {
                inputChanged = true;
                lastSentInput.MoveDown = !lastSentInput.MoveDown;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) != lastSentInput.MoveLeft)
            {
                inputChanged = true;
                lastSentInput.MoveLeft = !lastSentInput.MoveLeft;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) != lastSentInput.MoveRight)
            {
                inputChanged = true;
                lastSentInput.MoveRight = !lastSentInput.MoveRight;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q) != lastSentInput.TurnLeft)
            {
                inputChanged = true;
                lastSentInput.TurnLeft = !lastSentInput.TurnLeft;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E) != lastSentInput.TurnRight)
            {
                inputChanged = true;
                lastSentInput.TurnRight = !lastSentInput.TurnRight;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) != lastSentInput.Attack)
            {
                inputChanged = true;
                lastSentInput.Attack = !lastSentInput.Attack;
            }

            if (inputChanged)
            {
                MainGame._client?.SendInput(lastSentInput);
            }
        }
    }
}
