using Ozzyria.Client.UI;
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
        public const bool DEBUG_SHOW_RENDER_AREA = false;


        // TODO OZ-13 : have a mapping of entities and tiles to their sprites to avoid constant NEWing of sprites over and over again
        public void Render(RenderTarget worldRenderTexture, Camera camera, TileMap tileMap, int localPlayerId, Entity[] entities)
        {
            var graphicsManager = GraphicsManager.GetInstance();
            var graphics = new List<Graphic>();
            foreach (var entity in entities)
            {
                var movement = entity.GetComponent<Movement>(ComponentType.Movement);
                if (entity.HasComponent(ComponentType.Renderable))
                {
                    var renderable = entity.GetComponent<Renderable>(ComponentType.Renderable);
                    var sprite = renderable.Sprite;
                    var sfmlSprite = graphicsManager.CreateSprite(sprite);
                    sfmlSprite.Position = new Vector2f(movement.X, movement.Y);
                    sfmlSprite.Rotation = AngleHelper.RadiansToDegrees(movement.LookDirection);

                    if (entity.HasComponent(ComponentType.Stats))
                    {
                        var stats = entity.GetComponent<Stats>(ComponentType.Stats);

                        // only show health bar for entities that are not the local player
                        if (entity.Id != localPlayerId)
                        {
                            var statBar = new HoverStatBar { Layer = movement.Layer };
                            statBar.Move(movement.X, movement.Y);
                            statBar.SetMagnitude(stats.Health, stats.MaxHealth);

                            graphics.Add(statBar);
                        }
                    }

                    if (entity.HasComponent(ComponentType.Combat))
                    {
                        // show as attacking for a brief period, OZ-23 : swich this over to be an animation instead
                        var combat = entity.GetComponent<Combat>(ComponentType.Combat);
                        sfmlSprite.Color = (combat.Delay.Timer / combat.Delay.DelayInSeconds >= 0.3f) ? Color.White : Color.Red;
                    }
                    graphics.Add(new Graphic
                    {
                        Layer = movement.Layer,
                        X = sfmlSprite.Position.X - sfmlSprite.Origin.X,
                        Y = sfmlSprite.Position.Y - sfmlSprite.Origin.Y,
                        Width = sfmlSprite.TextureRect.Width,
                        Height = sfmlSprite.TextureRect.Height,
                        Z = renderable.Z,
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

                    var graphic = new Graphic
                    {
                        Layer = movement.Layer,
                        Z = Renderable.Z_DEBUG,
                    };
                    Shape shape;
                    if (collision is BoundingCircle)
                    {
                        var radius = ((BoundingCircle)collision).Radius;
                        shape = new CircleShape(radius);
                        shape.Position = new Vector2f(movement.X - radius, movement.Y - radius);

                        graphic.Width = radius * 2f;
                        graphic.Height = radius * 2f;
                    }
                    else
                    {
                        var width = ((BoundingBox)collision).Width;
                        var height = ((BoundingBox)collision).Height;
                        shape = new RectangleShape(new Vector2f(width, height));
                        shape.Position = new Vector2f(movement.X - (width / 2f), movement.Y - (height / 2f));

                        graphic.Width = width;
                        graphic.Height = height;
                    }
                    shape.FillColor = Color.Transparent;
                    shape.OutlineColor = Color.Magenta;
                    shape.OutlineThickness = 1;

                    graphic.X = shape.Position.X;
                    graphic.Y = shape.Position.Y;
                    graphic.drawables = new List<Drawable>() { shape };
                    graphics.Add(graphic);
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
                        Width = Tile.DIMENSION,
                        Height = Tile.DIMENSION,
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
            var graphicsInRenderOrder = graphics
                .Where(s => camera.IsInView(s.X, s.Y, s.Width, s.Height))
                .OrderBy(g => g.Layer)
                .ThenBy(g => g.Z)
                .ThenBy(g => g.Y);
            foreach (var graphic in graphicsInRenderOrder)
            {
                graphic.Draw(worldRenderTexture);
            }

            if (DEBUG_SHOW_RENDER_AREA)
            {
                foreach (var graphic in graphicsInRenderOrder)
                {
                    if (graphic.Z == Renderable.Z_BACKGROUND) continue; // skip rendering background to lessen noise

                    var shape = new RectangleShape(new Vector2f(graphic.Width, graphic.Height));
                    shape.Position = new Vector2f(graphic.X, graphic.Y);
                    shape.FillColor = Color.Transparent;
                    shape.OutlineColor = Color.Blue;
                    shape.OutlineThickness = 1;
                    worldRenderTexture.Draw(shape);
                }
            }
        }
    }

}
