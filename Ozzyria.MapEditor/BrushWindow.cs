﻿using Ozzyria.MapEditor.EventSystem;
using SFML.Graphics;
using System;

namespace Ozzyria.MapEditor
{
    class BrushWindow : GWindow
    {
        public TileType SelectedBrush { get; set; } = TileType.Ground;
        private int mouseX = 0;
        private int mouseY = 0;


        public BrushWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight) : base(x, y, width, height, screenWidth, screenHeight)
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
            foreach (TileType type in Enum.GetValues(typeof(TileType)))
            {
                var left = windowX + 10 + (i * 37); // TODO this is gross and copy+pasted from Draw, should make some kinda Button Class?
                var top = windowY + 10;
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
            foreach(TileType type in Enum.GetValues(typeof(TileType)))
            {
                var color = Color.Green;
                switch (type)
                {
                    case TileType.None:
                        color = Color.Black;
                        break;
                    case TileType.Water:
                        color = Color.Blue;
                        break;
                    case TileType.Fence:
                        color = Color.Red;
                        break;
                    case TileType.Ground:
                    default:
                        color = Color.Green;
                        break;
                }

                var left = windowX + 10 + (i * 37);
                var top = windowY + 10;
                var dimension = 32;
                buffer.Draw(new RectangleShape()
                {
                    Size = new SFML.System.Vector2f(dimension, dimension),
                    Position = new SFML.System.Vector2f(left, top),
                    FillColor = color,
                });

                var outlineColor = Color.White; // not hovered over and not selected
                if(mouseX >= left && mouseX < left+dimension
                    && mouseY >= top && mouseY < top + dimension)
                {
                    if (type == SelectedBrush)
                    {
                        outlineColor = Color.Yellow; // current selected and hovered over
                    }
                    else
                    {
                        outlineColor = Color.Cyan; // just hovered over
                    }
                }else if (type == SelectedBrush)
                {
                    outlineColor = Color.Magenta; // currently selected and not hovered over
                }

                buffer.Draw(new RectangleShape()
                {
                    Size = new SFML.System.Vector2f(32, 32),
                    Position = new SFML.System.Vector2f(windowX + 10 + (i * 37), windowY + 10),
                    FillColor = Color.Transparent,
                    OutlineColor = outlineColor,
                    OutlineThickness = 2
                });

                i++;
            }
        }
    }
}
