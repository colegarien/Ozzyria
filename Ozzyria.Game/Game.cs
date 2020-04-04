using Ozzyria.Game.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game
{
    public class Game
    {
        public Dictionary<int, Input> inputs;
        public Dictionary<int, Player> players;

        public List<ExperienceOrb> orbs;
        private float eventTimer = 0;

        public Game()
        {
            inputs = new Dictionary<int, Input>();
            players = new Dictionary<int, Player>();

            orbs = new List<ExperienceOrb>();
            orbs.Add(new ExperienceOrb { X = 400, Y = 300 });
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
            eventTimer += deltaTime;
            foreach(var idPlayerPair in players)
            {
                var input = inputs.GetValueOrDefault(idPlayerPair.Key) ?? new Input();
                var player = idPlayerPair.Value;

                player.Update(deltaTime, input);
                if (player.Attacking)
                {
                    var playersInRange = players.Values.Where(p => p.Id != player.Id && Math.Sqrt(Math.Pow(p.X - player.X, 2) + Math.Pow(p.Y - player.Y, 2)) <= player.AttackRange);
                    foreach(var target in playersInRange)
                    {
                        var angleToTarget = (float)Math.Atan2(target.X - player.X, target.Y - player.Y);
                        if (AngleHelper.IsInArc(angleToTarget, player.LookDirection, player.AttackAngle))
                        {
                            target.Damage(player.AttackDamage);
                        }
                    }
                }
            }

            foreach(var orb in orbs)
            {
                orb.Update(deltaTime, players.Values.ToArray());
            }
            // remove any orbs that have been absorbed
            orbs = orbs.Where(o => !o.HasBeenAbsorbed).ToList(); 

            if(eventTimer > 5)
            {
                eventTimer = 0;
                orbs.Add(new ExperienceOrb { X = 400, Y = 300 });
            }
        }

    }
}
