
using Ozzyria.Game.ECS;
using System;
using System.Collections.Generic;

namespace Ozzyria.MonoGameClient
{
    internal class BagState
    {
        public uint EntityId { get; set; }
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

        public BagState GetBag(uint entityId)
        {
            if (!BagContents.ContainsKey(entityId))
            {
                BagContents[entityId] = new BagState
                {
                    EntityId = entityId,
                    Contents = new List<Entity>(),
                    LastSyncRequest = DateTime.MinValue
                };
            }

            return BagContents[entityId];
        }

        public void SetBagContents(uint entityId,  List<Entity> contents)
        {
            if (!BagContents.ContainsKey(entityId))
            {
                BagContents[entityId] = new BagState{
                    EntityId = entityId,
                    Contents = new List<Entity>(),
                    LastSyncRequest = DateTime.MinValue
                };
            }

            BagContents[entityId].Contents.Clear();
            BagContents[entityId].Contents.AddRange(contents);
        }

        public void ForgetBagContents(uint entityId)
        {
            if(BagContents.ContainsKey((uint)entityId))
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
