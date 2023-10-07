using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class LocalPlayer : TriggerSystem
    {
        private MainGame _game;
        public LocalPlayer(MainGame game, EntityContext context) : base(context)
        {
            _game = game;
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            var localPlayer = entities[0];
            var playerLocation = ((Location)localPlayer?.GetComponent(typeof(Location)))?.Area ?? "";
            if ((_game.TileMap == null || playerLocation != _game.TileMap?.Name) && playerLocation != "")
            {
                _game.TileMap = _game.WorldLoader.LoadMap(playerLocation);
            }

            // re-requst latest inventory contents
            _game.Client?.RequestBagContents(localPlayer.id);
        }

        protected override bool Filter(Entity entity)
        {
            return ((Player)entity.GetComponent(typeof(Player))).PlayerId == _game.Client.Id;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Player), typeof(Location));
            var listener = context.CreateListener(query);
            listener.ListenToAdded = true;
            listener.ListenToChanged = true;

            return listener;
        }
    }
}
