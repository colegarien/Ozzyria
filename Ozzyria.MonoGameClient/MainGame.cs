using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ozzyria.Game;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using Ozzyria.MonoGameClient.Systems;
using Ozzyria.Networking;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Ozzyria.MonoGameClient
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _debugFont;

        public static Client _client;
        public static Camera _camera;
        public static Entity _localPlayer;
        public static TileMap _tileMap = null;
        public static WorldPersistence _worldLoader;

        private EntityContext _context;
        private SystemCoordinator _coordinator;

        private static Dictionary<string, Texture2D> textureResources;


        // for running a local server
        private const bool IS_SINGLEPLAYER = true;
        private CancellationTokenSource _cts;
        private Server _localServer;
        private Thread _localServerTheard;

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
            if (IS_SINGLEPLAYER)
            {
                _cts = new CancellationTokenSource();
                _localServer = new Server();
                _localServerTheard = new Thread(_localServer.Start);
                _localServerTheard.Start(_cts.Token);
            }

            _worldLoader = new WorldPersistence();
            _context = new EntityContext();
            _coordinator = new SystemCoordinator();
            _coordinator
                .Add(new Systems.Network())
                .Add(new Systems.LocalPlayer(_context))
                .Add(new Systems.RenderTracking(_context));

            _client = new Client();
            if (!_client.Connect("127.0.0.1", 13000))
            {
                Log("Join failed!");
                Exit();
                return;
            }
            Log($"Joined as Client #{_client.Id}");

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            _client.Disconnect();

            if (IS_SINGLEPLAYER)
            {
                // clean up local server
                _cts.Cancel();
            }

            base.OnExiting(sender, args);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugFont = Content.Load<SpriteFont>("debug_font");

            textureResources = new Dictionary<string, Texture2D>()
            {
                {"entity_set_001", Content.Load<Texture2D>("Sprites/entity_set_001")},
                {"outside_tileset_001", Content.Load<Texture2D>("Sprites/outside_tileset_001")},
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (!_client.IsConnected() || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Log($"Disconnecting");
                _client.Disconnect();
                Exit();
                return;
            }

            _coordinator.Execute((float)gameTime.ElapsedGameTime.TotalMilliseconds, _context);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ///
            /// Render Game World
            ///
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            foreach (var drawls in Systems.RenderTracking.finalDrawables)
            {
                if (drawls is DrawableInfo)
                {
                    DrawDrawableInfo((DrawableInfo)drawls);
                }
                else
                {
                    var drawlsList = ((ComplexDrawableInfo)drawls).Drawables;
                    foreach(var subDrawls in drawlsList)
                    {
                        DrawDrawableInfo(subDrawls);
                    }
                }
                
            }
            _spriteBatch.End();


            ///
            /// Render UI Overlay
            ///
            _spriteBatch.Begin();
            if (_localPlayer != null)
            {
                var localPlayerStats = (Stats)_localPlayer?.GetComponent(typeof(Stats));
                _spriteBatch.DrawString(_debugFont, $"HP: {localPlayerStats?.Health}/{localPlayerStats?.MaxHealth}\r\nEXP: {localPlayerStats?.Experience}/{localPlayerStats?.MaxExperience}", Vector2.Zero, Color.Red);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void DrawDrawableInfo(DrawableInfo drawls)
        {
            var spriteEffectFlags = (drawls.FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawls.FlipVertically ? SpriteEffects.FlipVertically : SpriteEffects.None);
            foreach (var rect in drawls.TextureRect)
            {
                _spriteBatch.Draw(textureResources[drawls.Sheet], new Rectangle((int)drawls.Position.X, (int)drawls.Position.Y, drawls.Width, drawls.Height), rect, drawls.Color, drawls.Rotation, drawls.Origin, spriteEffectFlags, 0);
            }
        }

    }
}