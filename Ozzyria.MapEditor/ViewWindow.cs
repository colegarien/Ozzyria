using Ozzyria.MapEditor.EventSystem;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Ozzyria.MapEditor
{
    class ViewWindow : GWindow
    {
        private const float hScrollSensitivity = 5f;
        private const float vScrollSensitivity = 5f;
        private const float zoomSensitivity = 0.01f;

        private float xOffset = 0f;
        private float yOffset = 0f;
        public float zoomPercent = 1f;

        private RenderTexture _renderBuffer; // for rendering window contents

        private float cursorScreenX = 0;
        private float cursorScreenY = 0;
        public int Layer { get; set; } = 0;
        public TileType SelectedBrush { get; set; }
        public ToolType SelectedTool { get; set; }

        public ViewWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight, int margin, int padding) : base(x, y, width, height, screenWidth, screenHeight, margin, padding)
        {
        }

        public override bool CanHandle(IEvent e)
        {
            return e is MapLoadedEvent
                || e is LayerChangedEvent
                || e is BrushTypeChangeEvent
                || e is ToolTypeChangeEvent
                || base.CanHandle(e);
        }

        public override void Notify(IEvent e)
        {
            base.Notify(e);
            if (e is ZoomEvent z)
            {
                OnZoom(z);
            }
            else if (e is MouseDragEvent m)
            {
                if (m.MiddleMouseDown)
                {
                    OnPan(m.DeltaX, m.DeltaY);
                }
                else if (m.LeftMouseDown)
                {
                    OnPaint(m.X, m.Y);
                }
            }
            else if (e is MapLoadedEvent l)
            {
                OnLoadMap(l);
            }
            else if (e is LayerChangedEvent c)
            {
                Layer = c.SelectedLayer;
            }
            else if (e is BrushTypeChangeEvent b)
            {
                SelectedBrush = b.SelectedBrush;
            }
            else if (e is ToolTypeChangeEvent t)
            {
                SelectedTool = t.SelectedTool;
            }
        }

        public void OnLoadMap(MapLoadedEvent e)
        {
            if (_renderBuffer != null)
            {
                _renderBuffer.Dispose();
            }

            _renderBuffer = new RenderTexture((uint)(e.Width * e.TileDimension), (uint)(e.Height * e.TileDimension));
            CenterView();
        }

        public override void OnResize(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight)
        {
            base.OnResize(x, y, width, height, screenWidth, screenHeight);

            // Center on map
            CenterView();
        }

        private void CenterView()
        {
            if (_renderBuffer == null)
            {
                xOffset = 0;
                yOffset = 0;
                zoomPercent = 1f;
                return;
            }

            // center in window based
            zoomPercent = 1f;
            xOffset = (_renderBuffer.Size.X * 0.5f) - GetCenterX();
            yOffset = (_renderBuffer.Size.Y * 0.5f) - GetCenterY();

            // biggest dimension should take of 88% of the screen (cause it look nice)
            var newZoom = (0.88f * GetWidth()) / _renderBuffer.Size.X;
            if (GetHeight() < GetWidth())
            {
                newZoom = (0.88f * GetHeight()) / _renderBuffer.Size.Y;
            }

            ZoomTo(GetCenterX(), GetCenterY(), newZoom);
        }

        public void OnPan(float deltaX, float deltaY)
        {
            xOffset -= deltaX / zoomPercent;
            yOffset -= deltaY / zoomPercent;
        }

        public override void OnMouseDown(MouseDownEvent e)
        {
            if (!e.LeftMouseDown)
            {
                return;
            }

            OnPaint(e.OriginX, e.OriginY);
        }

        public void OnPaint(int x, int y)
        {
            var tileDimension = MapManager.GetTileDimension();
            if (SelectedTool == ToolType.Pencil)
            {
                MapManager.PaintTile(Layer, (int)Math.Floor(ScreenToWorldX(x) / tileDimension), (int)Math.Floor(ScreenToWorldY(y) / tileDimension), SelectedBrush);
            }
            else if (SelectedTool == ToolType.Fill)
            {
                MapManager.FillTile(Layer, (int)Math.Floor(ScreenToWorldX(x) / tileDimension), (int)Math.Floor(ScreenToWorldY(y) / tileDimension), SelectedBrush);
            }
        }

        public override void OnHorizontalScroll(HorizontalScrollEvent e)
        {
            xOffset += (e.Delta / zoomPercent) * hScrollSensitivity;
        }

        public override void OnVerticalScroll(VerticalScrollEvent e)
        {
            yOffset -= (e.Delta / zoomPercent) * vScrollSensitivity;
        }

        public override void OnMouseMove(MouseMoveEvent e)
        {
            cursorScreenX = e.X;
            cursorScreenY = e.Y;
        }

        public void OnZoom(ZoomEvent e)
        {
            var scale = (e.Delta > 0)
                ? zoomSensitivity
                : -zoomSensitivity;
            var targetZoomPercent = zoomPercent * (1 + scale);

            ZoomTo(e.OriginX, e.OriginY, targetZoomPercent);
        }

        private void ZoomTo(int xOrigin, int yOrigin, float targetZoomPercent)
        {
            var previousWorldXOrigin = ScreenToWorldX(xOrigin);
            var previousWorldYOrigin = ScreenToWorldY(yOrigin);

            zoomPercent = targetZoomPercent;
            if (zoomPercent < 0.05f)
            {
                zoomPercent = 0.05f;
            }
            else if (zoomPercent > 10f)
            {
                zoomPercent = 10f;
            }

            var currentWorldXOrigin = ScreenToWorldX(xOrigin);
            var currentWorldYOrigin = ScreenToWorldY(yOrigin);

            xOffset += previousWorldXOrigin - currentWorldXOrigin;
            yOffset += previousWorldYOrigin - currentWorldYOrigin;
        }

        private float ScreenToWorldX(float screenX)
        {
            return (screenX / zoomPercent) + xOffset;
        }
        private float ScreenToWorldY(float screenY)
        {
            return (screenY / zoomPercent) + yOffset;
        }

        private float WorldToScreenX(float worldX)
        {
            return (worldX - xOffset) * zoomPercent;
        }
        private float WorldToScreenY(float worldY)
        {
            return (worldY - yOffset) * zoomPercent;
        }

        protected override void RenderWindowContents(RenderTarget buffer)
        {
            if (_renderBuffer == null)
            {
                return;
            }

            _renderBuffer.Clear();
            var tileDimension = MapManager.GetTileDimension();

            var mapWidth = MapManager.GetWidth();
            var mapHeight = MapManager.GetHeight();
            var tileSize = new Vector2f(tileDimension, tileDimension);
            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    var tileType = MapManager.GetTileType(Layer, x, y);
                    var tilePosition = new Vector2f((x * tileDimension), (y * tileDimension));
                    for (var i = 1; i <= 3; i++)
                    {
                        if (Layer - i >= 0)
                        {
                            var rawColor = Colors.TileColor(MapManager.GetTileType(Layer - i, x, y));
                            rawColor.A = (byte)(rawColor.A * (1 - (0.25 * i)));
                            var tileBackLayerShape = new RectangleShape(tileSize)
                            {
                                Position = tilePosition,
                                FillColor = rawColor
                            };
                            _renderBuffer.Draw(tileBackLayerShape);
                        }
                    }

                    if (tileType != TileType.None)
                    {
                        var tileShape = new RectangleShape(tileSize)
                        {
                            Position = tilePosition,
                            FillColor = Colors.TileColor(tileType)
                        };
                        _renderBuffer.Draw(tileShape);

                        DrawPathDirection(tilePosition.X, tilePosition.Y, tileDimension, tileDimension, MapManager.GetPathDirection(Layer, x, y));
                        DrawTransitionType(tilePosition.X, tilePosition.Y, tileDimension, tileDimension, MapManager.GetEdgeTransitionType(Layer, x, y), MapManager.GetCornerTransitionType(Layer, x, y));
                    }
                }
            }

            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    var overlayBorder = new RectangleShape(tileSize)
                    {
                        Position = new Vector2f((x * tileDimension), (y * tileDimension)),
                        FillColor = Color.Transparent,
                        OutlineThickness = 2,
                        OutlineColor = new Color(140, 140, 140)
                    };

                    _renderBuffer.Draw(overlayBorder);
                }
            }

            var cursorShape = new RectangleShape(new Vector2f(tileDimension - 2, tileDimension - 2))
            {
                Position = new Vector2f(((int)Math.Floor(ScreenToWorldX(cursorScreenX) / tileDimension) * tileDimension) + 1, ((int)Math.Floor(ScreenToWorldY(cursorScreenY) / tileDimension) * tileDimension) + 1),
                FillColor = Color.Transparent,
                OutlineThickness = 1,
                OutlineColor = Colors.HoverElement()
            };
            _renderBuffer.Draw(cursorShape);

            _renderBuffer.Display();

            // draw map
            buffer.Draw(new Sprite(_renderBuffer.Texture)
            {
                Position = new Vector2f(WorldToScreenX(0), WorldToScreenY(0)),
                Scale = new Vector2f(zoomPercent, zoomPercent)
            });
        }

        private void DrawPathDirection(float x, float y, int width, int height, PathDirection direction)
        {
            var pathDimension = 6f;
            var horizontalSize = new Vector2f(width / 2f, pathDimension);
            var verticalSize = new Vector2f(pathDimension, height / 2f);

            var horizontalY = y + (height / 2f) - (horizontalSize.Y / 2f);
            var verticalY = x + (width / 2f) - (verticalSize.X / 2f);

            if (direction != PathDirection.None)
            {
                var centerShape = new RectangleShape()
                {
                    Position = new Vector2f(verticalY, horizontalY),
                    Size = new Vector2f(pathDimension, pathDimension),
                    FillColor = Color.White
                };
                _renderBuffer.Draw(centerShape);


                if (direction == PathDirection.Up
                    || direction == PathDirection.UpDown
                    || direction == PathDirection.UpLeft
                    || direction == PathDirection.UpRight
                    || direction == PathDirection.UpT
                    || direction == PathDirection.LeftT
                    || direction == PathDirection.RightT
                    || direction == PathDirection.All)
                {
                    var shape = new RectangleShape()
                    {
                        Position = new Vector2f(verticalY, y),
                        Size = verticalSize,
                        FillColor = Color.White
                    };
                    _renderBuffer.Draw(shape);
                }

                if (direction == PathDirection.Down
                    || direction == PathDirection.DownLeft
                    || direction == PathDirection.DownRight
                    || direction == PathDirection.DownT
                    || direction == PathDirection.UpDown
                    || direction == PathDirection.LeftT
                    || direction == PathDirection.RightT
                    || direction == PathDirection.All)
                {
                    var shape = new RectangleShape()
                    {
                        Position = new Vector2f(verticalY, y + verticalSize.Y),
                        Size = verticalSize,
                        FillColor = Color.White
                    };
                    _renderBuffer.Draw(shape);
                }

                if (direction == PathDirection.Left
                    || direction == PathDirection.LeftRight
                    || direction == PathDirection.LeftT
                    || direction == PathDirection.UpT
                    || direction == PathDirection.DownT
                    || direction == PathDirection.DownLeft
                    || direction == PathDirection.UpLeft
                    || direction == PathDirection.All)
                {
                    var shape = new RectangleShape()
                    {
                        Position = new Vector2f(x, horizontalY),
                        Size = horizontalSize,
                        FillColor = Color.White
                    };
                    _renderBuffer.Draw(shape);
                }

                if (direction == PathDirection.Right
                    || direction == PathDirection.RightT
                    || direction == PathDirection.DownT
                    || direction == PathDirection.UpT
                    || direction == PathDirection.DownRight
                    || direction == PathDirection.LeftRight
                    || direction == PathDirection.UpRight
                    || direction == PathDirection.All)
                {
                    var shape = new RectangleShape()
                    {
                        Position = new Vector2f(x + horizontalSize.X, horizontalY),
                        Size = horizontalSize,
                        FillColor = Color.White
                    };
                    _renderBuffer.Draw(shape);
                }
            }
        }

        private void DrawTransitionType(float x, float y, int width, int height, IDictionary<TileType, EdgeTransitionType> edgeTransitions, IDictionary<TileType, CornerTransitionType> cornerTransitions)
        {
            var transitionDimension = 8f;
            var size = new Vector2f(transitionDimension, transitionDimension);

            var horizontalCenter = x + (width / 2f) - (transitionDimension / 2f);
            var verticalCenter = y + (height / 2f) - (transitionDimension / 2f);
            var right = x + width - transitionDimension;
            var bottom = y + height - transitionDimension;

            foreach (var kv in edgeTransitions)
            {
                var color = Color.White; // OZ-19 : base color on tile type
                var edgeType = kv.Value;
                if (edgeType != EdgeTransitionType.None)
                {
                    // Use the bit-mask math to see if it 'bit' is on or not
                    if (((int)edgeType & (1 << 0)) > 0)
                    {
                        var shape = new RectangleShape()
                        {
                            Position = new Vector2f(x, verticalCenter),
                            Size = size,
                            FillColor = color
                        };
                        _renderBuffer.Draw(shape);
                    }

                    if (((int)edgeType & (1 << 1)) > 0)
                    {
                        var shape = new RectangleShape()
                        {
                            Position = new Vector2f(horizontalCenter, y),
                            Size = size,
                            FillColor = color
                        };
                        _renderBuffer.Draw(shape);
                    }

                    if (((int)edgeType & (1 << 2)) > 0)
                    {
                        var shape = new RectangleShape()
                        {
                            Position = new Vector2f(right, verticalCenter),
                            Size = size,
                            FillColor = color
                        };
                        _renderBuffer.Draw(shape);
                    }

                    if (((int)edgeType & (1 << 3)) > 0)
                    {
                        var shape = new RectangleShape()
                        {
                            Position = new Vector2f(horizontalCenter, bottom),
                            Size = size,
                            FillColor = color
                        };
                        _renderBuffer.Draw(shape);
                    }
                }
            }

            foreach (var kv in cornerTransitions)
            {
                var color = Color.White; // OZ-19 : base color on tile type
                var cornerType = kv.Value;
                if (cornerType != CornerTransitionType.None)
                {
                    // Use the bit-mask math to see if it 'bit' is on or not
                    if (((int)cornerType & (1 << 0)) > 0)
                    {
                        var shape = new RectangleShape()
                        {
                            Position = new Vector2f(x, y),
                            Size = size,
                            FillColor = color
                        };
                        _renderBuffer.Draw(shape);
                    }

                    if (((int)cornerType & (1 << 1)) > 0)
                    {
                        var shape = new RectangleShape()
                        {
                            Position = new Vector2f(right, y),
                            Size = size,
                            FillColor = color
                        };
                        _renderBuffer.Draw(shape);
                    }


                    if (((int)cornerType & (1 << 2)) > 0)
                    {
                        var shape = new RectangleShape()
                        {
                            Position = new Vector2f(right, bottom),
                            Size = size,
                            FillColor = color
                        };
                        _renderBuffer.Draw(shape);
                    }


                    if (((int)cornerType & (1 << 3)) > 0)
                    {
                        var shape = new RectangleShape()
                        {
                            Position = new Vector2f(x, bottom),
                            Size = size,
                            FillColor = color
                        };
                        _renderBuffer.Draw(shape);
                    }
                }
            }
        }

    }
}
