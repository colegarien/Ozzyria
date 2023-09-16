using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Ozzyria.MonoGameClient.UI.InputTracker;

namespace Ozzyria.MonoGameClient.UI
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

        public Rectangle WindowArea { get; set; } = new Rectangle(0, 0, 0, 0);

        #region components
        protected Rectangle headerArea;
        protected Rectangle closeButton;

        protected int vScrollStart;
        protected int vScrollEnd;
        protected Rectangle vScrollHandleArea;

        protected int hScrollStart;
        protected int hScrollEnd;
        protected Rectangle hScrollHandleArea;
        #endregion

        public Window(Texture2D uiTexture, SpriteFont font)
        {
            _uiTexture = uiTexture;
            _font = font;
            Backing = blueImg;
        }

        /// 
        /// EVENT HANDLERS
        /// 
        protected bool dragVertScroll = false;
        protected bool dragHorzScroll = false;
        protected bool dragWindow = false;
        protected int dragXOffset = 0;
        protected int dragYOffset = 0;
        protected bool mStartedOnExit = false;
        public void HandleMouseDown(MouseButton button, int x, int y)
        {
            if (!IsVisible || !WindowArea.Contains(x, y))
            {
                return;
            }

            if(button == MouseButton.Left && vScrollHandleArea.Contains(x, y))
            {
                dragVertScroll = true;
                dragXOffset = x - vScrollHandleArea.Left;
                dragYOffset = y - vScrollHandleArea.Top;
            }
            if (button == MouseButton.Left && hScrollHandleArea.Contains(x, y))
            {
                dragHorzScroll = true;
                dragXOffset = x - hScrollHandleArea.Left;
                dragYOffset = y - hScrollHandleArea.Top;
            }
            if (button == MouseButton.Left && headerArea.Contains(x, y))
            {
                dragWindow = true;
                dragXOffset = x - headerArea.Left;
                dragYOffset = y - headerArea.Top;
            }

            if (HasCloseButton && button == MouseButton.Left && closeButton.Contains(x,y))
            {
                mStartedOnExit = true;
            }
        }
        public void HandleMouseUp(MouseButton button, int x, int y)
        {

            dragVertScroll = false;
            dragHorzScroll = false;
            dragWindow = false;

            if (HasCloseButton && button == MouseButton.Left && mStartedOnExit && closeButton.Contains(x, y))
            {
                IsVisible = false;
            }

            mStartedOnExit = false;
        }

        public void HandleMouseMove(int previousX, int previousY, int x, int y)
        {
            if (!IsVisible)
            {
                return;
            }

            if (dragVertScroll)
            {
                VerticalScrollPercent = (float)(y - dragYOffset - vScrollStart) / (float)(vScrollEnd - vScrollStart);
                if(VerticalScrollPercent < 0)
                {
                    VerticalScrollPercent = 0;
                }
                else if(VerticalScrollPercent > 1)
                {
                    VerticalScrollPercent = 1;
                }
            }

            if (dragHorzScroll)
            {
                HorizontalScrollPercent = (float)(x - dragXOffset - hScrollStart) / (float)(hScrollEnd - hScrollStart);
                if (HorizontalScrollPercent < 0)
                {
                    HorizontalScrollPercent = 0;
                }
                else if (HorizontalScrollPercent > 1)
                {
                    HorizontalScrollPercent = 1;
                }
            }

            if (dragWindow)
            {
                X = x - dragXOffset - PADDING;
                Y = y - dragYOffset - PADDING;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible)
            {
                return;
            }

            var contentArea = new Rectangle(X + PADDING, Y + PADDING + HEADER_HEIGHT, ContentWidth, ContentHeight);

            headerArea = HasVerticalScroll || !HasCloseButton
                ? new Rectangle(X + PADDING, Y + PADDING, contentArea.Width, HEADER_HEIGHT)
                : new Rectangle(X + PADDING, Y + PADDING, contentArea.Width - MARGIN - closeImg.Width, HEADER_HEIGHT);

            closeButton = new Rectangle(headerArea.X + headerArea.Width + MARGIN, Y + PADDING, closeImg.Width, closeImg.Height);
            var closeLeftMargin = new Rectangle(headerArea.X + headerArea.Width, Y + PADDING, MARGIN, HasVerticalScroll ? headerArea.Height + contentArea.Height : headerArea.Height);
            var closeBottomMargin = new Rectangle(closeButton.X, closeButton.Y + closeButton.Height, closeButton.Width, headerArea.Height - closeButton.Height);

            var vScrollArea = new Rectangle(contentArea.X + contentArea.Width + MARGIN, contentArea.Y, vScrollImg.Width, contentArea.Height);
            vScrollStart = vScrollArea.Y + MARGIN;
            vScrollEnd = vScrollArea.Bottom - MARGIN - vScrollHandleImg.Height;
            vScrollHandleArea = new Rectangle(vScrollArea.X + 1, vScrollStart + (int)((vScrollEnd - vScrollStart) * VerticalScrollPercent), vScrollHandleImg.Width, vScrollHandleImg.Height);
            var vScrollBottomMargin = new Rectangle(vScrollArea.X - MARGIN, vScrollArea.Y + vScrollArea.Height, vScrollArea.Width + MARGIN, MARGIN);

            var hScrollArea = new Rectangle(contentArea.X, contentArea.Y + contentArea.Height + MARGIN, contentArea.Width, hScrollImg.Height);
            hScrollStart = hScrollArea.X + MARGIN;
            hScrollEnd = hScrollArea.Right - MARGIN - hScrollHandleImg.Width;
            hScrollHandleArea = new Rectangle(hScrollArea.X + MARGIN + (int)((hScrollEnd - hScrollStart) * HorizontalScrollPercent), hScrollArea.Y + 1, hScrollHandleImg.Width, hScrollHandleImg.Height);
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

            WindowArea = new Rectangle(X, Y, horizontalPaddingWidth, verticalPaddingHeight);

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
            var totalConentWidth = 10 * 32;
            var totalConentHeight = 10 * 32;
            for (var i = 0; i < 15; i++)
            {
                for (var j = 0; j < 15; j++)
                {
                    // TODO do a real calculation for total content width and height to de-hard-code scroll calculation
                    var x = (int)(contentArea.X + i * 32 + MARGIN - (HorizontalScrollPercent * totalConentWidth));
                    var y = (int)(contentArea.Y + j * 32 + MARGIN - (VerticalScrollPercent * totalConentHeight));
                    ContentDraw(spriteBatch, contentArea, new Rectangle(x, y, 32, 32), slotImg, true);
                }
            }
        }

        private void ContentDraw(SpriteBatch spriteBatch, Rectangle bounds, Rectangle destination, Rectangle source, bool cropSource = false)
        {
            if (bounds.Contains(destination))
            {
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
            if (destinationTop < bounds.Top)
            {
                destinationTop = bounds.Top;
            }
            if (destinationBottom > bounds.Bottom)
            {
                destinationBottom = bounds.Bottom;
            }
            if (destinationLeft < bounds.Left)
            {
                destinationLeft = bounds.Left;
            }
            if (destinationRight > bounds.Right)
            {
                destinationRight = bounds.Right;
            }

            if (cropSource)
            {
                var widthChange = source.Width / (float)destination.Width;
                var heightChange = source.Height / (float)destination.Height;

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
