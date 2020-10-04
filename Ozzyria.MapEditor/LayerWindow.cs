using Ozzyria.MapEditor.EventSystem;
using SFML.Graphics;

namespace Ozzyria.MapEditor
{
    class LayerWindow : GWindow
    {

        public int CurrentLayer { get; set; } = 0;
        public int NumberOfLayers { get; set; } = 1;

        private int mouseX = 0;
        private int mouseY = 0;

        public LayerWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight, int margin, int padding) : base(x, y, width, height, screenWidth, screenHeight, margin, padding)
        {
            EventQueue.Queue(new LayerChangedEvent
            {
                SelectedLayer = CurrentLayer
            });
        }

        public override bool CanHandle(IEvent e)
        {
            return e is MapLoadedEvent
                || base.CanHandle(e);
        }

        public override void Notify(IEvent e)
        {
            base.Notify(e);
            if (e is MapLoadedEvent m)
            {
                OnLoadMap(m);
            }
        }

        public void OnLoadMap(MapLoadedEvent e)
        {
            NumberOfLayers = e.NumberOfLayers;
        }

        public override void OnHorizontalScroll(HorizontalScrollEvent e)
        {
            // not needed
        }

        public override void OnMouseMove(MouseMoveEvent e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        public override void OnVerticalScroll(VerticalScrollEvent e)
        {
            // might needed?
        }

        public override void OnMouseDown(MouseDownEvent e)
        {
            if (!e.LeftMouseDown)
            {
                return;
            }

            OnPickLayer(e.OriginX, e.OriginY);
        }

        public void OnPickLayer(int x, int y)
        {
            var height = 25;
            var width = GetIWidth();
            var left = GetILeft();
            var ii = 0;
            int top;
            for (var i = 0; i < NumberOfLayers; i++)
            {
                top = GetITop() + (i * (height + 5));

                var removeButtonLeft = left + width - (height - 8) - 4;
                if (x >= removeButtonLeft && x < removeButtonLeft + (height - 8) && y >= top + 4 && y < top + 4 + (height - 8))
                {
                    MapManager.RemoveLayer(i);
                    NumberOfLayers = MapManager.GetNumberOfLayers();
                    if (i <= CurrentLayer && (i != CurrentLayer || i >= NumberOfLayers))
                    {
                        CurrentLayer -= 1;
                        EventQueue.Queue(new LayerChangedEvent
                        {
                            SelectedLayer = CurrentLayer
                        });
                    }
                    return;
                }
                else if (x >= left && x < left + width && y >= top && y < top + height)
                {
                    CurrentLayer = i;
                    EventQueue.Queue(new LayerChangedEvent
                    {
                        SelectedLayer = CurrentLayer
                    });
                    return;
                }

                ii = i + 1;
            }

            top = GetITop() + (ii * (height + 5));
            if (x >= left && x < left + width && y >= top && y < top + height)
            {
                MapManager.AddLayer();
                NumberOfLayers = MapManager.GetNumberOfLayers();
            }
        }

        protected override void RenderWindowContents(RenderTarget buffer)
        {
            var font = FontFactory.GetRegular();

            var height = 25;
            var width = GetIWidth();
            var left = GetILeft();

            // TODO... this is a doozy, probably add a 'button class' of some kind?
            var ii = 0;
            int top;
            for (var i = 0; i < NumberOfLayers; i++)
            {
                top = GetITop() + (i * (height + 5));
                buffer.Draw(new RectangleShape()
                {
                    Size = new SFML.System.Vector2f(width, height),
                    Position = new SFML.System.Vector2f(left, top),
                    FillColor = Color.White,
                    OutlineColor = (mouseX >= left && mouseX < left + width && mouseY >= top && mouseY < top + height) ? (CurrentLayer == i ? Colors.HoverSelectedElement() : Colors.HoverElement()) : (CurrentLayer == i ? Colors.SelectedElement() : Colors.DefaultElement()),
                    OutlineThickness = 1
                });
                buffer.Draw(new RectangleShape()
                {
                    Size = new SFML.System.Vector2f(height - 8, height - 8),
                    Position = new SFML.System.Vector2f(left + width - (height - 8) - 4, top + 4),
                    FillColor = Color.Red,
                });
                var text = new Text("Layer #" + i, font)
                {
                    CharacterSize = 12,
                    Position = new SFML.System.Vector2f(left, top),
                    FillColor = Color.Black
                };
                buffer.Draw(text);

                ii = i + 1;
            }

            top = GetITop() + (ii * (height + 5));
            buffer.Draw(new RectangleShape()
            {
                Size = new SFML.System.Vector2f(width, height),
                Position = new SFML.System.Vector2f(left, top),
                FillColor = Color.Green,
                OutlineColor = (mouseX >= left && mouseX < left + width && mouseY >= top && mouseY < top + height) ? Colors.HoverElement() : Colors.DefaultElement(),
                OutlineThickness = 1
            });

            var addText = new Text("Add Layer", font)
            {
                CharacterSize = 12,
                Position = new SFML.System.Vector2f(left, top),
                FillColor = Color.Black
            };
            buffer.Draw(addText);
        }
    }
}
