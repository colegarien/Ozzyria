using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.Game.Utility;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Client
{
    class RenderSystem
    {
        public const bool DEBUG_SHOW_COLLISIONS = true;

        // OZ-13 : have a mapping of entities and tiles to their sprites to avoid constant NEWing of sprites over and over again

        // OZ-13 : actually use Game specific stuff, maybe even do ALL the rendering here
        public void Render(RenderTarget worldRenderTexture, Camera camera, TileMap tileMap, int localPlayerId, Entity[] entities)
        {
            var graphicsManager = GraphicsManager.GetInstance();
            var graphics = new List<Graphic>();
            foreach (var entity in entities)
            {
                var movement = entity.GetComponent<Movement>(ComponentType.Movement);
                if (entity.HasComponent(ComponentType.Renderable))
                {
                    var sprite = entity.GetComponent<Renderable>(ComponentType.Renderable).Sprite;
                    var sfmlSprite = graphicsManager.CreateSprite(sprite);
                    sfmlSprite.Position = graphicsManager.CreateSpritePosition(movement.X, movement.Y);
                    sfmlSprite.Rotation = AngleHelper.RadiansToDegrees(movement.LookDirection);

                    if (entity.HasComponent(ComponentType.Stats))
                    {
                        var stats = entity.GetComponent<Stats>(ComponentType.Stats);

                        // only show health bar for entities that are not the local player
                        if (entity.Id != localPlayerId)
                        {
                            var offset = new Vector2f(0, 14);

                            var background = new RectangleShape(new Vector2f(26, 5));
                            background.Position = new Vector2f(movement.X, movement.Y);
                            background.Origin = new Vector2f(background.Size.X / 2 + offset.X, background.Size.Y + offset.Y);
                            background.FillColor = Color.Red;

                            var overlay = new RectangleShape(background.Size);
                            overlay.Position = new Vector2f(movement.X, movement.Y);
                            overlay.Size = new Vector2f(((float)stats.Health / (float)stats.MaxHealth) * background.Size.X, overlay.Size.Y);
                            overlay.Origin = background.Origin;
                            overlay.FillColor = Color.Green;

                            graphics.Add(new Graphic
                            {
                                Layer = 1, // TODO OZ-13 : make entities on a layer?
                                X = background.Position.X,
                                Y = background.Position.Y,
                                Z = 10, // TODO OZ-13 : grab from entity... movement?
                                drawables = new List<Drawable>() {
                                    background,
                                    overlay
                                }
                            });
                        }
                    }

                    if (entity.HasComponent(ComponentType.Combat))
                    {
                        // show as attacking for a brief period
                        var combat = entity.GetComponent<Combat>(ComponentType.Combat);
                        sfmlSprite.Color = (combat.Delay.Timer / combat.Delay.DelayInSeconds >= 0.3f) ? Color.White : Color.Red;
                    }
                    graphics.Add(new Graphic
                    {
                        Layer = 1, // TODO OZ-13 : make entities on a layer?
                        X = sfmlSprite.Position.X,
                        Y = sfmlSprite.Position.Y,
                        Z = 5, // TODO OZ-13 : grab from entity... movement?
                        drawables = new List<Drawable>() { sfmlSprite }
                    });

                    // center camera on entity
                    if (entity.Id == localPlayerId)
                    {
                        camera.CenterView(movement.X, movement.Y);
                    }
                }

                if (DEBUG_SHOW_COLLISIONS && entity.HasComponent(ComponentType.Collision))
                {
                    var collision = entity.GetComponent<Collision>(ComponentType.Collision);

                    Shape shape;
                    if (collision is BoundingCircle)
                    {
                        var radius = ((BoundingCircle)collision).Radius;
                        shape = new CircleShape(radius);
                        shape.Position = new Vector2f(movement.X - radius, movement.Y - radius);
                    }
                    else
                    {
                        var width = ((BoundingBox)collision).Width;
                        var height = ((BoundingBox)collision).Height;
                        shape = new RectangleShape(new Vector2f(width, height));
                        shape.Position = new Vector2f(movement.X - (width / 2f), movement.Y - (height / 2f));
                    }
                    shape.FillColor = Color.Transparent;
                    shape.OutlineColor = Color.Magenta;
                    shape.OutlineThickness = 1;

                    graphics.Add(new Graphic
                    {
                        Layer = 1, // for der lols
                        X = shape.Position.X,
                        Y = shape.Position.Y,
                        Z = 10000, // show on top
                        drawables = new List<Drawable>() {
                            shape
                        }
                    });
                }
            }
            foreach (var layer in tileMap.Layers)
            {
                foreach (var tile in layer.Value)
                {
                    var sprite = graphicsManager.CreateTileSprite(tile);
                    graphics.Add(new Graphic
                    {
                        Layer = layer.Key,
                        X = sprite.Position.X,
                        Y = sprite.Position.Y,
                        Z = tile.Z,
                        drawables = new List<Drawable>() {
                            sprite
                        }
                    });
                }
            }

            RenderSprites(worldRenderTexture, camera, graphics.ToArray());
        }

        private void RenderSprites(RenderTarget worldRenderTexture, Camera camera, Graphic[] graphics)
        {
            var graphicsInLayer = graphics
                .Where(s => camera.IsInView(s.X, s.Y))
                .OrderBy(g => g.Layer)
                .ThenBy(g => g.Z)
                .ThenBy(g => g.Y);
            foreach (var graphic in graphicsInLayer)
            {
                graphic.Draw(worldRenderTexture);
            }
        }
    }


    // OZ-13 : convert these to special "Graphic" subclasses
    class UIProgressBar
    {
        private const int NUM_SEGMENTS = 10;
        private RectangleShape[] segments = new RectangleShape[NUM_SEGMENTS];
        private Color background;
        private Color foreground;

        public UIProgressBar(float X, float Y, Color backgroundColor, Color foregroundColor)
        {
            background = backgroundColor;
            foreground = foregroundColor;
            for (var segment = 0; segment < NUM_SEGMENTS; segment++)
            {
                segments[segment] = new RectangleShape()
                {
                    Position = new Vector2f(X + (22 * segment), Y),
                    Size = new Vector2f(20, 10),
                };
            }
        }

        public void SetMagnitude(int current, int max)
        {
            var fillToSegment = Math.Round((float)(current) / (float)(max) * NUM_SEGMENTS);
            for (var segment = 0; segment < NUM_SEGMENTS; segment++)
            {
                var fillSegment = segment < fillToSegment;
                segments[segment].FillColor = fillSegment ? foreground : background;
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (var segment in segments)
            {
                window.Draw(segment);
            }
        }
    }
    class HoverStatBar
    {
        private RectangleShape background;
        private RectangleShape overlay;

        public HoverStatBar()
        {
            var offset = new Vector2f(0, 14);

            background = new RectangleShape(new Vector2f(26, 5));
            background.Origin = new Vector2f(background.Size.X / 2 + offset.X, background.Size.Y + offset.Y);
            background.FillColor = Color.Red;

            overlay = new RectangleShape(background.Size);
            overlay.Origin = background.Origin;
            overlay.FillColor = Color.Green;
        }

        public void Move(float X, float Y)
        {
            background.Position = GraphicsManager.GetInstance().CreateSpritePosition(X, Y);
            overlay.Position = GraphicsManager.GetInstance().CreateSpritePosition(X, Y);
        }

        public void SetMagnitude(int current, int max)
        {
            overlay.Size = new Vector2f(((float)current / (float)max) * background.Size.X, overlay.Size.Y);
        }

        public virtual void Draw(RenderTarget window)
        {
            window.Draw(background);
            window.Draw(overlay);
        }
    }
}
