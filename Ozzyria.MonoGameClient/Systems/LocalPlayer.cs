using Microsoft.Xna.Framework;
using Ozzyria.Model.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering;
using Ozzyria.Content;
using Ozzyria.Content.Models;

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
            if ((_game.AreaData == null || playerLocation != _game.AreaData?.AreaMetaData?.AreaId) && playerLocation != "")
            {
                _game.LocalState.ForgetBags();
                _game.AreaData = Ozzyria.Content.Models.Area.AreaData.Retrieve(playerLocation);
                RebuildTileMapGraphics();
            }
        }

        private void RebuildTileMapGraphics()
        {
            var resourceRegistry = Registry.GetInstance();
            var pipeline = GraphicsPipeline.Get();

            pipeline.ClearTileGraphics();
            if(_game.AreaData?.TileData == null)
            {
                return;
            }

            for (var layer = 0; layer < (_game.AreaData.TileData?.Layers?.Length ?? 0); layer++)
            {
                for (var x = 0; x < (_game.AreaData.TileData?.Layers[layer]?.Length ?? 0); x++)
                {
                    for (var y = 0; y < (_game.AreaData.TileData?.Layers[layer][x]?.Length ?? 0); y++)
                    {
                        var graphicX = x * Constants.TILE_DIMENSION;
                        var graphicY = y * Constants.TILE_DIMENSION;

                        var subSpace = 0;
                        foreach (var drawableId in _game.AreaData.TileData?.Layers[layer][x][y])
                        {
                            if (resourceRegistry.Drawables.ContainsKey(drawableId))
                            {
                                var drawable = resourceRegistry.Drawables[drawableId];
                                var graphic = pipeline.GetTileGraphic();
                                graphic.Resource = drawable.Resource;
                                graphic.Layer = layer;
                                graphic.SubLayer = graphicY + drawable.RenderOffset;
                                graphic.SubSubLayer = subSpace + drawable.Subspace;
                                graphic.Destination = new Rectangle(graphicX, graphicY, drawable.Width, drawable.Height);
                                graphic.Source = new Rectangle(drawable.Left, drawable.Top, drawable.Width, drawable.Height);
                                graphic.Angle = drawable.BaseAngle;
                                graphic.Origin = new Vector2(drawable.OriginX, drawable.OriginY);
                                graphic.Colour = Color.White;
                                graphic.FlipVertically = drawable.FlipVertically;
                                graphic.FlipHorizontally = drawable.FlipHorizontally;
                            }
                            else
                            {
                                // TODO draw 'missing' graphic
                            }

                            subSpace++;
                        }
                    }
                }
            }
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
