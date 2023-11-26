
using Ozzyria.Game.ECS;
using System;
using System.Collections.Generic;

namespace Ozzyria.MonoGameClient
{
    internal class BagState
    {
        public uint EntityId { get; set; }
        public string Name { get; set; }
        public List<Entity> Contents { get; set; }
        public DateTime LastSyncRequest { get; set; }
    }

    internal class LocalState
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Experience { get; set; }
        public int MaxExperience { get; set; }

        public uint PlayerEntityId { get; set; }
        public Dictionary<uint, BagState> BagContents { get; set; } = new Dictionary<uint, BagState>();

        public bool HasBag(uint entityId)
        {
            return BagContents.ContainsKey(entityId);
        }

        public BagState GetBag(uint entityId)
        {
            if (!BagContents.ContainsKey(entityId))
            {
                return new BagState
                {
                    EntityId = entityId,
                    Name = "",
                    Contents = new List<Entity>(),
                    LastSyncRequest = DateTime.MinValue
                };
            }

            return BagContents[entityId];
        }

        public void SetBagContents(uint entityId, string name,  List<Entity> contents)
        {
            if (!BagContents.ContainsKey(entityId))
            {
                BagContents[entityId] = new BagState{
                    EntityId = entityId,
                    Name = name,
                    Contents = new List<Entity>(),
                    LastSyncRequest = DateTime.MinValue
                };
            }

            BagContents[entityId].Contents.Clear();
            BagContents[entityId].Contents.AddRange(contents);
            BagContents[entityId].Name = name;
        }

        public void ForgetBagContents(uint entityId)
        {
            if(BagContents.ContainsKey(entityId))
            {
                BagContents.Remove(entityId);
            }
        }

        public void ForgetBags()
        {
            BagContents.Clear();
        }
    }
}
