using Ozzyria.Game;
using Ozzyria.Game.Component;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace Ozzyria.Client
{
    public class GraphicsManager
    {
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
            var sfmlSprite = new Sprite(GetTexture("Resources/Sprites/entity_set_001.png"));
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

        public Vertex[] CreateVertexArray(TileMap tileMap)
        {
            var tileSetVertices = new List<Vertex>();
            var tileSize = Tile.DIMENSION;
            foreach(var tile in tileMap.backgroundTiles) {
                tileSetVertices.Add(new Vertex
                {
                    Position = new Vector2f(tile.X * tileSize, tile.Y * tileSize),
                    TexCoords = new Vector2f(tile.TextureCoordX * tileSize, tile.TextureCoordY * tileSize),
                    Color = Color.White
                });
                tileSetVertices.Add(new Vertex
                {
                    Position = new Vector2f(tile.X * tileSize + tileSize, tile.Y * tileSize),
                    TexCoords = new Vector2f(tile.TextureCoordX * tileSize + tileSize, tile.TextureCoordY * tileSize),
                    Color = Color.White
                });
                tileSetVertices.Add(new Vertex
                {
                    Position = new Vector2f(tile.X * tileSize + tileSize, tile.Y * tileSize + tileSize),
                    TexCoords = new Vector2f(tile.TextureCoordX * tileSize + tileSize, tile.TextureCoordY * tileSize + tileSize),
                    Color = Color.White
                });
                tileSetVertices.Add(new Vertex
                {
                    Position = new Vector2f(tile.X * tileSize, tile.Y * tileSize + tileSize),
                    TexCoords = new Vector2f(tile.TextureCoordX * tileSize, tile.TextureCoordY * tileSize + tileSize),
                    Color = Color.White
                });
            }

            // TODO need to layer middle ground with sprites instead
            foreach (var tile2 in tileMap.middlegroundTiles)
            {
                tileSetVertices.Add(new Vertex
                {
                    Position = new Vector2f(tile2.X * tileSize, tile2.Y * tileSize),
                    TexCoords = new Vector2f(tile2.TextureCoordX * tileSize, tile2.TextureCoordY * tileSize),
                    Color = Color.White
                });
                tileSetVertices.Add(new Vertex
                {
                    Position = new Vector2f(tile2.X * tileSize + tileSize, tile2.Y * tileSize),
                    TexCoords = new Vector2f(tile2.TextureCoordX * tileSize + tileSize, tile2.TextureCoordY * tileSize),
                    Color = Color.White
                });
                tileSetVertices.Add(new Vertex
                {
                    Position = new Vector2f(tile2.X * tileSize + tileSize, tile2.Y * tileSize + tileSize),
                    TexCoords = new Vector2f(tile2.TextureCoordX * tileSize + tileSize, tile2.TextureCoordY * tileSize + tileSize),
                    Color = Color.White
                });
                tileSetVertices.Add(new Vertex
                {
                    Position = new Vector2f(tile2.X * tileSize, tile2.Y * tileSize + tileSize),
                    TexCoords = new Vector2f(tile2.TextureCoordX * tileSize, tile2.TextureCoordY * tileSize + tileSize),
                    Color = Color.White
                });
            }
            return tileSetVertices.ToArray();
        }

    }
}
