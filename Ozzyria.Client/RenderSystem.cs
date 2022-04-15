using Ozzyria.Client.Graphics;
using Ozzyria.Client.Graphics.DebugShape;
using Ozzyria.Client.Graphics.UI;
using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.Game.ECS;
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
        public const bool DEBUG_SHOW_RENDER_AREA = false;

        private List<IGraphic> cachedTileMapGraphics;
        private EntityQuery query;

        public RenderSystem()
        {
            query = new EntityQuery();
            query.And(typeof(Movement), typeof(Renderable));
        }

        // TODO OZ-15 rework this a bit so that graphics don't have to constantly be re-instantiaed, possibly tracking by Entities Ids or Tile Maps
        public void Render(RenderTarget target, Camera camera, TileMap tileMap, EntityContext context, uint playerEntityId)
        {
            var graphicsManager = GraphicsManager.GetInstance();
            var graphics = new List<IGraphic>();

            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                if (entity.HasComponent(typeof(Renderable)))
                {
                    if (entity.HasComponent(typeof(Stats)))
                    {
                        // Show Health Bar for Entities that are not the local player
                        var stats = (Stats)entity.GetComponent(typeof(Stats));
                        if (entity.id != playerEntityId)
                        {
                            graphics.Add(new HoverStatBar(movement.Layer, movement.X, movement.Y, stats.Health, stats.MaxHealth));
                        }
                    }

                    var renderable = (Renderable)entity.GetComponent(typeof(Renderable));
                    var sfmlSprite = graphicsManager.CreateSprite(renderable.Sprite);
                    sfmlSprite.Position = new Vector2f(movement.X, movement.Y);
                    sfmlSprite.Rotation = AngleHelper.RadiansToDegrees(movement.LookDirection);

                    // OZ-23 : swich this over to be an animation instead
                    if (entity.HasComponent(typeof(Combat)))
                    {
                        // show as attacking for a brief period
                        var combat = (Combat)entity.GetComponent(typeof(Combat));
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
                    if (entity.id == playerEntityId)
                    {
                        camera.CenterView(movement.X, movement.Y);
                    }
                }

                if (DEBUG_SHOW_COLLISIONS && (entity.HasComponent(typeof(BoundingBox)) || entity.HasComponent(typeof(BoundingCircle))))
                {
                    var collision = (Collision)(entity.GetComponent(typeof(BoundingBox)) ?? entity.GetComponent(typeof(BoundingCircle)));
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
                    if (graphic.GetZOrder() == (int)ZLayer.Background) continue; // skip rendering background to lessen noise

                    var debugGraphic = new DebugRenderArea(graphic);
                    debugGraphic.Draw(target);
                }
            }
        }
    }

}
