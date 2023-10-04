using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ozzyria.MonoGameClient.UI
{
    internal class WindowManager
    {
        private InputTracker _inputTracker;

        // UI Components
        Window _inventoryWindow;



        public WindowManager(Texture2D uiTexture, SpriteFont font, InputTracker inputTracker)
        {
            _inputTracker = inputTracker;

            _inventoryWindow = new Window(uiTexture, font)
            {
                IsVisible = false,
                HasCloseButton = true,
                HasVerticalScroll = true,
                HasHorizontalScroll = true,
                X = 140,
                Y = 30,
                Header = "Inventory",
                VerticalScrollPercent = 0f,
                HorizontalScrollPercent = 0f,
                ContentWidth = 164,
                ContentHeight = 164,
            };

            ///
            /// Register Events
            ///
            _inputTracker.OnMouseUp += _inventoryWindow.HandleMouseUp;
            _inputTracker.OnMouseDown += _inventoryWindow.HandleMouseDown;
            _inputTracker.OnMouseMove += _inventoryWindow.HandleMouseMove;
            _inputTracker.OnMouseVerticalScroll += _inventoryWindow.HandleVerticalScroll;
            _inputTracker.OnMouseHorizontalScroll += _inventoryWindow.HandleHorizontalScroll;
        }

        public void Update(float deltaTime)
        {
            _inputTracker.Calculate(deltaTime);


            if (_inputTracker.IsKeyReleased(Keys.I))
            {
                _inventoryWindow.IsVisible = !_inventoryWindow.IsVisible;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _inventoryWindow.Draw(spriteBatch);
        }
    }
}
