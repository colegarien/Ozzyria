using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;
using System;

namespace Ozzyria.MonoGameClient.UI.Windows
{
    internal class InventoryWindow : Window
    {

        private const int GRID_WIDTH = 5;
        private const int GRID_HEIGHT = 5;

        private const int GRID_DIM = 32;

        private Registry _resources;

        private int mouseGridX = -1;
        private int mouseGridY = -1;

        public InventoryWindow(MainGame game, Texture2D uiTexture, SpriteFont font) : base(game, uiTexture, font)
        {
            // TODO UI move the ui textures into the Resource Registry!
            _resources = Registry.GetInstance();

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

        protected override void OnMouseMove(int previousX, int previousY, int x, int y)
        {
            mouseGridX = mouseGridY = -1;
            if (!contentArea.Contains(x, y))
            {
                return;
            }

            mouseGridX = (x - contentArea.X - MARGIN) / GRID_DIM;
            mouseGridY = (y - contentArea.Y - MARGIN) / GRID_DIM;
        }

        protected override void RenderContent(SpriteBatch spriteBatch)
        {
            var inventoryContents = _game.LocalState.InventoryContents;
            ContentDraw(spriteBatch, _uiTexture, contentArea, contentArea, greyImg);
            for (var i = 0; i < GRID_WIDTH; i++)
            {
                for (var j = 0; j < GRID_HEIGHT; j++)
                {
                    var x = ContentX + MARGIN + i * GRID_DIM;
                    var y = ContentY + MARGIN + j * GRID_DIM;
                    ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x, y, GRID_DIM, GRID_DIM), slotImg, true);
                    if (mouseGridX == i && mouseGridY == j)
                    {
                        // highlight hovered over slot
                        ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x, y, GRID_DIM, GRID_DIM), slotHighlightImg, true);
                    }

                    // Render Items
                    var index = i + j * GRID_WIDTH;
                    if (index < inventoryContents.Count)
                    {
                        var itemEntity = inventoryContents[index];
                        if (itemEntity.HasComponent(typeof(Item)))
                        {
                            var item = (Item)itemEntity.GetComponent(typeof(Item));

                            if (_resources.FrameSources.ContainsKey(item.Icon))
                            {
                                var source = _resources.FrameSources[item.Icon];
                                var sourceRect = new Rectangle(source.Left, source.Top, source.Width, source.Height);
                                ContentDraw(spriteBatch, _game.TextureResources[_resources.Resources[source.Resource]], contentArea, new Rectangle(x, y, GRID_DIM, GRID_DIM), sourceRect, true);
                            }
                            else
                            {
                                ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x, y, GRID_DIM, GRID_DIM), missingIconImg, true);
                            }

                            if (item.IsEquipped)
                            {
                                ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x + 16, y, 16, 16), equippedIconImg, true);
                            }
                        }
                        else
                        {
                            ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(x, y, GRID_DIM, GRID_DIM), missingIconImg, true);
                        }

                    }
                }
            }

            var mouseIndex = mouseGridX + mouseGridY * GRID_WIDTH;
            if (mouseIndex >= 0 && mouseIndex < inventoryContents.Count)
            {
                var itemEntity = inventoryContents[mouseIndex];
                if (itemEntity.HasComponent(typeof(Item)))
                {
                    var item = (Item)itemEntity.GetComponent(typeof(Item));

                    // TODO UI render proper stats tool tip box!
                    var x = ContentX + MARGIN + mouseGridX * GRID_DIM + GRID_DIM;
                    var y = ContentY + MARGIN + mouseGridY * GRID_DIM;
                    spriteBatch.Draw(_uiTexture, new Rectangle(x, y, 120, 14), purpleImg, Color.White);
                    spriteBatch.DrawString(_font, item.Name, new Vector2(x+MARGIN, y+MARGIN), Color.White);

                    /**
                     * TODO
                     *  1. add equip / unequip
                     *  2. make EquippedGear based on what's equipped in Inventory
                     *  3. make combat based on stats of equipped gear!
                     */
                }
            }
        }

    }
}
