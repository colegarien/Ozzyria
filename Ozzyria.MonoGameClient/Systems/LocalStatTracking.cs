using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class LocalStatTracking : TriggerSystem
    {
        public LocalStatTracking(EntityContext context) : base(context)
        {
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            if(entities.Length > 0 && entities[0].HasComponent(typeof(Stats)))
            {
                var localPlayer = entities[0];

                var localStats = (Stats)localPlayer.GetComponent(typeof(Stats));
                MainGame._localStatBlock.Health = localStats.Health;
                MainGame._localStatBlock.MaxHealth = localStats.MaxHealth;
                MainGame._localStatBlock.Experience = localStats.Experience;
                MainGame._localStatBlock.MaxExperience = localStats.MaxExperience;
            }
        }

        protected override bool Filter(Entity entity)
        {
            return ((Player)entity.GetComponent(typeof(Player))).PlayerId == MainGame._client.Id;
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
