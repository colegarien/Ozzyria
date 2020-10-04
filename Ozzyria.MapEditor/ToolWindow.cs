using Ozzyria.MapEditor.EventSystem;
using SFML.Graphics;
using System;

namespace Ozzyria.MapEditor
{
    class ToolWindow : GWindow
    {
        public ToolType SelectedTool { get; set; } = ToolType.Pencil;
        private int mouseX = 0;
        private int mouseY = 0;

        public ToolWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight, int margin, int padding) : base(x, y, width, height, screenWidth, screenHeight, margin, padding)
        {
        }

        public override void OnMouseDown(MouseDownEvent e)
        {
            if (!e.LeftMouseDown)
            {
                return;
            }

            // TODO select tool
        }

        public override void OnMouseMove(MouseMoveEvent e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        public override void OnHorizontalScroll(HorizontalScrollEvent e)
        {
            // do nothing
        }

        public override void OnVerticalScroll(VerticalScrollEvent e)
        {
            // do nothing
        }

        protected override void RenderWindowContents(RenderTarget buffer)
        {
            var i = 0;
            var tool = ToolType.Pencil;

            var left = GetILeft() + (i * 37);
            var top = GetITop();
            var dimension = 32;
            buffer.Draw(new RectangleShape()
            {
                Size = new SFML.System.Vector2f(dimension, dimension),
                Position = new SFML.System.Vector2f(left, top),
                FillColor = Color.Black,
            });
            buffer.Draw(new Text
            {
                CharacterSize = 16,
                DisplayedString = "P",
                FillColor = Color.Red,
                OutlineColor = Color.Black,
                OutlineThickness = 1,
                Font = FontFactory.GetRegular(),
                Position = new SFML.System.Vector2f(left+10, top+5)
            });

            var outlineColor = Colors.DefaultElement();
            if (mouseX >= left && mouseX < left + dimension
                && mouseY >= top && mouseY < top + dimension)
            {
                if (tool == SelectedTool)
                {
                    outlineColor = Colors.HoverSelectedElement();
                }
                else
                {
                    outlineColor = Colors.HoverElement();
                }
            }
            else if (tool == SelectedTool)
            {
                outlineColor = Colors.SelectedElement();
            }

            buffer.Draw(new RectangleShape()
            {
                Size = new SFML.System.Vector2f(32, 32),
                Position = new SFML.System.Vector2f(GetILeft() + (i * 37), GetITop()),
                FillColor = Color.Transparent,
                OutlineColor = outlineColor,
                OutlineThickness = 2
            });

        }
    }
}
