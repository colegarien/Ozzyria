using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ozzyria.Game;
using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using Ozzyria.MonoGameClient.Systems;
using Ozzyria.MonoGameClient.UI;
using Ozzyria.MonoGameClient.UI.Windows;
using Ozzyria.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ozzyria.MonoGameClient
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _greyFont;

        private EntityContext _context;
        private SystemCoordinator _coordinator;

        // "global" variables used by systems
        internal Dictionary<string, Texture2D> TextureResources;
        internal Client Client;
        internal Camera Camera;
        internal TileMap TileMap = null;
        internal WorldPersistence WorldLoader;
        internal LocalState LocalState;

        // for running a local server
        private const bool IS_SINGLEPLAYER = true;
        private CancellationTokenSource _cts;
        private Server _localServer;
        private Thread _localServerTheard;

        private Texture2D _uiTexture;
        private WindowManager _uiManager;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public static void Log(string message)
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

            LocalState = new LocalState();
            WorldLoader = new WorldPersistence();
            _context = new EntityContext();
            _coordinator = new SystemCoordinator();
            _coordinator
                .Add(new Systems.Network(this))
                .Add(new Systems.LocalPlayer(this, _context))
                .Add(new Systems.RenderTracking(this, _context))
                .Add(new Systems.LocalStateTracking(this, _context));

            Client = new Client();
            if (!Client.Connect("127.0.0.1", 13000))
            {
                Log("Join failed!");
                Exit();
                return;
            }
            Log($"Joined as Client #{Client.Id}");

            IsMouseVisible = false;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            Camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Client.Disconnect();

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
            _greyFont = Content.Load<SpriteFont>("sf_greyfont");

            TextureResources = new Dictionary<string, Texture2D>()
            {
                {"entity_set_001", Content.Load<Texture2D>("Sprites/entity_set_001")},
                {"outside_tileset_001", Content.Load<Texture2D>("Sprites/outside_tileset_001")},
            };

            _uiTexture = Content.Load<Texture2D>("ui_components");
            _uiManager = new WindowManager(new InputTracker(Camera));
            _uiManager.AddWindow(new InventoryWindow(this, _uiTexture, _greyFont)
            {
                IsVisible = false,
                X = 140,
                Y = 30,
            });
        }

        protected override void Update(GameTime gameTime)
        {
            if (!Client.IsConnected() || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Log($"Disconnecting");
                Client.Disconnect();
                Exit();
                return;
            }

            _coordinator.Execute((float)gameTime.ElapsedGameTime.TotalMilliseconds, _context);
            _uiManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ///
            /// Render Game World
            ///
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetViewMatrix());
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
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, new RasterizerState { ScissorTestEnable=true }, null, Camera.GetScaleMatrix());

            // stat bar
            _spriteBatch.Draw(_uiTexture, new Rectangle(0, 333, 160, 27), new Rectangle(0,64,160, 27), Color.White);
            // health
            _spriteBatch.Draw(_uiTexture, new Rectangle(3, 336, 154, 6), new Rectangle(104, 32, 8, 8), Color.White);
            _spriteBatch.Draw(_uiTexture, new Rectangle(3, 336, (int)(154f * ((float)LocalState.Health / (float)LocalState.MaxHealth)), 6), new Rectangle(96, 32, 8, 8), Color.White);
            // magic?
            _spriteBatch.Draw(_uiTexture, new Rectangle(3, 344, 154, 6), new Rectangle(120, 32, 8, 8), Color.White);
            _spriteBatch.Draw(_uiTexture, new Rectangle(3, 344, 134, 6), new Rectangle(112, 32, 8, 8), Color.White);
            // experience
            _spriteBatch.Draw(_uiTexture, new Rectangle(3, 352, 154, 6), new Rectangle(104, 40, 8, 8), Color.White);
            _spriteBatch.Draw(_uiTexture, new Rectangle(3, 352, (int)(154f * ((float)LocalState.Experience / (float)LocalState.MaxExperience)), 6), new Rectangle(96, 40, 8, 8), Color.White);

            // equipped weapon
            _spriteBatch.Draw(_uiTexture, new Rectangle(162, 333, 32, 27), new Rectangle(64, 34, 32, 27), Color.White);
            var equippedWeapon = LocalState.InventoryContents.FirstOrDefault(i =>
            {
                var ii = (Item)i.GetComponent(typeof(Item));
                return ii != null && ii.IsEquipped && ii.EquipmentSlot == "weapon";
            })?.GetComponent(typeof(Item)) as Item;
            if (equippedWeapon != null)
            {
                var resources = Registry.GetInstance();
                var source = resources.FrameSources[equippedWeapon.Icon];
                var sourceRect = new Rectangle(source.Left, source.Top, source.Width, source.Height);
                _spriteBatch.Draw(TextureResources[resources.Resources[source.Resource]], new Rectangle(162, 331, 32, 32), sourceRect, Color.White);
            }

            // draw windows
            _uiManager.Draw(_spriteBatch);

            // custom mouse cursor
            var mousePosition = Mouse.GetState().Position;
            _spriteBatch.Draw(_uiTexture, new Rectangle((int)(mousePosition.X/Camera.hScale), (int)(mousePosition.Y/Camera.vScale), 16, 16), new Rectangle(80,0,16,16), Color.White);


            _spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void DrawDrawableInfo(DrawableInfo drawls)
        {
            var spriteEffectFlags = (drawls.FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawls.FlipVertically ? SpriteEffects.FlipVertically : SpriteEffects.None);
            foreach (var rect in drawls.TextureRect)
            {
                _spriteBatch.Draw(TextureResources[drawls.Sheet], new Rectangle((int)drawls.Position.X, (int)drawls.Position.Y, drawls.Width, drawls.Height), rect, drawls.Color, drawls.Rotation, drawls.Origin, spriteEffectFlags, 0);

                // debug shapes
                // _spriteBatch.Draw(TextureResources[drawls.Sheet], new Rectangle((int)drawls.Position.X, (int)drawls.Position.Y + drawls.Height, drawls.Width, 1), new Rectangle(905,87,4,4), Color.White);
            }
        }

    }
}