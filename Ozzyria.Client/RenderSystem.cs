using Ozzyria.Game;
using SFML.Graphics;
using SFML.System;
using System;
using System.Linq;

namespace Ozzyria.Client
{
    class RenderSystem
    {
        public const bool DEBUG_SHOW_COLLISIONS = true;

        // OZ-13 : have a mapping of entities and tiles to their sprites to avoid constant NEWing of sprites over and over again

        // OZ-13 : actually use Game specific stuff, maybe even do ALL the rendering here
        public void Render(RenderTarget worldRenderTexture, Camera camera, TileMap tileMap, Sprite[] sprites, HoverStatBar[] hoverStatBars, DebugCollisionShape[] collisionShapes)
        {
            var graphicsManager = GraphicsManager.GetInstance();
            for (var layer = GraphicsManager.MINIMUM_LAYER; layer <= GraphicsManager.MAXIMUM_LAYER; layer++)
            {
                // OZ-13 refactor this so it grabs everything within the renderable space then sorts by layer & Z (and maybe Y for things on the same Z)

                // Render strip by strip from bottom of screen to top
                for (var y = tileMap.Height - 1; y >= 0; y--)
                {
                    if (layer == 1) // TODO OZ-13 allow sprites to be on any layer + colliders on different layers
                    {
                        // entities in the world
                        var spritesInLayer = sprites.Where(s => s.Position.Y >= y * Tile.DIMENSION && s.Position.Y < (y + 1) * Tile.DIMENSION && camera.IsInView(s.Position.X, s.Position.Y));
                        foreach (var sprite in spritesInLayer)
                        {
                            worldRenderTexture.Draw(sprite);
                        }

                        // in-world ui
                        foreach (var hoverStatBar in hoverStatBars)
                        {
                            hoverStatBar.Draw(worldRenderTexture);
                        }

                        if (DEBUG_SHOW_COLLISIONS)
                        {
                            // debug colissions boxes
                            foreach (var collisionShape in collisionShapes)
                            {
                                collisionShape.Draw(worldRenderTexture);
                            }
                        }
                    }

                    if (tileMap.HasLayer(layer))
                    {
                        var tiles = tileMap.GetTiles(layer).Where(t => t.Y == y && camera.IsInView(t.X * Tile.DIMENSION, t.Y * Tile.DIMENSION));
                        foreach (var tile in tiles)
                        {
                            var sprite = graphicsManager.CreateTileSprite(tile);
                            worldRenderTexture.Draw(sprite);
                        }
                    }
                }
            }
        }
    }


    // OZ-13 : get rid of all of this non-sense white noise (or at-least pack it into folders and their own classes
    class DebugCollisionShape
    {
        private Shape shape;
        public DebugCollisionShape(Shape shape)
        {
            this.shape = shape;
            shape.FillColor = Color.Transparent;
            shape.OutlineColor = Color.Magenta;
            shape.OutlineThickness = 1;
        }

        public void Draw(RenderTarget window)
        {
            window.Draw(shape);
        }
    }

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
