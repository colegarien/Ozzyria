using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ozzyria.Model.Components;
using Grecs;

namespace Ozzyria.MonoGameClient.UI.Windows
{
    internal class ItemStatWindow : Window
    {
        public Entity Subject { get; set; }

        public ItemStatWindow(MainGame game, Texture2D uiTexture, SpriteFont font) : base(game, uiTexture, font)
        {
            Backing = redImg;

            Header = "";
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

        public void ChangeSubject(int x, int y, Entity entity)
        {
            if(Subject == entity)
            {
                return;
            }

            if (entity.HasComponent(typeof(Item)))
            {
                var item = (Item)entity.GetComponent(typeof(Item));

                Subject = entity;
                Header = item.Name;
                IsVisible = true;
                X = x;
                Y = y;

                CalculateInternals();
            }
            else
            {
                IsVisible = false;
                Subject = null;
            }
        }

        protected override void RenderContent(SpriteBatch spriteBatch)
        {
            if(Subject == null)
            {
                return;
            }

            var item = (Item)Subject.GetComponent(typeof(Item));

            ContentDraw(spriteBatch, _uiTexture, contentArea, new Rectangle(ContentX, ContentY, ContentWidth, ContentHeight), darkRedImg);
            spriteBatch.DrawString(_font, "Some details\nabout:\n  " + item.Name + "!!", new Vector2(ContentX + MARGIN, ContentY + MARGIN), Color.White);
            
            // TODO OZ-55 make combat based on stats of equipped gear and pull in actual stats here!!
        }
    }
}
