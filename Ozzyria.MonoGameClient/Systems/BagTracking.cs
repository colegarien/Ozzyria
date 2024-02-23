using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.UI;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class BagTracking : TriggerSystem
    {
        private MainGame _game;
        public BagTracking(MainGame game, EntityContext context) : base(context)
        {
            _game = game;
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                if (!entity.HasComponent(typeof(Bag)))
                {
                    _game.LocalState.ForgetBagContents(entity.id);
                    continue;
                }

                var bag = (Bag)entity.GetComponent(typeof(Bag));
                _game.LocalState.SetBagContents(entity.id, bag.Name, bag.Contents);
            }
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Bag));
            var listener = context.CreateListener(query);
            listener.ListenToAdded = true;
            listener.ListenToChanged = true;
            listener.ListenToRemoved = true;

            return listener;
        }
    }
}
