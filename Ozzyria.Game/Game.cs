using System;

namespace Ozzyria.Game
{
    public class Game
    {
        const float ACCELERATION = 200f;
        const float MAX_SPEED = 100f;
        const float TURN_SPEED = 5f;

        public Input[] inputs;
        public Player[] players;

        public Game(int maxPlayers)
        {
            inputs = new Input[maxPlayers];
            players = new Player[maxPlayers];
        }

        public void OnPlayerJoin(int id)
        {
            players[id] = new Player(){ Id = id };
            inputs[id] = new Input();
        }

        public void OnPlayerInput(int id, Input input)
        {
            inputs[id] = input;
        }

        public void OnPlayerLeave(int id)
        {
            players[id] = null;
            inputs[id] = null;
        }

        public void Update(float deltaTime)
        {
            for(var i = 0; i < players.Length; i++)
            {
                var input = inputs[i];
                var player = players[i];
                if(input == null || player == null)
                {
                    continue;
                }
                
                if (input.TurnLeft)
                {
                    player.LookDirection += TURN_SPEED * deltaTime;
                }
                if (input.TurnRight)
                {
                    player.LookDirection -= TURN_SPEED * deltaTime;
                }
                if (input.MoveUp || input.MoveDown || input.MoveLeft || input.MoveRight)
                {
                    player.Speed += ACCELERATION * deltaTime;
                    if (player.Speed > MAX_SPEED)
                    {
                        player.Speed = MAX_SPEED;
                    }
                }
                if (!input.MoveDown && !input.MoveUp && !input.MoveRight && !input.MoveLeft)
                {
                    if (player.Speed > 0)
                    {
                        player.Speed -= ACCELERATION * deltaTime;
                        if (player.Speed < 0.0f)
                        {
                            player.Speed = 0.0f;
                        }
                    }
                    else if (player.Speed < 0)
                    {
                        player.Speed += ACCELERATION * deltaTime;
                        if (player.Speed > 0.0f)
                        {
                            player.Speed = 0.0f;
                        }
                    }
                }

                if(input.MoveUp && !input.MoveLeft && !input.MoveRight && !input.MoveDown)
                {
                    player.MoveDirection = 0;
                }
                else if (input.MoveDown && !input.MoveLeft && !input.MoveRight && !input.MoveUp)
                {
                    player.MoveDirection = (float)(Math.PI);
                }
                else if(!input.MoveUp && !input.MoveDown)
                {
                    var sideways = (float)(Math.PI / 2f);
                    if (input.MoveRight)
                        player.MoveDirection = -sideways;
                    else if(input.MoveLeft)
                        player.MoveDirection = sideways;
                }
                else if(input.MoveUp && !input.MoveDown)
                {
                    var forwardFortyFive = (float)(Math.PI / 4f);
                    if (input.MoveRight)
                        player.MoveDirection = -forwardFortyFive;
                    else if(input.MoveLeft)
                        player.MoveDirection = forwardFortyFive;
                }
                else if(input.MoveDown && !input.MoveUp)
                {
                    var backwardFortyFive = (float)((3f * Math.PI) / 4f);
                    if (input.MoveRight)
                        player.MoveDirection = -backwardFortyFive;
                    else if(input.MoveLeft)
                        player.MoveDirection = backwardFortyFive;
                }

                ///
                /// UPDATE LOGIC HERE
                ///
                player.X += player.Speed * deltaTime * (float)Math.Sin(player.LookDirection + player.MoveDirection);
                player.Y += player.Speed * deltaTime * (float)Math.Cos(player.LookDirection + player.MoveDirection);
            }
        }

    }
}
