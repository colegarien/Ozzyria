using Microsoft.Xna.Framework;
using Ozzyria.Game;
using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MonoGameClient.Systems
{
    internal class RenderTracking : TriggerSystem
    {
        public static List<IDrawableInfo> finalDrawables = new List<IDrawableInfo>();
        public static List<IDrawableInfo> tileMapDrawables = new List<IDrawableInfo>();

        public Registry ResourceRegistry;

        private MainGame _game;

        public RenderTracking(MainGame game, EntityContext context) : base(context)
        {
            ResourceRegistry = Registry.GetInstance();
            _game = game;
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));

                if (entity.HasComponent(typeof(Player)) && ((Player)entity.GetComponent(typeof(Player))).PlayerId == _game.Client.Id && movement != null)
                {
                    _game.Camera.CenterView(movement.X, movement.Y);
                    // coordinates are centered so offset bounds by half a tile
                    _game.Camera.ApplyBounds(-Tile.HALF_DIMENSION, -Tile.HALF_DIMENSION, _game.TileMap.Width * Tile.DIMENSION - Tile.HALF_DIMENSION, _game.TileMap.Height * Tile.DIMENSION - Tile.HALF_DIMENSION);

                    RebuildTileMapGraphics();
                }

            }

            finalDrawables = tileMapDrawables
                .Where(d => _game.Camera.IsInView(d.GetLeft(), d.GetTop(), d.GetWidth(), d.GetHeight()))
                .OrderBy(d => d.GetLayer())
                .ThenBy(d => d.GetZ())
                .ThenBy(d => d.GetTop())
                .ToList();
        }

        private void RebuildTileMapGraphics()
        {
            // TODO use the graphicsPipeline to render tielsets

            tileMapDrawables = new List<IDrawableInfo>();
            foreach (var layer in _game.TileMap?.Layers)
            {
                foreach (var tile in layer.Value)
                {
                    if (!_game.Camera.IsInView(tile.X * Tile.DIMENSION, tile.Y * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION))
                        continue;

                    var textureList = new List<Rectangle>();
                    textureList.Add(new Rectangle(tile.TextureCoordX * Tile.DIMENSION, tile.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION));
                    foreach (var decal in tile.Decals)
                    {
                        textureList.Add(new Rectangle(decal.TextureCoordX * Tile.DIMENSION, decal.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION));
                    }

                    tileMapDrawables.Add(new DrawableInfo
                    {
                        Sheet = _game.TileMap?.TileSet,
                        Layer = layer.Key,
                        Position = new Vector2(tile.X * Tile.DIMENSION, tile.Y * Tile.DIMENSION),
                        Width = Tile.DIMENSION,
                        Height = Tile.DIMENSION,
                        Z = tile.Z,
                        TextureRect = textureList.ToArray(),
                        Rotation = 0,
                        Origin = new Vector2(Tile.HALF_DIMENSION, Tile.HALF_DIMENSION),
                        Color = Color.White
                    });
                }
            }
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Movement), typeof(Player));
            var listener = context.CreateListener(query);

            listener.ListenToAdded = true;
            listener.ListenToChanged = true;
            listener.ListenToRemoved = true;

            return listener;
        }
    }

    public interface IDrawableInfo {
        public uint? GetEntityId();
        public float GetLeft();
        public float GetTop();
        public int GetWidth();
        public int GetHeight();
        public int GetLayer();
        public int GetZ();
    }
    public class DrawableInfo : IDrawableInfo
    {
        public uint? EntityId { get; set; }
        public string Sheet { get; set; }
        public int Layer { get; set; }
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Z { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Rectangle[] TextureRect { get; set; }
        public Color Color { get; set; }
        public bool FlipHorizontally { get; set; }
        public bool FlipVertically { get; set; }

        public uint? GetEntityId()
        {
            return EntityId;
        }

        public float GetLeft()
        {
            return Position.X;
        }

        public float GetTop()
        {
            return Position.Y;
        }

        public int GetWidth()
        {
            return Width;
        }

        public int GetHeight()
        {
            return Height;
        }

        public int GetLayer()
        {
            return Layer;
        }

        public int GetZ()
        {
            return Z;
        }
    }
    public class ComplexDrawableInfo : IDrawableInfo
    {
        public List<DrawableInfo> Drawables = new List<DrawableInfo>();

        public uint? GetEntityId()
        {
            if (Drawables.Count == 0)
                return null;

            return Drawables[0].EntityId;
        }

        public float GetLeft()
        {
            if (Drawables.Count == 0)
                return 0;

            return Drawables.Min(d => d.GetLeft());
        }

        public float GetTop()
        {
            if (Drawables.Count == 0)
                return 0;

            return Drawables.Min(d => d.GetTop());
        }

        public int GetWidth()
        {
            if (Drawables.Count == 0)
                return 0;

            return Drawables.Max(d => d.Width);
        }

        public int GetHeight()
        {
            if (Drawables.Count == 0)
                return 0;

            return Drawables.Max(d => d.Height);
        }

        public int GetLayer()
        {
            if (Drawables.Count == 0)
                return 0;

            return Drawables.Max(d => d.Layer);
        }

        public int GetZ()
        {
            if (Drawables.Count == 0)
                return 0;

            return Drawables.Max(d => d.Z);
        }
    }
}
