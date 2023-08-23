using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class LocalPlayer : TriggerSystem
    {
        public LocalPlayer(EntityContext context) : base(context)
        {
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            if (entities.Length > 0 && (MainGame._localPlayer == null || MainGame._localPlayer.id != entities[0].id))
                MainGame._localPlayer = entities[0];

            var playerLocation = ((Location)MainGame._localPlayer?.GetComponent(typeof(Location)))?.Area ?? "";
            if ((MainGame._tileMap == null || playerLocation != MainGame._tileMap?.Name) && playerLocation != "")
            {
                MainGame._tileMap = MainGame._worldLoader.LoadMap(playerLocation);
            }
        }

        protected override bool Filter(Entity entity)
        {
            return ((Player)entity.GetComponent(typeof(Player))).PlayerId == MainGame._client.Id;
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
