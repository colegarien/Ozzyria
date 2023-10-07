
using Ozzyria.Game.ECS;
using System.Collections.Generic;

namespace Ozzyria.MonoGameClient
{
    internal class LocalState
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Experience { get; set; }
        public int MaxExperience { get; set; }

        public List<Entity> InventoryContents { get; set; }
    }
}
