using Microsoft.Xna.Framework;
using Ozzyria.Game;
using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MonoGameClient.Systems
{
    // TODO OZ-23 how to do debug shapes... that's a tough one lol
    internal class RenderTracking : TriggerSystem
    {
        public static List<DrawableInfo> finalDrawables = new List<DrawableInfo>();
        public static List<DrawableInfo> tileMapDrawables = new List<DrawableInfo>();
        public static List<DrawableInfo> entityDrawables = new List<DrawableInfo>();

        public Registry ResourceRegistry;

        public RenderTracking(EntityContext context) : base(context)
        {
            ResourceRegistry = Registry.GetInstance();
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var renderable = (Renderable)entity.GetComponent(typeof(Renderable));

                if (MainGame._localPlayer != null && MainGame._localPlayer?.id == entity.id)
                {
                    MainGame._camera.CenterView(movement.X, movement.Y);
                    RebuildTileMapGraphics();
                }

                if (movement == null || renderable == null)
                {
                    entityDrawables.RemoveAll(d => d.EntityId != null && d.EntityId == entity.id);
                }
                else
                {
                    UpdateEntityDrawables(entity, movement, renderable);
                }
            }

            finalDrawables = tileMapDrawables
                .Concat(entityDrawables)
                .Where(e => MainGame._camera.IsInView(e.Position.X, e.Position.Y, e.Width, e.Height))
                .OrderBy(g => g.Layer)
                .ThenBy(g => g.Z)
                .ThenBy(g => g.Position.Y)
                .ToList();
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
                        Sheet = MainGame._tileMap?.TileSet,
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

        private void UpdateEntityDrawables(Entity entity, Movement movement, Renderable renderable)
        {
            var existingItemIndex = entityDrawables.FindIndex(0, entityDrawables.Count, e => e.EntityId != null && e.EntityId == entity.id);
            var currentClip = ResourceRegistry.Clips[renderable.CurrentClip];
            var frame = currentClip.GetFrame(renderable.CurrentFrame);
            var transform = frame.Transform;
            var source = ResourceRegistry.FrameSources[frame.SourceId];

            var drawable = new DrawableInfo
            {
                EntityId = entity.id,
                Sheet = ResourceRegistry.Resources[source.Resource],
                Layer = movement.Layer,
                Position = new Vector2(movement.X - (transform.DestinationW * 0.5f) + transform.RelativeX, movement.Y - (transform.DestinationH * 0.5f) + transform.RelativeY),
                Rotation = (transform.RelativeRotation ? -movement.LookDirection : 0) + transform.Rotation,
                Width = transform.DestinationW,
                Height = transform.DestinationH,
                Z = renderable.Z,
                Color = new Color(transform.Red, transform.Green, transform.Blue, transform.Alpha),
                TextureRect = new Rectangle[] { new Rectangle(source.Left, source.Top, source.Width, source.Height) },
                FlipHorizontally = transform.FlipHorizontally,
                FlipVertically = transform.FlipVertically,
                Origin = new Vector2((transform.DestinationW * 0.5f) + transform.OriginOffsetX, (transform.DestinationH * 0.5f) + transform.OriginOffsetY)
            };

            if (existingItemIndex != -1)
                entityDrawables[existingItemIndex] = drawable;
            else
                entityDrawables.Add(drawable);
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

    public struct DrawableInfo
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
    }
}
