using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ozzyria.Game;
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
        private SpriteFont _greyFont;

        private EntityContext _context;
        private SystemCoordinator _coordinator;

        private Dictionary<string, Texture2D> textureResources;

        // "global" variables used by systems
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

            textureResources = new Dictionary<string, Texture2D>()
            {
                {"entity_set_001", Content.Load<Texture2D>("Sprites/entity_set_001")},
                {"outside_tileset_001", Content.Load<Texture2D>("Sprites/outside_tileset_001")},
            };

            _uiTexture = Content.Load<Texture2D>("ui_components");
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
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, new RasterizerState { ScissorTestEnable=true }, null, Camera.GetScaleMatrix());

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
            // TODO draw icon for weapon here

            // window panel resources
            var blueImg = new Rectangle(0,0,16,16);
            var greyImg = new Rectangle(0, 16, 16, 16);
            var slotImg = new Rectangle(0, 32, 32, 32);
            var equippedIconImg = new Rectangle(112, 0, 16, 16);
            var exitImg = new Rectangle(64, 0, 11, 11);
            var vScrollImg = new Rectangle(32, 32, 11, 16);
            var vScrollHandleImg = new Rectangle(32, 22, 9, 10);
            var hScrollImg = new Rectangle(48, 32, 16, 11);
            var hScrollHandleImg = new Rectangle(48, 22, 10, 9);

            // window panel constants
            var padding = 3;
            var margin = 2;
            var headerHeight = 17;

            // window panel config
            var hasExitButton = true;
            var hasVerticalScroll = true;
            var hasHorizontalScroll = true;

            // window panel variables
            var windowPosition = new Vector2(140, 30);
            var windowHeader = "Inventory";
            var verticalScrollPercent = 0.5f;
            var horizontalScrollPercent = 0f;

            var contentArea = new Rectangle((int)windowPosition.X + padding, (int)windowPosition.Y+padding+headerHeight,164,164);

            var headerArea = hasVerticalScroll || !hasExitButton
                ? new Rectangle((int)windowPosition.X + padding, (int)windowPosition.Y + padding, contentArea.Width, headerHeight)
                : new Rectangle((int)windowPosition.X + padding, (int)windowPosition.Y + padding, contentArea.Width-margin-exitImg.Width, headerHeight);

            var exitButton = new Rectangle(headerArea.X + headerArea.Width + margin, (int)windowPosition.Y+padding, exitImg.Width, exitImg.Height);
            var exitLeftMargin = new Rectangle(headerArea.X + headerArea.Width, (int)windowPosition.Y + padding, margin, hasVerticalScroll ? headerArea.Height + contentArea.Height: headerArea.Height);
            var exitBottomMargin = new Rectangle(exitButton.X, exitButton.Y + exitButton.Height, exitButton.Width, headerArea.Height - exitButton.Height);

            var vScrollArea = new Rectangle(contentArea.X + contentArea.Width + margin, contentArea.Y, vScrollImg.Width, contentArea.Height);
            var vScrollStart = vScrollArea.Y + margin;
            var vScrollEnd = vScrollArea.Bottom - margin - vScrollHandleImg.Height;
            var vScrollHandleArea = new Rectangle(vScrollArea.X + 1, vScrollArea.Y + margin + (int)((vScrollEnd-vScrollStart) * verticalScrollPercent), vScrollHandleImg.Width, vScrollHandleImg.Height);
            var vScrollBottomMargin =  new Rectangle(vScrollArea.X - margin, vScrollArea.Y + vScrollArea.Height, vScrollArea.Width+margin, margin);

            var hScrollArea = new Rectangle(contentArea.X, contentArea.Y + contentArea.Height + margin, contentArea.Width, hScrollImg.Height);
            var hScrollStart = hScrollArea.X + margin;
            var hScrollEnd = hScrollArea.Right - margin - hScrollHandleImg.Width;
            var hScrollHandleArea =  new Rectangle(hScrollArea.X + margin + (int)((hScrollEnd - hScrollStart) * horizontalScrollPercent), hScrollArea.Y + 1, hScrollHandleImg.Width, hScrollHandleImg.Height);
            var hScrollTopMargin = new Rectangle(hScrollArea.X, hScrollArea.Y - margin, hScrollArea.Width, margin);

            var hvFiller = new Rectangle(hScrollArea.X + hScrollArea.Width, vScrollArea.Y+vScrollArea.Height+margin, vScrollArea.Width+margin, hScrollArea.Height);

            // exterior paddings
            var horizontalPaddingWidth = padding + contentArea.Width + padding;
            if (hasVerticalScroll)
            {
                horizontalPaddingWidth += margin + vScrollArea.Width;
            }
            var verticalPaddingHeight = headerArea.Height + contentArea.Height;
            if (hasHorizontalScroll)
            {
                verticalPaddingHeight += margin + hScrollArea.Height;
            }

            var topPadding = new Rectangle((int)windowPosition.X, (int)windowPosition.Y, horizontalPaddingWidth, padding);
            var leftPadding = new Rectangle((int)windowPosition.X, (int)windowPosition.Y + padding, padding, verticalPaddingHeight);

            var bottomPadding = new Rectangle((int)windowPosition.X, leftPadding.Bottom, horizontalPaddingWidth, padding);
            var rightPadding = new Rectangle(topPadding.Right-padding, (int)windowPosition.Y + padding, padding, verticalPaddingHeight);

            // draw the window
            _spriteBatch.Draw(_uiTexture, headerArea, blueImg, Color.White);
            if (hasExitButton)
            {
                _spriteBatch.Draw(_uiTexture, exitButton, exitImg, Color.White);
                _spriteBatch.Draw(_uiTexture, exitLeftMargin, blueImg, Color.White);
                _spriteBatch.Draw(_uiTexture, exitBottomMargin, blueImg, Color.White);
            }
            if (hasVerticalScroll)
            {
                _spriteBatch.Draw(_uiTexture, vScrollArea, vScrollImg, Color.White);
                _spriteBatch.Draw(_uiTexture, vScrollHandleArea, vScrollHandleImg, Color.White);
                _spriteBatch.Draw(_uiTexture, vScrollBottomMargin, blueImg, Color.White);

                if (!hasExitButton)
                {
                    // fill in where exit button would go with background
                    _spriteBatch.Draw(_uiTexture, exitLeftMargin, blueImg, Color.White);
                    _spriteBatch.Draw(_uiTexture, exitButton, blueImg, Color.White);
                    _spriteBatch.Draw(_uiTexture, exitBottomMargin, blueImg, Color.White);
                }
            }
            if (hasHorizontalScroll)
            {
                _spriteBatch.Draw(_uiTexture, hScrollArea, hScrollImg, Color.White);
                _spriteBatch.Draw(_uiTexture, hScrollHandleArea, hScrollHandleImg, Color.White);
                _spriteBatch.Draw(_uiTexture, hScrollTopMargin, blueImg, Color.White);
            }

            if(hasVerticalScroll && hasHorizontalScroll)
            {
                _spriteBatch.Draw(_uiTexture, hvFiller, blueImg, Color.White);
            }

            _spriteBatch.Draw(_uiTexture, topPadding, blueImg, Color.White);
            _spriteBatch.Draw(_uiTexture, bottomPadding, blueImg, Color.White);
            _spriteBatch.Draw(_uiTexture, leftPadding, blueImg, Color.White);
            _spriteBatch.Draw(_uiTexture, rightPadding, blueImg, Color.White);
            if (windowHeader != "")
            {
                _spriteBatch.DrawString(_greyFont, windowHeader, new Vector2(headerArea.X, headerArea.Y), Color.White);
            }

            // draw content
            var originalScissor = GraphicsDevice.ScissorRectangle;
            GraphicsDevice.ScissorRectangle = new Rectangle((int)(contentArea.X * Camera.hScale), (int)(contentArea.Y * Camera.hScale), (int)(contentArea.Width* Camera.hScale), (int)(contentArea.Height* Camera.hScale));
            
            _spriteBatch.Draw(_uiTexture, contentArea, greyImg, Color.White);
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 15; j++)
                {
                    var x = (int)(contentArea.X + (i * 32) + margin);
                    var y = (int)(contentArea.Y + (j * 32) + margin - verticalScrollPercent * 32);
                    _spriteBatch.Draw(_uiTexture, new Rectangle(x, y, 32, 32), slotImg, Color.White);
                }
            }

            GraphicsDevice.ScissorRectangle = originalScissor;

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
                _spriteBatch.Draw(textureResources[drawls.Sheet], new Rectangle((int)drawls.Position.X, (int)drawls.Position.Y, drawls.Width, drawls.Height), rect, drawls.Color, drawls.Rotation, drawls.Origin, spriteEffectFlags, 0);
            }
        }

    }
}