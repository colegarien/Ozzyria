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

            var playerEntityMap = ((Player)MainGame._localPlayer?.GetComponent(typeof(Player)))?.Map ?? "";
            if ((MainGame._tileMap == null || playerEntityMap != MainGame._tileMap?.Name) && playerEntityMap != "")
            {
                MainGame._tileMap = MainGame._worldLoader.LoadMap(playerEntityMap);
            }
        }

        protected override bool Filter(Entity entity)
        {
            return ((Player)entity.GetComponent(typeof(Player))).PlayerId == MainGame._client.Id;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Player));
            return context.CreateListener(query);
        }
    }
}
