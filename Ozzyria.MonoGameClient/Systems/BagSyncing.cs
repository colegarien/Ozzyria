using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class BagSyncing : TickSystem
    {
        private MainGame _game;
        public BagSyncing(MainGame game)
        {
            _game = game;
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var bagEntities = context.GetEntities(new EntityQuery().And(typeof(Bag)));
            foreach(var entity in bagEntities)
            {
                var localBagState = _game.LocalState.GetBag(entity.id);
                if (localBagState.LastSyncRequest <= DateTime.UtcNow.AddMinutes(-5))
                {
                    // re-request bag state if it's been a while and haven't requested recently
                    localBagState.LastSyncRequest = DateTime.UtcNow;
                    _game.Client?.RequestBagContents(entity.id);
                }
            }
        }
    }
}
