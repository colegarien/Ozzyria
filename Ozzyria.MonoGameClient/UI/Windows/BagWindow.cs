using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;
using static Ozzyria.MonoGameClient.UI.InputTracker;

namespace Ozzyria.MonoGameClient.UI.Windows
{
    internal class BagWindow : Window
    {

        private const int GRID_WIDTH = 5;
        private const int GRID_HEIGHT = 5;

        private const int GRID_DIM = 32;

        private Registry _resources;

        private int mouseGridX = -1;
        private int mouseGridY = -1;

        private ItemStatWindow _itemStatWindow;
        private ContextActionWindow _contextActionWindow;

        public uint BagEntityId { get; set; }

        public BagWindow(MainGame game, Texture2D uiTexture, SpriteFont font) : base(game, uiTexture, font)
        {
            _resources = Registry.GetInstance();
            _itemStatWindow = new ItemStatWindow(game, uiTexture, font);
            _contextActionWindow = new ContextActionWindow(game, uiTexture, font);

            HasCloseButton = true;
            HasVerticalScroll = false;
            HasHorizontalScroll = false;
            Header = "";
            VerticalScrollPercent = 0f;
            HorizontalScrollPercent = 0f;
            ContentWidth = (MARGIN * 2) + (GRID_WIDTH * GRID_DIM);
            ContentHeight = (MARGIN * 2) + (GRID_HEIGHT * GRID_DIM);
            ContentTotalWidth = ContentWidth;
            ContentTotalHeight = ContentHeight;
        }


        protected override void OnMouseMove(int previousX, int previousY, int x, int y)
        {
            if (_contextActionWindow.IsVisible && _contextActionWindow.WindowArea.Contains(x, y))
            {
                _contextActionWindow.HandleMouseMove(previousX, previousY, x, y);
            }

            mouseGridX = mouseGridY = -1;
            if (!contentArea.Contains(x, y))
            {
                return;
            }

            mouseGridX = (x - contentArea.X - MARGIN) / GRID_DIM;
            mouseGridY = (y - contentArea.Y - MARGIN) / GRID_DIM;

        }
        protected override void OnMouseClick(MouseButton button, int x, int y)
        {
            if (IsVisible && button == MouseButton.Right)
            {
                var inventoryContents = _game.LocalState.GetBag(BagEntityId).Contents;
                var mouseIndex = mouseGridX + mouseGridY * GRID_WIDTH;
                if (mouseIndex >= 0 && mouseIndex < inventoryContents.Count)
                {
                    var itemEntity = inventoryContents[mouseIndex];
                    _contextActionWindow.OpenContextMenu(x, y, BagEntityId, itemEntity);
                }
                else
                {
                    _contextActionWindow.CloseContextMenu();
                }

            }
            else if (button == MouseButton.Left && _contextActionWindow.IsVisible && _contextActionWindow.WindowArea.Contains(x, y))
            {
                _contextActionWindow.HandleMouseUp(button, x, y);
            }
            else
            {
                _contextActionWindow.CloseContextMenu();
            }
        }

        protected override void RenderContent(SpriteBatch spriteBatch)
        {
            if (!_game.LocalState.HasBag(BagEntityId))
            {
                // bag went missing, close out window
                Manager?.CloseWindow(this);
                return;
            }

            var localBagState = _game.LocalState.GetBag(BagEntityId);
            Header = localBagState.Name;

            var inventoryContents = localBagState.Contents;
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

                                // center source rect into slot center
                                var slotRect = new Rectangle(x + (GRID_DIM / 2) - (source.Width / 2), y + (GRID_DIM / 2) - (source.Height / 2), source.Width, source.Height);
                                ContentDraw(spriteBatch, _game.TextureResources[_resources.Resources[source.Resource]], contentArea, slotRect, sourceRect, true);
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
            if (!_contextActionWindow.IsVisible && mouseIndex >= 0 && mouseIndex < inventoryContents.Count)
            {
                var itemEntity = inventoryContents[mouseIndex];
                _itemStatWindow.ChangeSubject(ContentX + MARGIN + mouseGridX * GRID_DIM + GRID_DIM, ContentY + MARGIN + mouseGridY * GRID_DIM, itemEntity);
                _itemStatWindow.Draw(spriteBatch);
            }

            _contextActionWindow.Draw(spriteBatch);
        }

    }
}
