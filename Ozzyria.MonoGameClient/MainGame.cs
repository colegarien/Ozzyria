using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ozzyria.Content;
using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering;
using Ozzyria.MonoGameClient.Systems;
using Ozzyria.MonoGameClient.Systems.Rendering;
using Ozzyria.MonoGameClient.UI;
using Ozzyria.MonoGameClient.UI.Handlers;
using Ozzyria.MonoGameClient.UI.Windows;
using Ozzyria.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Ozzyria.MonoGameClient.UI.InputTracker;
using Movement = Ozzyria.Model.Components.Movement;
using Ozzyria.Model.Extensions;
using Ozzyria.Model.Types;

namespace Ozzyria.MonoGameClient
{
    public class MainGame : Microsoft.Xna.Framework.Game, IMouseUpHandler
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _greyFont;
        private SpriteFont _greyMonoFont;
        private GraphicsPipeline _pipeline;

        private EntityContext _context;
        private SystemCoordinator _coordinator;

        // "global" variables used by systems
        internal Dictionary<string, Texture2D> TextureResources;
        internal Client Client;
        internal Camera Camera;
        internal LocalState LocalState;
        internal Ozzyria.Content.Models.Area.AreaData AreaData = null;

        // for running a local server
        private const bool IS_SINGLEPLAYER = true;
        private CancellationTokenSource _cts;
        private Server _localServer;
        private Thread _localServerTheard;

        private Texture2D _uiTexture;
        internal WindowManager UiManager;
        internal BagWindow BagWindow;
        internal ContextActionWindow ActionWindow;

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
            _context = new EntityContext();
            _coordinator = new SystemCoordinator();
            _coordinator
                .Add(new AnimatorSystem())
                .Add(new BagSyncing(this))
                .Add(new Network(this))
                .Add(new LocalPlayer(this, _context))
                .Add(new CameraTracking(this, _context))
                .Add(new LocalStateTracking(this, _context))
                .Add(new BagTracking(this, _context))
                .Add(new SkeletonSystem(_context)).Add(new GraphicsSystem(_context));

            _pipeline = GraphicsPipeline.Get();

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
            _greyMonoFont = Content.Load<SpriteFont>("sf_monogreyfont");

            TextureResources = new Dictionary<string, Texture2D>()
            {
                {"entity_set_001", Content.Load<Texture2D>("Sprites/entity_set_001")},
                {"outside_tileset_001", Content.Load<Texture2D>("Sprites/outside_tileset_001")},
            };

            _uiTexture = Content.Load<Texture2D>("ui_components");
            UiManager = new WindowManager(new InputTracker(Camera));
            UiManager.SubscribeToEvents(this);
            UiManager.AddWindow(new InventoryWindow(this, _uiTexture, _greyFont)
            {
                IsVisible = false,
                X = 140,
                Y = 30,
            });
            UiManager.AddWindow(new ConsoleWindow(this, _uiTexture, _greyFont, _greyMonoFont)
            {
                IsVisible = false,
                X = 140,
                Y = 30,
            });
            BagWindow = new BagWindow(this, _uiTexture, _greyFont)
            {
                IsVisible = false,
                X = 140,
                Y = 30,
            };
            UiManager.AddWindow(BagWindow);
            ActionWindow = new ContextActionWindow(this, _uiTexture, _greyFont)
            {
                IsVisible = false,
                X = 140,
                Y = 30,
            };
            UiManager.AddWindow(ActionWindow);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!Client.IsConnected() || UiManager.QuitRequested())
            {
                Log($"Disconnecting");
                Client.Disconnect();
                Exit();
                return;
            }

            _coordinator.Execute((float)gameTime.ElapsedGameTime.TotalMilliseconds, _context);
            UiManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            _pipeline.SwapBuffer();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var resources = Registry.GetInstance();
            GraphicsDevice.Clear(Color.Black);

            ///
            /// Render Game World
            ///
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetViewMatrix());
            foreach(var graphic in _pipeline.GetGraphics(Camera))
            {
                if (!graphic.Hidden)
                {
                    var effect = SpriteEffects.None;
                    if (graphic.FlipHorizontally)
                        effect |= SpriteEffects.FlipHorizontally;
                    if (graphic.FlipVertically)
                        effect |= SpriteEffects.FlipVertically;

                    _spriteBatch.Draw(TextureResources[resources.Resources[graphic.Resource]], graphic.Destination, graphic.Source, graphic.Colour, graphic.Angle, graphic.Origin, effect, 0);
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
            var equippedWeapon = LocalState.GetBag(LocalState.PlayerEntityId).Contents.FirstOrDefault(i =>
            {
                var ii = (Item)i.GetComponent(typeof(Item));
                return ii != null && ii.IsEquipped && ii.EquipmentSlot == "weapon";
            })?.GetComponent(typeof(Item)) as Item;
            if (equippedWeapon != null)
            {
                if (resources.UIIcons.ContainsKey(equippedWeapon.Icon))
                {
                    var source = resources.UIIcons[equippedWeapon.Icon];
                    var sourceRect = new Rectangle(source.Left, source.Top, source.Width, source.Height);
                    var slotRectangle = new Rectangle(162, 331, 32, 32);

                    // draw icon centered onto the slot
                    _spriteBatch.Draw(TextureResources[resources.Resources[source.Resource]], new Rectangle(slotRectangle.Center.X - (sourceRect.Width / 2), slotRectangle.Center.Y - (sourceRect.Height / 2), sourceRect.Width, sourceRect.Height), sourceRect, Color.White);
                }
            }

            // draw windows
            UiManager.Draw(_spriteBatch);

            // custom mouse cursor
            var mousePosition = Mouse.GetState().Position;
            _spriteBatch.Draw(_uiTexture, new Rectangle((int)(mousePosition.X/Camera.hScale), (int)(mousePosition.Y/Camera.vScale), 16, 16), new Rectangle(80,0,16,16), Color.White);


            _spriteBatch.End();
            base.Draw(gameTime);
        }

        bool IMouseUpHandler.HandleMouseUp(InputTracker tracker, MouseButton button, int x, int y)
        {
            if (button == MouseButton.Right)
            {
                var clickedBag = _context.GetEntities().FirstOrDefault(e =>
                {
                    if (e.id == LocalState.PlayerEntityId || !e.HasComponent(typeof(Movement)) || !e.HasComponent(typeof(Bag)))
                    {
                        return false;
                    }

                    var m = e.GetComponent<Movement>();
                    return Math.Pow(m.X - tracker.WorldMouseX(), 2) + Math.Pow(m.Y - tracker.WorldMouseY(), 2) <= 100;
                });

                if (clickedBag != null)
                {
                    // open context menu for bag
                    ActionWindow.OpenContextMenu(tracker.MouseX(), tracker.MouseY(), clickedBag.id, clickedBag);
                    return true;
                }
            }

            return false;
        }
    }
}