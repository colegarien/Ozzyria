﻿using Ozzyria.Model.Components;
using Grecs;
using Ozzyria.Content.Models;

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
                    if (_game.AreaData?.TileData != null)
                    {
                        _game.Camera.ApplyBounds(0, 0, _game.AreaData.TileData.Width * Constants.TILE_DIMENSION, _game.AreaData.TileData.Height * Constants.TILE_DIMENSION);
                    }

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
