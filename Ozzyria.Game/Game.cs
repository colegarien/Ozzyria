using System;
using System.Collections.Generic;

namespace Ozzyria.Game
{
    public class Game
    {
        public Dictionary<int, Input> inputs;
        public Dictionary<int, Player> players;

        public Game()
        {
            inputs = new Dictionary<int, Input>();
            players = new Dictionary<int, Player>();
        }

        public void OnPlayerJoin(int id)
        {
            players[id] = new Player() { Id = id };
            inputs[id] = new Input();
        }

        public void OnPlayerInput(int id, Input input)
        {
            inputs[id] = input;
        }

        public void OnPlayerLeave(int id)
        {
            if (players.ContainsKey(id))
            {
                players.Remove(id);
            }
            if (inputs.ContainsKey(id))
            {
                inputs.Remove(id);
            }
        }

        public void Update(float deltaTime)
        {
            foreach(var idPlayerPair in players)
            {
                var input = inputs.GetValueOrDefault(idPlayerPair.Key) ?? new Input();
                var player = idPlayerPair.Value;

                player.Update(deltaTime, input);
            }
        }

    }
}
