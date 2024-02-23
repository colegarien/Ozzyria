using Ozzyria.Game.Components;
using Grecs;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class LocalStateTracking : TriggerSystem
    {
        private MainGame _game;
        public LocalStateTracking(MainGame game, EntityContext context) : base(context)
        {
            _game = game;
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            if(entities.Length > 0 && entities[0].HasComponent(typeof(Stats)))
            {
                var localPlayer = entities[0];
                _game.LocalState.PlayerEntityId = localPlayer.id;

                var localStats = (Stats)localPlayer.GetComponent(typeof(Stats));
                _game.LocalState.Health = localStats.Health;
                _game.LocalState.MaxHealth = localStats.MaxHealth;
                _game.LocalState.Experience = localStats.Experience;
                _game.LocalState.MaxExperience = localStats.MaxExperience;
            }
        }

        protected override bool Filter(Entity entity)
        {
            return ((Player)entity.GetComponent(typeof(Player))).PlayerId == _game.Client.Id;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Player), typeof(Stats));
            var listener = context.CreateListener(query);
            listener.ListenToAdded = true;
            listener.ListenToChanged = true;

            return listener;
        }
    }
}
