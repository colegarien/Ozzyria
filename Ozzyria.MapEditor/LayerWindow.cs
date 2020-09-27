using SFML.Graphics;
using System;

namespace Ozzyria.MapEditor
{
    class LayerWindow : GWindow
    {

        public int CurrentLayer { get; set; } = 0;
        public int NumberOfLayers { get; set; } = 1;

        private int mouseX = 0;
        private int mouseY = 0;

        public LayerWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight) : base(x, y, width, height, screenWidth, screenHeight)
        {
        }

        public override void OnHorizontalScroll(float delta)
        {
            // not needed
        }

        public override void OnMouseMove(int x, int y)
        {
            mouseX = x;
            mouseY = y;
        }

        public override void OnVerticalScroll(float delta)
        {
            // might needed?
        }

        public void OnPickLayer(int x, int y)
        {
            // change layer selection
        }

        protected override void RenderWindowContents(RenderTarget buffer)
        {
            var font = FontFactory.GetRegular();

            var height = 25;
            var width = windowWidth - 20;
            var left = windowX + 10;
            var top = 0;

            // TODO... this is a doozy, probably add a 'button class' of some kind?
            var ii = 0;
            for (var i = 0; i < NumberOfLayers; i++)
            {
                top = windowY + 10 + (i * (height + 5));
                buffer.Draw(new RectangleShape()
                {
                    Size = new SFML.System.Vector2f(width, height),
                    Position = new SFML.System.Vector2f(left, top),
                    FillColor = Color.White,
                    OutlineColor = (mouseX >= left && mouseX < left + width && mouseY >= top && mouseY < top + height) ? (CurrentLayer == i ? Color.Yellow : Color.Cyan) : (CurrentLayer == i ? Color.Magenta : Color.White),
                    OutlineThickness = 1
                });
                buffer.Draw(new RectangleShape()
                {
                    Size = new SFML.System.Vector2f(height - 8, height - 8),
                    Position = new SFML.System.Vector2f(left + width - (height - 8) - 4, top + 4),
                    FillColor = Color.Red,
                });
                var text = new Text("Layer #" + i, font);
                text.CharacterSize = 12;
                text.Position = new SFML.System.Vector2f(left, top);
                text.FillColor = Color.Black;
                buffer.Draw(text);

                ii = i + 1;
            }

            top = windowY + 10 + (ii * (height + 5));
            buffer.Draw(new RectangleShape()
            {
                Size = new SFML.System.Vector2f(width, height),
                Position = new SFML.System.Vector2f(left, top),
                FillColor = Color.Green,
                OutlineColor = (mouseX >= left && mouseX < left + width && mouseY >= top && mouseY < top + height) ? Color.Cyan : Color.Green,
                OutlineThickness = 1
            });

            var addText = new Text("Add Layer", font);
            addText.CharacterSize = 12;
            addText.Position = new SFML.System.Vector2f(left, top);
            addText.FillColor = Color.Black;
            buffer.Draw(addText);
        }
    }
}
