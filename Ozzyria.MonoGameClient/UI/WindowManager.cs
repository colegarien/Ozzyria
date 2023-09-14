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
                VerticalScrollPercent = 0.5f,
                HorizontalScrollPercent = 0.5f,
                ContentWidth = 164,
                ContentHeight = 164,
            };
        }

        public void Update(float deltaTime)
        {
            _inputTracker.Calculate(deltaTime);


            if (_inputTracker.IsKeyReleased(Keys.I))
            {
                _inventoryWindow.IsVisible = !_inventoryWindow.IsVisible;
            }

            if (_inventoryWindow.IsVisible && _inventoryWindow.WindowArea.Contains(_inputTracker.MouseX(), _inputTracker.MouseY()) && _inputTracker.IsLeftMouseDown())
            {
                _inventoryWindow.X = _inputTracker.MouseX() - _inventoryWindow.WindowArea.Width / 2;
                _inventoryWindow.Y = _inputTracker.MouseY() - _inventoryWindow.WindowArea.Height / 2;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _inventoryWindow.Draw(spriteBatch);
        }
    }
}
