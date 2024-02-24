using Microsoft.Xna.Framework;
using Ozzyria.Game;
using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering;

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
                _game.LocalState.ForgetBags();
                _game.TileMap = _game.WorldLoader.LoadMap(playerLocation);
                RebuildTileMapGraphics();
            }
        }

        private void RebuildTileMapGraphics()
        {
            var pipeline = GraphicsPipeline.Get();

            pipeline.ClearTileGraphics();
            foreach (var layer in _game.TileMap?.Layers)
            {
                foreach (var tile in layer.Value)
                {

                    var tileSetResource = _game.TileMap?.Resource ?? 1;
                    var x = tile.X * Tile.DIMENSION;
                    var y = tile.Y * Tile.DIMENSION;
                    var subLayer = y + tile.Z;
                    var origin = Vector2.Zero;//new Vector2(Tile.DIMENSION / 2, Tile.DIMENSION / 2);

                    var subSpace = 0;
                    var baseGraphic = pipeline.GetTileGraphic();
                    baseGraphic.Resource = tileSetResource;
                    baseGraphic.Layer = layer.Key;
                    baseGraphic.SubLayer = subLayer;
                    baseGraphic.SubSubLayer = subSpace;
                    baseGraphic.Destination = new Rectangle(x, y, Tile.DIMENSION, Tile.DIMENSION);
                    baseGraphic.Source = new Rectangle(tile.TextureCoordX * Tile.DIMENSION, tile.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION);
                    baseGraphic.Angle = 0f;
                    baseGraphic.Origin = origin;
                    baseGraphic.Colour = Color.White;
                    baseGraphic.FlipVertically = false;
                    baseGraphic.FlipHorizontally = false;
                    subSpace++;


                    foreach (var decal in tile.Decals)
                    {
                        var decalGraphic = pipeline.GetTileGraphic();
                        decalGraphic.Resource = tileSetResource;
                        decalGraphic.Layer = layer.Key;
                        decalGraphic.SubLayer = subLayer;
                        decalGraphic.SubSubLayer = subSpace;
                        decalGraphic.Destination = new Rectangle(x, y, Tile.DIMENSION, Tile.DIMENSION);
                        decalGraphic.Source = new Rectangle(decal.TextureCoordX * Tile.DIMENSION, decal.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION);
                        decalGraphic.Angle = 0f;
                        decalGraphic.Origin = origin;
                        decalGraphic.Colour = Color.White;
                        decalGraphic.FlipVertically = false;
                        decalGraphic.FlipHorizontally = false;
                        subSpace++;
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
