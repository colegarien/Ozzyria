using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ozzyria.MonoGameClient.UI.Windows
{
    internal class InventoryWindow : BagWindow
    {
        public InventoryWindow(MainGame game, Texture2D uiTexture, SpriteFont font) : base(game, uiTexture, font)
        {
        }

        protected override bool OnKeysReleased(InputTracker tracker)
        {
            if (tracker.IsKeyReleased(Keys.I))
            {
                Manager?.ToggleWindowVisibility(this);
            }

            return false;
        }

        protected override void RenderContent(SpriteBatch spriteBatch)
        {
            BagEntityId = _game.LocalState.PlayerEntityId;
            base.RenderContent(spriteBatch);
        }

    }
}
