using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System.Collections.Generic;
using static Ozzyria.MonoGameClient.UI.InputTracker;

namespace Ozzyria.MonoGameClient.UI.Windows
{
    internal class ContextAction
    {
        public string Label { get; set; } = "";
        public Rectangle RenderArea { get; set; } = new Rectangle();
        public System.Action Action;
    }

    internal class ContextActionWindow : Window
    {

        int mouseX = -1;
        int mouseY = -1;

        List<ContextAction> _actions = new List<ContextAction>();

        public ContextActionWindow(MainGame game, Texture2D uiTexture, SpriteFont font) : base(game, uiTexture, font)
        {
            Backing = purpleImg;

            Header = "Actions";
            HasCloseButton = false;
            HasVerticalScroll = false;
            HasHorizontalScroll = false;
            VerticalScrollPercent = 0f;
            HorizontalScrollPercent = 0f;
            ContentWidth = (MARGIN * 2) + 120;
            ContentHeight = (MARGIN * 2) + 120;
            ContentTotalWidth = ContentWidth;
            ContentTotalHeight = ContentHeight;
        }

        public void OpenContextMenu(int x, int y, uint bagEntityId, Entity subject)
        {
            // Reposition window
            X = x; Y = y - (HEADER_HEIGHT/2);
            CalculateInternals();

            int renderX = ContentX + MARGIN;
            int renderY = ContentY + MARGIN;

            _actions.Clear();

            if (subject.HasComponent(typeof(Item)))
            {
                var item = (Item)subject.GetComponent(typeof(Item));
                if (_game.LocalState.GetBag(bagEntityId).Contents.Contains(subject))
                {
                    if (item.EquipmentSlot != "")
                    {
                        if (!item.IsEquipped)
                        {
                            _actions.Add(new ContextAction
                            {
                                Label = "equip",
                                RenderArea = new Rectangle(renderX, renderY, ContentWidth - (MARGIN * 2), 14),
                                Action = () =>
                                {
                                    _game.Client.RequestEquipItem(bagEntityId, item.Slot);
                                }
                            });
                            renderY += 14;
                        }
                        else
                        {
                            _actions.Add(new ContextAction
                            {
                                Label = "unequip",
                                RenderArea = new Rectangle(renderX, renderY, ContentWidth - (MARGIN * 2), 14),
                                Action = () =>
                                {
                                    _game.Client.RequestUnequipItem(bagEntityId, item.Slot);
                                }
                            });
                            renderY += 14;
                        }
                    }

                    _actions.Add(new ContextAction
                    {
                        Label = "drop",
                        RenderArea = new Rectangle(renderX, renderY, ContentWidth - (MARGIN * 2), 14),
                        Action = () =>
                        {
                            _game.Client.RequestDropItem(bagEntityId, item.Slot);
                        }
                    });
                    renderY += 14;
                }
            }

            IsVisible = true;
        }

        public void CloseContextMenu()
        {
            _actions.Clear();
            IsVisible = false;
        }

        protected override void OnMouseMove(int previousX, int previousY, int x, int y)
        {
            // track mouse for hover effect
            mouseX = x;
            mouseY = y;
        }

        protected override void OnMouseClick(MouseButton button, int x, int y)
        {
            if(button == MouseButton.Left)
            {
                foreach (var action in _actions)
                {
                    if(action.RenderArea.Contains(x, y))
                    {
                        action.Action?.Invoke();
                        CloseContextMenu();
                        break;
                    }
                }
            }
        }

        protected override void RenderContent(SpriteBatch spriteBatch)
        {
            ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(ContentX, ContentY, ContentWidth, ContentHeight), darkPurpleImg);

            // Draw hover higlights
            foreach (var action in _actions)
            {
                if (action.RenderArea.Contains(mouseX, mouseY))
                {
                    spriteBatch.Draw(_uiTexture, action.RenderArea, darkerPurpleImg, Color.White);
                }
            }

            // Draw Labels
            foreach (var action in _actions)
            {
                spriteBatch.DrawString(_font, action.Label, new Vector2(action.RenderArea.X, action.RenderArea.Y), action.RenderArea.Contains(mouseX, mouseY) ? Color.Yellow : Color.White);
            }

        }
    }
}
