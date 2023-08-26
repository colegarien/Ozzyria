using Microsoft.Xna.Framework;
using Ozzyria.Game;
using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MonoGameClient.Systems
{
    // TODO OZ-23 how to do debug shapes... that's a tough one lol
    internal class RenderTracking : TriggerSystem
    {
        public static List<IDrawableInfo> finalDrawables = new List<IDrawableInfo>();
        public static List<IDrawableInfo> tileMapDrawables = new List<IDrawableInfo>();
        public static List<IDrawableInfo> entityDrawables = new List<IDrawableInfo>();

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
                var state = (AnimationState)entity.GetComponent(typeof(AnimationState));
                var gear = (EquippedGear)entity.GetComponent(typeof(EquippedGear));

                if (entity.HasComponent(typeof(Player)) && ((Player)entity.GetComponent(typeof(Player))).PlayerId == MainGame._client.Id && movement != null)
                {
                    MainGame._camera.CenterView(movement.X, movement.Y);
                    RebuildTileMapGraphics();
                }

                if (movement == null || renderable == null)
                {
                    entityDrawables.RemoveAll(d => d.GetEntityId() != null && d.GetEntityId() == entity.id);
                }
                else
                {
                    UpdateEntityDrawables(entity, movement, renderable, state, gear);
                }
            }

            finalDrawables = tileMapDrawables
                .Concat(entityDrawables)
                .Where(d => MainGame._camera.IsInView(d.GetLeft(), d.GetTop(), d.GetWidth(), d.GetHeight()))
                .OrderBy(d => d.GetLayer())
                .ThenBy(d => d.GetZ())
                .ThenBy(d => d.GetTop())
                .ToList();
        }

        private void RebuildTileMapGraphics()
        {
            tileMapDrawables = new List<IDrawableInfo>();
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

        private void UpdateEntityDrawables(Entity entity, Movement movement, Renderable renderable, AnimationState state, EquippedGear gear)
        {
            var existingItemIndex = entityDrawables.FindIndex(0, entityDrawables.Count, e => e.GetEntityId() != null && e.GetEntityId() == entity.id);

            if (renderable.IsDynamic)
            {
                // TODO OZ-23 try using the ComplexDrawableInfo and then try the Z instead!!!
                var complexDrawable = new ComplexDrawableInfo();

                var direction = state.GetDirectionVariable("Direction");
                if (direction == "north" || direction == "west")
                {
                    if (gear.Weapon != "")
                    {
                        var clip = BuildSubClip(entity, movement, renderable, state, gear, $"generic-weapon_{state.State}_{direction}");
                        if (clip != null)
                            complexDrawable.Drawables.Add(clip);
                    }
                    if (gear.WeaponEffect != "")
                    {
                        var clip = BuildSubClip(entity, movement, renderable, state, gear, $"generic-weaponfx_{state.State}_{direction}");
                        if (clip != null)
                            complexDrawable.Drawables.Add(clip);
                    }
                }

                if (gear.Body != "")
                {
                    var clip = BuildSubClip(entity, movement, renderable, state, gear, $"{gear.Body}_{state.State}_{direction}");
                    if (clip != null)
                        complexDrawable.Drawables.Add(clip);
                }

                if (gear.Armor != "")
                {
                    var clip = BuildSubClip(entity, movement, renderable, state, gear, $"generic-armor_{state.State}_{direction}");
                    if (clip != null)
                        complexDrawable.Drawables.Add(clip);
                }

                if (gear.Mask != "")
                {
                    var clip = BuildSubClip(entity, movement, renderable, state, gear, $"generic-mask_{state.State}_{direction}");
                    if (clip != null)
                        complexDrawable.Drawables.Add(clip);
                }

                if (gear.Hat != "")
                {
                    var clip = BuildSubClip(entity, movement, renderable, state, gear, $"generic-hat_{state.State}_{direction}");
                    if (clip != null)
                        complexDrawable.Drawables.Add(clip);
                }


                if (direction == "south" || direction == "east")
                {
                    if (gear.Weapon != "")
                    {
                        var clip = BuildSubClip(entity, movement, renderable, state, gear, $"generic-weapon_{state.State}_{direction}");
                        if (clip != null)
                            complexDrawable.Drawables.Add(clip);
                    }
                    if (gear.WeaponEffect != "")
                    {
                        var clip = BuildSubClip(entity, movement, renderable, state, gear, $"generic-weaponfx_{state.State}_{direction}");
                        if (clip != null)
                            complexDrawable.Drawables.Add(clip);
                    }

                }

                if (complexDrawable.Drawables.Count > 0)
                    PushEntityDrawable(existingItemIndex, complexDrawable);
            }
            else
            {
                PushClip(entity, movement, renderable, existingItemIndex, renderable.StaticClip);
            }
        }

        private DrawableInfo BuildSubClip(Entity entity, Movement movement, Renderable renderable, AnimationState state, EquippedGear gear, string clip)
        {
            if (!ResourceRegistry.Clips.ContainsKey(clip))
                return null;

            var currentClip = ResourceRegistry.Clips[clip];
            var frame = currentClip.GetFrame(renderable.CurrentFrame);
            var transform = frame.Transform;
            var sourceId = frame.SourceId;
            
            // TODO OZ-23 make it so that "body" can have generic animation as well!! (3 frame idle, 4 frame attack, etc)
            // TODO OZ-23 decouple the name of the gear with the name of the frame-itself
            if(sourceId == "**HAT**")
                sourceId = gear.Hat + "_" + state.GetDirectionVariable("Direction");
            else if (sourceId == "**ARMOR**")
                sourceId = gear.Armor + "_" + state.GetDirectionVariable("Direction");
            else if (sourceId == "**MASK**")
                sourceId = gear.Mask + "_" + state.GetDirectionVariable("Direction");
            else if (sourceId == "**WEAPON**")
                sourceId = gear.Weapon;
            else if (sourceId == "**WEAPONFX**")
                sourceId = gear.WeaponEffect + "_" + state.GetDirectionVariable("Direction");

            if (!ResourceRegistry.FrameSources.ContainsKey(sourceId))
                return null;

            var source = ResourceRegistry.FrameSources[sourceId]; // TODO OZ-23 cross-reference item-id with sources

            return BuildDrawable(entity.id, movement.Layer, renderable.Z, movement.X, movement.Y, movement.LookDirection, transform, source);
        }

        private void PushClip(Entity entity, Movement movement, Renderable renderable, int itemIndex, string clip)
        {
            if (!ResourceRegistry.Clips.ContainsKey(clip))
                return;

            var currentClip = ResourceRegistry.Clips[clip];
            var frame = currentClip.GetFrame(renderable.CurrentFrame);
            var transform = frame.Transform;
            var source = ResourceRegistry.FrameSources[frame.SourceId];

            var drawable = BuildDrawable(entity.id, movement.Layer, renderable.Z, movement.X, movement.Y, movement.LookDirection, transform, source);
            PushEntityDrawable(itemIndex, drawable);
        }

        private DrawableInfo BuildDrawable(uint entityId, int layer, int z, float x, float y, float rotation, FrameTransform transform, FrameSource source)
        {
            return new DrawableInfo
            {
                EntityId = entityId,
                Sheet = ResourceRegistry.Resources[source.Resource],
                Layer = layer,
                Position = new Vector2(x - (transform.DestinationW * 0.5f) + transform.RelativeX, y - (transform.DestinationH * 0.5f) + transform.RelativeY),
                Rotation = (transform.RelativeRotation ? -rotation : 0) + transform.Rotation,
                Width = transform.DestinationW,
                Height = transform.DestinationH,
                Z = z,
                Color = new Color(transform.Red, transform.Green, transform.Blue, transform.Alpha),
                TextureRect = new Rectangle[] { new Rectangle(source.Left, source.Top, source.Width, source.Height) },
                FlipHorizontally = transform.FlipHorizontally,
                FlipVertically = transform.FlipVertically,
                Origin = new Vector2((transform.DestinationW * 0.5f) + transform.OriginOffsetX, (transform.DestinationH * 0.5f) + transform.OriginOffsetY)
            };
        }

        private void PushEntityDrawable(int index, IDrawableInfo drawable)
        {
            if (index > -1 && entityDrawables.Count > index)
                entityDrawables[index] = drawable;
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
