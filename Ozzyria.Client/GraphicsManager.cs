﻿using Ozzyria.Client.Graphics;
using Ozzyria.Game;
using Ozzyria.Game.Components;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace Ozzyria.Client
{
    public class GraphicsManager
    {
        public const int MINIMUM_LAYER = 0;
        public const int MAXIMUM_LAYER = 255;

        private static GraphicsManager _manager;
        private IDictionary<string, Texture> _loadedTextures;

        public static GraphicsManager GetInstance()
        {
            if (_manager == null)
            {
                _manager = new GraphicsManager();
            }

            return _manager;
        }

        private GraphicsManager()
        {
            _loadedTextures = new Dictionary<string, Texture>();
        }

        public Texture GetTexture(string texture)
        {
            if (!_loadedTextures.ContainsKey(texture))
            {
                _loadedTextures[texture] = new Texture(texture);
            }

            return _loadedTextures[texture];
        }

        public Sprite CreateSprite(SpriteType spriteType)
        {
            var sfmlSprite = new Sprite(GetTexture(Content.Loader.Root() + "/Entities/Sprites/entity_set_001.png"));
            switch (spriteType)
            {
                case SpriteType.Particle:
                    sfmlSprite.Origin = new Vector2f(16, 16);
                    sfmlSprite.TextureRect = new IntRect(0, 96, 32, 32);
                    sfmlSprite.Color = Color.Yellow;
                    break;
                case SpriteType.Player:
                    sfmlSprite.Origin = new Vector2f(16, 16);
                    sfmlSprite.TextureRect = new IntRect(0, 32, 32, 32);
                    sfmlSprite.Color = Color.White;
                    break;
                case SpriteType.Slime:
                default:
                    sfmlSprite.Origin = new Vector2f(16, 16);
                    sfmlSprite.TextureRect = new IntRect(0, 0, 32, 32);
                    sfmlSprite.Color = Color.White;
                    break;
            }

            return sfmlSprite;
        }

        public IGraphic CreateTileGraphic(string tileSet, int layer, Tile tile)
        {
            var sprites = new List<Drawable>();
            var baseSprite = new Sprite(GetTexture(Content.Loader.Root() + "/TileSets/Sprites/" + tileSet + ".png"))
            {
                Position = new Vector2f(tile.X * Tile.DIMENSION, tile.Y * Tile.DIMENSION),
                TextureRect = new IntRect(tile.TextureCoordX * Tile.DIMENSION, tile.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION)
            };

            sprites.Add(baseSprite);
            foreach (var decal in tile.Decals)
            {
                sprites.Add(new Sprite(GetTexture(Content.Loader.Root() + "/TileSets/Sprites/" + tileSet + ".png"))
                {
                    Position = new Vector2f(tile.X * Tile.DIMENSION, tile.Y * Tile.DIMENSION),
                    TextureRect = new IntRect(decal.TextureCoordX * Tile.DIMENSION, decal.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION)
                });
            }

            return new CompositeGraphic
            {
                Layer = layer,
                X = baseSprite.Position.X,
                Y = baseSprite.Position.Y,
                Width = Tile.DIMENSION,
                Height = Tile.DIMENSION,
                Z = tile.Z,
                drawables = sprites
            };
        }

    }
}
