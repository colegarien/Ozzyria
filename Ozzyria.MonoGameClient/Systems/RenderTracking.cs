using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ozzyria.Game;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MonoGameClient.Systems
{

    internal class RenderTracking : TriggerSystem
    {
        public static List<DrawableInfo> finalDrawables = new List<DrawableInfo>();
        public static List<DrawableInfo> tileMapDrawables = new List<DrawableInfo>();
        public static List<DrawableInfo> entityDrawables = new List<DrawableInfo>();

        public RenderTracking(EntityContext context) : base(context)
        {
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            var hadChange = false;
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var renderable = (Renderable)entity.GetComponent(typeof(Renderable));

                if (MainGame._localPlayer != null && MainGame._localPlayer?.id == entity.id)
                {
                    MainGame._camera.CenterView(movement.X, movement.Y);
                    hadChange = true;

                    RebuildTileMapGraphics();
                }


                entityDrawables.RemoveAll(d => d.Entity != null && d.Entity?.id == entity.id); // TODO OZ-23 don't do this, just remove/add/update when necessary
                if (movement == null || renderable == null || !MainGame._camera.IsInView(movement.X - Tile.HALF_DIMENSION, movement.Y - Tile.HALF_DIMENSION, Tile.DIMENSION, Tile.DIMENSION))
                {
                    hadChange = true;

                    // TODO OZ-23 Remove from "renderables" list or skip it altogether
                }
                else
                {
                    hadChange = true;

                    // TODO OZ-23 add or update to "drawables" list
                    AddEntityDrawables(entity, movement, renderable);
                }
            }

            if (hadChange)
            {
                finalDrawables = tileMapDrawables
                    .Concat(entityDrawables)
                    .OrderBy(g => g.Layer)
                    .ThenBy(g => g.Z)
                    .ThenBy(g => g.Position.Y)
                    .ToList();
            }
        }

        private void RebuildTileMapGraphics()
        {
            tileMapDrawables = new List<DrawableInfo>();
            foreach (var layer in MainGame._tileMap?.Layers)
            {
                foreach (var tile in layer.Value)
                {
                    if (!MainGame._camera.IsInView(tile.X * Tile.DIMENSION, tile.Y * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION))
                        continue;

                    var textureList = new List<Rectangle>();
                    textureList.Add(new Rectangle(tile.TextureCoordX * Tile.DIMENSION, tile.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION));
                    foreach (var decal in tile.Decals)
                    {
                        textureList.Add(new Rectangle(decal.TextureCoordX * Tile.DIMENSION, decal.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION));
                    }

                    tileMapDrawables.Add(new DrawableInfo
                    {
                        Sheet = MainGame.tileSheet,
                        Layer = layer.Key,
                        Position = new Vector2(tile.X * Tile.DIMENSION, tile.Y * Tile.DIMENSION),
                        Width = Tile.DIMENSION,
                        Height = Tile.DIMENSION,
                        Z = tile.Z,
                        TextureRect = textureList.ToArray()
                    });
                }
            }
        }

        private void AddEntityDrawables(Entity entity, Movement movement, Renderable renderable)
        {
            entityDrawables.Add(new DrawableInfo
            {
                Entity = entity,
                Sheet = MainGame.entitySheet,
                Layer = movement.Layer,
                Position = new Vector2(movement.X - Tile.HALF_DIMENSION, movement.Y - Tile.HALF_DIMENSION),
                Rotation = -movement.LookDirection,
                Width = Tile.DIMENSION,
                Height = Tile.DIMENSION,
                Z = renderable.Z,
                Color = renderable.Sprite == SpriteType.Particle ? Color.Yellow : Color.White,
                TextureRect = new Rectangle[] { GetEntitySpriteRect(renderable.Sprite) }
            });           
        }


        private Rectangle GetEntitySpriteRect(SpriteType type)
        {
            switch (type)
            {
                case SpriteType.Particle:
                    return new Rectangle(0, 96, 32, 32);
                case SpriteType.Player:
                    return new Rectangle(0, 32, 32, 32);
                case SpriteType.Slime:
                default:
                    return new Rectangle(0, 0, 32, 32);
            }
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Movement), typeof(Renderable));
            var listener = context.CreateListener(query);

            listener.ListenToAdded = true;
            listener.ListenToChanged = true;
            listener.ListenToRemoved = true;

            return listener;
        }
    }

    public class DrawableInfo
    {
        public Entity Entity { get; set; }
        public Texture2D Sheet { get; set; }
        public int Layer { get; set; }
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Z { get; set; }
        public float Rotation { get; set; } = 0f;
        public Vector2 Origin { get; set; } = new Vector2(16, 16);
        public Rectangle[] TextureRect { get; set; }
        public Color Color { get; set; } = Color.White;
    }
}
