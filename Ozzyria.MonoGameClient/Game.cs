using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ozzyria.Game;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using Ozzyria.Networking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MonoGameClient
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Client _client;
        private EntityContext _context;
        private TileMap _tileMap = null;
        private WorldPersistence _worldLoader;

        private Texture2D entitySheet;
        private Texture2D tileSheet;

        private Camera _camera;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        protected override void Initialize()
        {
            _worldLoader = new WorldPersistence();
            _context = new EntityContext();
            _client = new Client();
            if (!_client.Connect("127.0.0.1", 13000))
            {
                Log("Join failed!");
                Exit();
                return;
            }
            Log($"Join as Client #{_client.Id}");

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            entitySheet = Content.Load<Texture2D>("Sprites/entity_set_001");
            tileSheet = Content.Load<Texture2D>("Sprites/outside_tileset_001");
        }

        protected override void Update(GameTime gameTime)
        {
            ///
            /// EVENT HANDLING HERE
            ///
            if (!_client.IsConnected() || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Log($"Disconnecting");
                _client.Disconnect();
                Exit();
            }

            var input = new Input
            {
                MoveUp = Keyboard.GetState().IsKeyDown(Keys.W),
                MoveDown = Keyboard.GetState().IsKeyDown(Keys.S),
                MoveLeft = Keyboard.GetState().IsKeyDown(Keys.A),
                MoveRight = Keyboard.GetState().IsKeyDown(Keys.D),
                TurnLeft = Keyboard.GetState().IsKeyDown(Keys.Q),
                TurnRight = Keyboard.GetState().IsKeyDown(Keys.E),
                Attack = Keyboard.GetState().IsKeyDown(Keys.Space)
            };

            ///
            /// Do Updates
            ///
            _client.SendInput(input);
            _client.HandleIncomingMessages(_context);

            var localPlayer = _context.GetEntities(new EntityQuery().And(typeof(Player))).FirstOrDefault(e => ((Player)e.GetComponent(typeof(Player))).PlayerId == _client.Id);
            var playerEntityId = localPlayer?.id ?? 0;
            var playerEntityMap = ((Player)localPlayer?.GetComponent(typeof(Player)))?.Map ?? "";

            if ((_tileMap == null || playerEntityMap != _tileMap?.Name) && playerEntityMap != "")
            {
                _tileMap = _worldLoader.LoadMap(playerEntityMap);
            }

            var playerMovement = (Movement)localPlayer.GetComponent(typeof(Movement));
            _camera.CenterView(playerMovement.X, playerMovement.Y);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());

            var query = new EntityQuery();
            query.And(typeof(Movement), typeof(Renderable));
            var entities = _context.GetEntities(query);

            var listDrawables = new List<DrawableInfo>();
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var renderable = (Renderable)entity.GetComponent(typeof(Renderable));

                if (!_camera.IsInView(movement.X - Tile.HALF_DIMENSION, movement.Y - Tile.HALF_DIMENSION, Tile.DIMENSION, Tile.DIMENSION))
                    continue;

                listDrawables.Add(new DrawableInfo
                {
                    Sheet = entitySheet,
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

            foreach (var layer in _tileMap.Layers)
            {
                foreach (var tile in layer.Value)
                {
                    if (!_camera.IsInView(tile.X * Tile.DIMENSION, tile.Y * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION))
                        continue;

                    var textureList = new List<Rectangle>();
                    textureList.Add(new Rectangle(tile.TextureCoordX * Tile.DIMENSION, tile.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION));
                    foreach(var decal in tile.Decals)
                    {
                        textureList.Add(new Rectangle(decal.TextureCoordX * Tile.DIMENSION, decal.TextureCoordY * Tile.DIMENSION, Tile.DIMENSION, Tile.DIMENSION));
                    }

                    listDrawables.Add(new DrawableInfo
                    {
                        Sheet = tileSheet,
                        Layer = layer.Key,
                        Position = new Vector2(tile.X * Tile.DIMENSION, tile.Y * Tile.DIMENSION),
                        Width = Tile.DIMENSION,
                        Height = Tile.DIMENSION,
                        Z = tile.Z,
                        TextureRect = textureList.ToArray()
                    });
                }
            }

            foreach (var drawls in listDrawables.OrderBy(g => g.Layer).ThenBy(g => g.Z).ThenBy(g => g.Position.Y))
            {
                var finalRotation = drawls.Rotation; //(transform.RelativeRotation ? (MathHelper.PiOver2 * drawls.Rotation) : 0f) + transform.Rotation;
                var spriteEffectFlags = SpriteEffects.None;//(transform.FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (transform.FlipVertically ? SpriteEffects.FlipVertically : SpriteEffects.None);

                foreach (var rect in drawls.TextureRect)
                {
                    _spriteBatch.Draw(drawls.Sheet, new Rectangle((int)drawls.Position.X, (int)drawls.Position.Y, drawls.Width, drawls.Height), rect, drawls.Color, finalRotation, drawls.Origin, spriteEffectFlags, 0);
                }
                
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        protected Rectangle GetEntitySpriteRect(SpriteType type)
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

        public class DrawableInfo
        {
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
}