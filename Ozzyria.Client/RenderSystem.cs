using Ozzyria.Client.Graphics;
using Ozzyria.Client.Graphics.DebugShape;
using Ozzyria.Client.Graphics.UI;
using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.Game.Utility;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Client
{
    class RenderSystem
    {
        public const bool DEBUG_SHOW_COLLISIONS = true;
        public const bool DEBUG_SHOW_RENDER_AREA = true;

        private List<IGraphic> cachedTileMapGraphics;

        // TODO OZ-15 rework this a bit so that graphics don't have to constantly be re-instantiaed, possibly tracking by Entities Ids or Tile Maps
        public void Render(RenderTarget target, Camera camera, TileMap tileMap, int localPlayerId, Entity[] entities)
        {
            var graphicsManager = GraphicsManager.GetInstance();
            var graphics = new List<IGraphic>();
            foreach (var entity in entities)
            {
                var movement = entity.GetComponent<Movement>(ComponentType.Movement);
                if (entity.HasComponent(ComponentType.Renderable))
                {
                    if (entity.HasComponent(ComponentType.Stats))
                    {
                        // Show Health Bar for Entities that are not the local player
                        var stats = entity.GetComponent<Stats>(ComponentType.Stats);
                        if (entity.Id != localPlayerId)
                        {
                            graphics.Add(new HoverStatBar(movement.Layer, movement.X, movement.Y, stats.Health, stats.MaxHealth));
                        }
                    }

                    var renderable = entity.GetComponent<Renderable>(ComponentType.Renderable);
                    var sfmlSprite = graphicsManager.CreateSprite(renderable.Sprite);
                    sfmlSprite.Position = new Vector2f(movement.X, movement.Y);
                    sfmlSprite.Rotation = AngleHelper.RadiansToDegrees(movement.LookDirection);

                    // OZ-23 : swich this over to be an animation instead
                    if (entity.HasComponent(ComponentType.Combat))
                    {
                        // show as attacking for a brief period
                        var combat = entity.GetComponent<Combat>(ComponentType.Combat);
                        sfmlSprite.Color = (combat.Delay.Timer / combat.Delay.DelayInSeconds >= 0.3f) ? Color.White : Color.Red;
                    }

                    graphics.Add(new CompositeGraphic
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
                    graphics.Add(new DebugCollision(movement, collision));
                }
            }

            if (cachedTileMapGraphics == null)
            {
                // To avoid building static graphics more than once
                cachedTileMapGraphics = new List<IGraphic>();
                foreach (var layer in tileMap.Layers)
                {
                    foreach (var tile in layer.Value)
                    {
                        cachedTileMapGraphics.Add(graphicsManager.CreateTileGraphic(tileMap.TileSet, layer.Key, tile));
                    }
                }
            }

            graphics.AddRange(cachedTileMapGraphics);
            RenderGraphics(target, camera, graphics.ToArray());
        }

        private void RenderGraphics(RenderTarget target, Camera camera, IGraphic[] graphics)
        {
            var graphicsInRenderOrder = graphics
                .Where(s => camera.IsInView(s.GetLeft(), s.GetTop(), s.GetWidth(), s.GetHeight()))
                .OrderBy(g => g.GetLayer())
                .ThenBy(g => g.GetZOrder())
                .ThenBy(g => g.GetTop());
            foreach (var graphic in graphicsInRenderOrder)
            {
                graphic.Draw(target);
            }

            if (DEBUG_SHOW_RENDER_AREA)
            {
                foreach (var graphic in graphicsInRenderOrder)
                {
                    if (graphic.GetZOrder() == Renderable.Z_BACKGROUND) continue; // skip rendering background to lessen noise

                    var debugGraphic = new DebugRenderArea(graphic);
                    debugGraphic.Draw(target);
                }
            }
        }
    }

}
