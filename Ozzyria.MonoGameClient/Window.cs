using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ozzyria.MonoGameClient
{
    internal class Window
    {
        // Constants
        public const int MARGIN = 2;
        public const int PADDING = 3;
        public const int HEADER_HEIGHT = 17;

        // Resources, TODO not this
        private Texture2D _uiTexture;
        private SpriteFont _font;
        private Rectangle blueImg = new Rectangle(0, 0, 16, 16);
        private Rectangle redImg = new Rectangle(16, 0, 16, 16);
        private Rectangle purpleImg = new Rectangle(16, 16, 16, 16);
        private Rectangle greyImg = new Rectangle(0, 16, 16, 16);
        private Rectangle slotImg = new Rectangle(0, 32, 32, 32);
        private Rectangle equippedIconImg = new Rectangle(112, 0, 16, 16);
        private Rectangle closeImg = new Rectangle(64, 0, 11, 11);
        private Rectangle vScrollImg = new Rectangle(32, 32, 11, 16);
        private Rectangle vScrollHandleImg = new Rectangle(32, 22, 9, 10);
        private Rectangle hScrollImg = new Rectangle(48, 32, 16, 11);
        private Rectangle hScrollHandleImg = new Rectangle(48, 22, 10, 9);

        // Configs
        public bool HasCloseButton { get; set; } = false;
        public bool HasVerticalScroll { get; set; } = false;
        public bool HasHorizontalScroll { get; set; } = false;
        public Rectangle Backing { get; set; }

        // Variables
        public bool IsVisible { get; set; } = false;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public string Header { get; set; } = "";
        public float VerticalScrollPercent { get; set; } = 0f;
        public float HorizontalScrollPercent { get; set; } = 0f;

        public int ContentWidth { get; set; } = 0;
        public int ContentHeight { get; set; } = 0;

        public Window(Texture2D uiTexture, SpriteFont font)
        {
            _uiTexture = uiTexture;
            _font = font;
            Backing = blueImg;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible)
            {
                return;
            }

            var contentArea = new Rectangle(X + PADDING, Y + PADDING + HEADER_HEIGHT, ContentWidth, ContentHeight);

            var headerArea = HasVerticalScroll || !HasCloseButton
                ? new Rectangle(X + PADDING, Y + PADDING, contentArea.Width, HEADER_HEIGHT)
                : new Rectangle(X + PADDING, Y + PADDING, contentArea.Width - MARGIN - closeImg.Width, HEADER_HEIGHT);

            var closeButton = new Rectangle(headerArea.X + headerArea.Width + MARGIN, Y + PADDING, closeImg.Width, closeImg.Height);
            var closeLeftMargin = new Rectangle(headerArea.X + headerArea.Width, Y + PADDING, MARGIN, HasVerticalScroll ? headerArea.Height + contentArea.Height : headerArea.Height);
            var closeBottomMargin = new Rectangle(closeButton.X, closeButton.Y + closeButton.Height, closeButton.Width, headerArea.Height - closeButton.Height);

            var vScrollArea = new Rectangle(contentArea.X + contentArea.Width + MARGIN, contentArea.Y, vScrollImg.Width, contentArea.Height);
            var vScrollStart = vScrollArea.Y + MARGIN;
            var vScrollEnd = vScrollArea.Bottom - MARGIN - vScrollHandleImg.Height;
            var vScrollHandleArea = new Rectangle(vScrollArea.X + 1, vScrollArea.Y + MARGIN + (int)((vScrollEnd - vScrollStart) * VerticalScrollPercent), vScrollHandleImg.Width, vScrollHandleImg.Height);
            var vScrollBottomMargin = new Rectangle(vScrollArea.X - MARGIN, vScrollArea.Y + vScrollArea.Height, vScrollArea.Width + MARGIN, MARGIN);

            var hScrollArea = new Rectangle(contentArea.X, contentArea.Y + contentArea.Height + MARGIN, contentArea.Width, hScrollImg.Height);
            var hScrollStart = hScrollArea.X + MARGIN;
            var hScrollEnd = hScrollArea.Right - MARGIN - hScrollHandleImg.Width;
            var hScrollHandleArea = new Rectangle(hScrollArea.X + MARGIN + (int)((hScrollEnd - hScrollStart) * HorizontalScrollPercent), hScrollArea.Y + 1, hScrollHandleImg.Width, hScrollHandleImg.Height);
            var hScrollTopMargin = new Rectangle(hScrollArea.X, hScrollArea.Y - MARGIN, hScrollArea.Width, MARGIN);

            var hvFiller = new Rectangle(hScrollArea.X + hScrollArea.Width, vScrollArea.Y + vScrollArea.Height + MARGIN, vScrollArea.Width + MARGIN, hScrollArea.Height);

            // exterior PADDINGs
            var horizontalPaddingWidth = PADDING + contentArea.Width + PADDING;
            if (HasVerticalScroll)
            {
                horizontalPaddingWidth += MARGIN + vScrollArea.Width;
            }
            var verticalPaddingHeight = headerArea.Height + contentArea.Height;
            if (HasHorizontalScroll)
            {
                verticalPaddingHeight += MARGIN + hScrollArea.Height;
            }

            var topPadding = new Rectangle(X, Y, horizontalPaddingWidth, PADDING);
            var leftPadding = new Rectangle(X, Y + PADDING, PADDING, verticalPaddingHeight);

            var bottomPadding = new Rectangle(X, leftPadding.Bottom, horizontalPaddingWidth, PADDING);
            var rightPadding = new Rectangle(topPadding.Right - PADDING, Y + PADDING, PADDING, verticalPaddingHeight);

            // draw the window
            spriteBatch.Draw(_uiTexture, headerArea, Backing, Color.White);
            if (HasCloseButton)
            {
                spriteBatch.Draw(_uiTexture, closeButton, closeImg, Color.White);
                spriteBatch.Draw(_uiTexture, closeLeftMargin, Backing, Color.White);
                spriteBatch.Draw(_uiTexture, closeBottomMargin, Backing, Color.White);
            }
            if (HasVerticalScroll)
            {
                spriteBatch.Draw(_uiTexture, vScrollArea, vScrollImg, Color.White);
                spriteBatch.Draw(_uiTexture, vScrollHandleArea, vScrollHandleImg, Color.White);
                spriteBatch.Draw(_uiTexture, vScrollBottomMargin, Backing, Color.White);

                if (!HasCloseButton)
                {
                    // fill in where close button would go with background
                    spriteBatch.Draw(_uiTexture, closeLeftMargin, Backing, Color.White);
                    spriteBatch.Draw(_uiTexture, closeButton, Backing, Color.White);
                    spriteBatch.Draw(_uiTexture, closeBottomMargin, Backing, Color.White);
                }
            }
            if (HasHorizontalScroll)
            {
                spriteBatch.Draw(_uiTexture, hScrollArea, hScrollImg, Color.White);
                spriteBatch.Draw(_uiTexture, hScrollHandleArea, hScrollHandleImg, Color.White);
                spriteBatch.Draw(_uiTexture, hScrollTopMargin, Backing, Color.White);
            }

            if (HasVerticalScroll && HasHorizontalScroll)
            {
                spriteBatch.Draw(_uiTexture, hvFiller, Backing, Color.White);
            }

            spriteBatch.Draw(_uiTexture, topPadding, Backing, Color.White);
            spriteBatch.Draw(_uiTexture, bottomPadding, Backing, Color.White);
            spriteBatch.Draw(_uiTexture, leftPadding, Backing, Color.White);
            spriteBatch.Draw(_uiTexture, rightPadding, Backing, Color.White);
            if (Header != "")
            {
                spriteBatch.DrawString(_font, Header, new Vector2(headerArea.X, headerArea.Y), Color.White);
            }

            // draw content
            ContentDraw(spriteBatch, contentArea, contentArea, greyImg);
            for (var i = 0; i < 15; i++)
            {
                for (var j = 0; j < 15; j++)
                {
                    // TODO do a real calculation for total content width and height to de-hard-code scroll calculation
                    var x = (int)(contentArea.X + (i * 32) + MARGIN - HorizontalScrollPercent * 32);
                    var y = (int)(contentArea.Y + (j * 32) + MARGIN - VerticalScrollPercent * 32);
                    ContentDraw(spriteBatch, contentArea, new Rectangle(x, y, 32, 32), slotImg, true);
                }
            }
        }

        private void ContentDraw(SpriteBatch spriteBatch, Rectangle bounds, Rectangle destination, Rectangle source, bool cropSource = false)
        {
            if(bounds.Contains(destination)) {
                // don't crop
                spriteBatch.Draw(_uiTexture, destination, source, Color.White);
            }

            // decompose rectangles
            var destinationTop = destination.Top;
            var destinationBottom = destination.Bottom;
            var destinationLeft = destination.Left;
            var destinationRight = destination.Right;

            var sourceTop = source.Top;
            var sourceBottom = source.Bottom;
            var sourceLeft = source.Left;
            var sourceRight = source.Right;

            // calculate crop
            if(destinationTop < bounds.Top)
            {
                destinationTop = bounds.Top;
            }
            if(destinationBottom > bounds.Bottom)
            {
                destinationBottom = bounds.Bottom;
            }
            if(destinationLeft < bounds.Left)
            {
                destinationLeft = bounds.Left;
            }
            if(destinationRight > bounds.Right)
            {
                destinationRight = bounds.Right;
            }

            if (cropSource)
            {
                var widthChange = (float)source.Width / (float)destination.Width;
                var heightChange = (float)source.Height / (float)destination.Height;

                // scale destination delta onto source (they can be different sizes)
                sourceTop += (int)(destinationTop - destination.Top * heightChange);
                sourceBottom += (int)(destinationBottom - destination.Bottom * heightChange);
                sourceLeft += (int)(destinationLeft - destination.Left * widthChange);
                sourceRight += (int)(destinationRight - destination.Right * widthChange);
            }

            // apply crop
            var destinationWidth = destinationRight - destinationLeft;
            var destinationHeight = destinationBottom - destinationTop;

            var sourceWidth = sourceRight - sourceLeft;
            var sourceHeight = sourceBottom - sourceTop;

            // make sure not an invalid
            if (destinationWidth > 0 && destinationHeight > 0 && sourceWidth > 0 && sourceHeight > 0)
            {
                // recompose and draw
                spriteBatch.Draw(_uiTexture, new Rectangle(destinationLeft, destinationTop, destinationWidth, destinationHeight), new Rectangle(sourceLeft, sourceTop, sourceWidth, sourceHeight), Color.White);
            }
        }
    }
}
