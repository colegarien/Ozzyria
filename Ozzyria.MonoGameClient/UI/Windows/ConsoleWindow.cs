using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ozzyria.MonoGameClient.Rendering;

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

        int cursor = 0;
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

            AddMessage("----------------------------------------");
            AddMessage("--                                    --");
            AddMessage("-- OZZYTRON COMMAND CONSOLE           --");
            AddMessage("--                                    --");
            AddMessage("----------------------------------------");
            AddMessage("");
            AddMessage("  ++ INTENDED FOR INTERNAL USE ONLY ++  ");
            AddMessage("");
            AddMessage("THIS CONSOLE PROVIDES EASY ACCESS TO COMMANDS WITHOUT HAVING TO TOY WITH THE UI.");
            AddMessage("WELCOME TO THE FUTURE.");
            AddMessage("                          - OZZY GREYMAN");
            AddMessage("");
            AddMessage("");
            AddMessage("");
            AddMessage("");
            AddMessage("");
            AddMessage("");
            input = "";
            cursor = input.Length;
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
                extraMessage = "  " + message.Substring(printableLength).Trim();
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

        protected override bool OnKeysPressed(InputTracker tracker)
        {
            if (!IsVisible)
                return false;

            //
            // CURSOR CONTROLs
            //
            if (tracker.IsKeyPressed(Keys.Back))
            {
                if (cursor > 0)
                {
                    input = input.Remove(cursor - 1, 1);
                    cursor--;
                }
            }

            if (tracker.IsKeyPressed(Keys.Delete))
            {
                if (cursor < input.Length)
                {
                    input = input.Remove(cursor, 1);
                }
            }

            if (tracker.IsKeyPressed(Keys.Left))
            {
                if (cursor > 0)
                    cursor--;
            }

            if (tracker.IsKeyPressed(Keys.Right))
            {
                if (cursor + 1 <= input.Length)
                    cursor++;
            }

            if (tracker.IsKeyPressed(Keys.Enter))
            {
                ParseInput(input);
                input = "";
                cursor = 0;
            }

            //
            // TEXT ENTRY
            //
            if (tracker.IsKeyPressed(Keys.Space))
                DoInput(" ");
            if (tracker.IsKeyPressed(Keys.A))
                DoInput("A");
            if (tracker.IsKeyPressed(Keys.B))
                DoInput("B");
            if (tracker.IsKeyPressed(Keys.C))
                DoInput("C");
            if (tracker.IsKeyPressed(Keys.D))
                DoInput("D");
            if (tracker.IsKeyPressed(Keys.E))
                DoInput("E");
            if (tracker.IsKeyPressed(Keys.F))
                DoInput("F");
            if (tracker.IsKeyPressed(Keys.G))
                DoInput("G");
            if (tracker.IsKeyPressed(Keys.H))
                DoInput("H");
            if (tracker.IsKeyPressed(Keys.I))
                DoInput("I");
            if (tracker.IsKeyPressed(Keys.J))
                DoInput("J");
            if (tracker.IsKeyPressed(Keys.K))
                DoInput("K");
            if (tracker.IsKeyPressed(Keys.L))
                DoInput("L");
            if (tracker.IsKeyPressed(Keys.M))
                DoInput("M");
            if (tracker.IsKeyPressed(Keys.N))
                DoInput("N");
            if (tracker.IsKeyPressed(Keys.O))
                DoInput("O");
            if (tracker.IsKeyPressed(Keys.P))
                DoInput("P");
            if (tracker.IsKeyPressed(Keys.Q))
                DoInput("Q");
            if (tracker.IsKeyPressed(Keys.R))
                DoInput("R");
            if (tracker.IsKeyPressed(Keys.S))
                DoInput("S");
            if (tracker.IsKeyPressed(Keys.T))
                DoInput("T");
            if (tracker.IsKeyPressed(Keys.U))
                DoInput("U");
            if (tracker.IsKeyPressed(Keys.V))
                DoInput("V");
            if (tracker.IsKeyPressed(Keys.W))
                DoInput("W");
            if (tracker.IsKeyPressed(Keys.X))
                DoInput("X");
            if (tracker.IsKeyPressed(Keys.Y))
                DoInput("Y");
            if (tracker.IsKeyPressed(Keys.Z))
                DoInput("Z");
            if (tracker.IsKeyPressed(Keys.NumPad0) || tracker.IsKeyPressed(Keys.D0))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? ")" : "0");
            if (tracker.IsKeyPressed(Keys.NumPad1) || tracker.IsKeyPressed(Keys.D1))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "!" : "1");
            if (tracker.IsKeyPressed(Keys.NumPad2) || tracker.IsKeyPressed(Keys.D2))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "@" : "2");
            if (tracker.IsKeyPressed(Keys.NumPad3) || tracker.IsKeyPressed(Keys.D3))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "#" : "3");
            if (tracker.IsKeyPressed(Keys.NumPad4) || tracker.IsKeyPressed(Keys.D4))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "$" : "4");
            if (tracker.IsKeyPressed(Keys.NumPad5) || tracker.IsKeyPressed(Keys.D5))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "%" : "5");
            if (tracker.IsKeyPressed(Keys.NumPad6) || tracker.IsKeyPressed(Keys.D6))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "^" : "6");
            if (tracker.IsKeyPressed(Keys.NumPad7) || tracker.IsKeyPressed(Keys.D7))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "&" : "7");
            if (tracker.IsKeyPressed(Keys.NumPad8) || tracker.IsKeyPressed(Keys.D8))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "*" : "8");
            if (tracker.IsKeyPressed(Keys.NumPad9) || tracker.IsKeyPressed(Keys.D9))
                DoInput(tracker.IsKeyDown(Keys.RightShift) || tracker.IsKeyDown(Keys.LeftShift) ? "(" : "9");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemMinus))
                DoInput("_");
            else if(tracker.IsKeyPressed(Keys.OemMinus))
                DoInput("-");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemPlus))
                DoInput("+");
            else if (tracker.IsKeyPressed(Keys.OemPlus))
                DoInput("=");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemOpenBrackets))
                DoInput("{");
            else if (tracker.IsKeyPressed(Keys.OemOpenBrackets))
                DoInput("[");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemCloseBrackets))
                DoInput("}");
            else if (tracker.IsKeyPressed(Keys.OemCloseBrackets))
                DoInput("]");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemPipe))
                DoInput("|");
            else if (tracker.IsKeyPressed(Keys.OemPipe))
                DoInput("\\");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemComma))
                DoInput("<");
            else if (tracker.IsKeyPressed(Keys.OemComma))
                DoInput(",");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemPeriod))
                DoInput(">");
            else if (tracker.IsKeyPressed(Keys.OemPeriod))
                DoInput(".");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemSemicolon))
                DoInput(":");
            else if (tracker.IsKeyPressed(Keys.OemSemicolon))
                DoInput(";");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemQuotes))
                DoInput("\"");
            else if (tracker.IsKeyPressed(Keys.OemQuotes))
                DoInput("'");
            if ((tracker.IsKeyDown(Keys.LeftShift) || tracker.IsKeyDown(Keys.RightShift)) && tracker.IsKeyPressed(Keys.OemQuestion))
                DoInput("?");
            else if (tracker.IsKeyPressed(Keys.OemQuestion))
                DoInput("/");

            return true;
        }

        private void ParseInput(string input)
        {
            AddMessage(">" + input);

            var parts = input.Trim().ToUpper().Split(" ");
            if (parts[0] == "TOGGLE" && parts[1] == "DEBUG")
            {
                Settings.DebugRendering = !Settings.DebugRendering;
                return;
            }

            AddMessage("?? UNKOWN COMMAND `" + input + "`");
        }

        protected override bool OnKeysHeld(InputTracker tracker)
        {
            return true;
        }

        protected override bool OnKeysReleased(InputTracker tracker)
        {
            if (tracker.IsKeyReleased(Keys.OemTilde))
            {
                Manager?.ToggleWindowVisibility(this);
            }

            return true;
        }

        private void DoInput(string enteredText)
        {
            if (enteredText.Length <= 0)
                return;

            if (input.Length + enteredText.Length <= NUM_COLS)
            {
                input = input.Insert(cursor, enteredText.ToUpper());
                cursor++;
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
                ContentDraw(spriteBatch, _uiTexture, new Rectangle(contentArea.X + MARGIN + (cursor * FONT_DIM), contentArea.Bottom - (MARGIN / 2) - FONT_DIM, 2, 6), contentArea, blueImg);
            }
        }
    }
}
