using Ozzyria.Game.Components;
using Ozzyria.Game;
using Grecs;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class CameraTracking : TriggerSystem
    {
        private MainGame _game;
        public CameraTracking(MainGame game, EntityContext context) : base(context)
        {
            _game = game;
        }


        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent(typeof(Movement)))
                {
                    var movement = entity.GetComponent<Movement>();
                    _game.Camera.CenterView(movement.X, movement.Y);
                    // coordinates are centered so offset bounds by half a tile
                    _game.Camera.ApplyBounds(-Tile.HALF_DIMENSION, -Tile.HALF_DIMENSION, _game.TileMap.Width * Tile.DIMENSION - Tile.HALF_DIMENSION, _game.TileMap.Height * Tile.DIMENSION - Tile.HALF_DIMENSION);

                    return;
                }
            }
        }

        protected override bool Filter(Entity entity)
        {
            return entity.GetComponent<Player>().PlayerId == _game.Client.Id;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Player), typeof(Movement));
            var listener = context.CreateListener(query);
            listener.ListenToAdded = true;
            listener.ListenToChanged = true;

            return listener;
        }
    }
}
