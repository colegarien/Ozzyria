using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ozzyria.MonoGameClient.UI.Windows
{
    internal class ConsoleWindow : Window
    {
        private SpriteFont _monoFont;

        const int FONT_DIM = 6;
        const int NUM_COLS = 40;
        const int NUM_ROWS = 20;

        int _logIndex = 0;
        string[] _log = new string[NUM_ROWS];

        string input = "";

        public ConsoleWindow(MainGame game, Texture2D uiTexture, SpriteFont font, SpriteFont monoFont) : base(game, uiTexture, font)
        {
            _monoFont = monoFont;

            Backing = darkRedImg;
            HasCloseButton = true;
            HasVerticalScroll = false;
            HasHorizontalScroll = false;
            Header = "Console";
            VerticalScrollPercent = 0f;
            HorizontalScrollPercent = 0f;
            ContentWidth = (MARGIN * 2) + (FONT_DIM * NUM_COLS);
            ContentHeight = (MARGIN * 2) + (FONT_DIM * (NUM_ROWS + 1)); // one extra line for input bar
            ContentTotalWidth = ContentWidth;
            ContentTotalHeight = ContentHeight;

            // TODO UI add in KEY EVENT pipeline to allow actually typing in INPUT
            AddMessage("WELCOME TO THE COMMAND CONSOLE!! THIS PROVIDES EASY ACCESS TO TINKER WITH DIFFERENT FEATURES AND FUNCTIONALITY WITHOUT DEALING TOO MUCH WITH THE UI!!");
            AddMessage(">");
            AddMessage(">");
            AddMessage(">PRINTLN 'HELLO'");
            AddMessage("-HELLO");
            AddMessage(">");
            AddMessage(">");
            AddMessage(">NON-SENSE INPUT");
            AddMessage("-UKNOWN COMMAND `NON-SENSE`");
            input = "COMMAND GOES HERE BEACH";
        }

        protected void AddMessage(string message)
        {
            string printableMessage = message;
            string extraMessage = "";
            if (printableMessage.Length > NUM_COLS) {
                // trim down large messages into multiple messages (break at whitespace)
                int printableLength = NUM_COLS;
                int nextWhitespace = printableMessage.Substring(0, printableLength).LastIndexOf(' ');
                if (nextWhitespace > 0 && nextWhitespace < printableLength)
                {
                    printableLength = nextWhitespace;
                }

                printableMessage = message.Substring(0, printableLength);
                // indent extra messages to make it clear the message are related
                extraMessage = "--" + message.Substring(printableLength).Trim();
            }

            _logIndex++;
            if(_logIndex >= NUM_ROWS)
            {
                _logIndex = 0;
            }

            _log[_logIndex] = printableMessage;

            if(extraMessage != "")
            {
                AddMessage(extraMessage);
            }
        }

        protected override void RenderContent(SpriteBatch spriteBatch)
        {
            ContentDraw(spriteBatch, _uiTexture, contentArea, contentArea, darkerRedImg);
            ContentDraw(spriteBatch, _uiTexture, new Rectangle(contentArea.X, contentArea.Bottom - MARGIN - FONT_DIM, contentArea.Width, FONT_DIM + MARGIN), contentArea, purpleImg);

            var output = "";
            bool wrapped = false;
            int i = _logIndex + 1;
            if (i >= NUM_ROWS)
                i = 0;
            int startIndex = i;

            while (!wrapped)
            {
                output += _log[i] + "\n";

                i++;
                if (i >= NUM_ROWS)
                    i = 0;

                // we wrapped around and are back at the start
                if (i == startIndex)
                    wrapped = true;
            }

            spriteBatch.DrawString(_monoFont, output, new Vector2(ContentX + MARGIN, ContentY + MARGIN), Color.DarkCyan);

            spriteBatch.DrawString(_monoFont, input, new Vector2(contentArea.X + MARGIN, contentArea.Bottom - (MARGIN / 2) - FONT_DIM), Color.Red);
            if (input.Length != NUM_COLS)
            {
                ContentDraw(spriteBatch, _uiTexture, new Rectangle(contentArea.X + MARGIN + (input.Length * FONT_DIM), contentArea.Bottom - (MARGIN / 2) - FONT_DIM, 2, 6), contentArea, blueImg);
            }
        }
    }
}
