using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ozzyria.MonoGameClient.UI.Windows
{
    internal class InventoryWindow : Window
    {

        private const int GRID_WIDTH = 5;
        private const int GRID_HEIGHT = 5;

        private const int GRID_DIM = 32;

        private Texture2D _itemTexture;

        public InventoryWindow(Texture2D uiTexture, Texture2D itemTexture, SpriteFont font) : base(uiTexture, font)
        {
            // TODO UI ideally have a central registry.. might need to refactor the whole "Drawables" situation
            _itemTexture = itemTexture;

            HasCloseButton = true;
            HasVerticalScroll = false;
            HasHorizontalScroll = false;
            Header = "Inventory";
            VerticalScrollPercent = 0f;
            HorizontalScrollPercent = 0f;
            ContentWidth = (MARGIN * 2) + (GRID_WIDTH * GRID_DIM);
            ContentHeight = (MARGIN * 2) + (GRID_HEIGHT * GRID_DIM);
            ContentTotalWidth = ContentWidth;
            ContentTotalHeight = ContentHeight;
        }

        protected override void RenderContent(SpriteBatch spriteBatch)
        {
            ContentDraw(spriteBatch, _uiTexture, contentArea, contentArea, greyImg);
            for (var i = 0; i < GRID_WIDTH; i++)
            {
                for (var j = 0; j < GRID_HEIGHT; j++)
                {
                    var x = ContentX + MARGIN + i * GRID_DIM;
                    var y = ContentY + MARGIN + j * GRID_DIM;

                    // TODO UI link in renderables based on player's inventory and what's equipped
                    ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x, y, GRID_DIM, GRID_DIM), slotImg, true);

                    // TODO UI actually check items in inventory and what is equipped by player
                    if(i == 0 && j == 0)
                    {
                        ContentDraw(spriteBatch, _itemTexture, contentArea, new Rectangle(x, y+4, GRID_DIM, GRID_DIM), new Rectangle(928,0,32,32), true);
                        ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x+16, y, 16, 16), equippedIconImg, true);
                    } else if (i == 1 && j == 0)
                    {
                        ContentDraw(spriteBatch, _itemTexture, contentArea, new Rectangle(x, y + 2, GRID_DIM, GRID_DIM), new Rectangle(928, 137, 32, 32), true);
                        ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x + 16, y, 16, 16), equippedIconImg, true);
                    }
                    else if (i == 2 && j == 0)
                    {
                        ContentDraw(spriteBatch, _itemTexture, contentArea, new Rectangle(x, y + 12, GRID_DIM, GRID_DIM), new Rectangle(928, 169, 32, 32), true);
                        ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x + 16, y, 16, 16), equippedIconImg, true);
                    }
                    else if (i == 3 && j == 0)
                    {
                        ContentDraw(spriteBatch, _itemTexture, contentArea, new Rectangle(x+6, y, GRID_DIM, GRID_DIM), new Rectangle(32, 96, 32, 32), true);
                        ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x + 16, y, 16, 16), equippedIconImg, true);
                    }
                }
            }
        }

    }
}
