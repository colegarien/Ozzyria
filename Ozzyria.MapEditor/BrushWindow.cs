using Ozzyria.MapEditor.EventSystem;
using SFML.Graphics;

namespace Ozzyria.MapEditor
{
    class BrushWindow : GWindow
    {
        public int SelectedBrush { get; set; } = 1;
        private int mouseX = 0;
        private int mouseY = 0;


        public BrushWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight, int margin, int padding) : base(x, y, width, height, screenWidth, screenHeight, margin, padding)
        {
            EventQueue.Queue(new BrushTypeChangeEvent
            {
                SelectedBrush = SelectedBrush
            });
        }


        public override void OnMouseDown(MouseDownEvent e)
        {
            if (!e.LeftMouseDown)
            {
                return;
            }

            var i = 0;
            foreach (int type in MapManager.GetTileTypes())
            {
                var left = GetILeft() + (i * 37);
                var top = GetITop();
                var dimension = 32;
                if (e.OriginX >= left && e.OriginX < left + dimension
                    && e.OriginY >= top && e.OriginY < top + dimension)
                {
                    SelectedBrush = type;
                    EventQueue.Queue(new BrushTypeChangeEvent
                    {
                        SelectedBrush = SelectedBrush
                    });
                    return;
                }
                i++;
            }
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
            foreach(int type in MapManager.GetTileTypes())
            {
                var left = GetILeft() + (i * 37);
                var top = GetITop();
                var dimension = 32;
                buffer.Draw(new RectangleShape()
                {
                    Size = new SFML.System.Vector2f(dimension, dimension),
                    Position = new SFML.System.Vector2f(left, top),
                    FillColor = Colors.TileColor(type),
                });

                var outlineColor = Colors.DefaultElement();
                if(mouseX >= left && mouseX < left+dimension
                    && mouseY >= top && mouseY < top + dimension)
                {
                    if (type == SelectedBrush)
                    {
                        outlineColor = Colors.HoverSelectedElement();
                    }
                    else
                    {
                        outlineColor = Colors.HoverElement();
                    }
                }else if (type == SelectedBrush)
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

                i++;
            }
        }
    }
}
