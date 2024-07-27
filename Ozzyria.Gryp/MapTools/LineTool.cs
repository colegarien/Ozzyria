using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class LineTool : ITool
    {
        public uint BrushResource { get; set; } = 1;
        public int BrushTextureX { get; set; } = 0;
        public int BrushTextureY { get; set; } = 0;

        private bool lining = false;
        public float LineStartX, LineStartY, LineEndX, LineEndY;

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && !lining)
            {
                lining = true;

                LineStartX = LineEndX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                LineStartY = LineEndY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
            }
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if(mouseState.IsLeftDown && lining)
            {
                LineEndX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                LineEndY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
            }
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            if (!mouseState.IsLeftDown && lining)
            {
                lining = false;

                LineEndX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                LineEndY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);

                var tileX1 = (int)Math.Floor(LineStartX / 32);
                var tileY1 = (int)Math.Floor(LineStartY / 32);
                var tileX2 = (int)Math.Floor(LineEndX / 32);
                var tileY2 = (int)Math.Floor(LineEndY / 32);

                foreach (var point in bresenham(tileX1, tileY1, tileX2, tileY2))
                {
                    var tileData = new Tile();
                    tileData.DrawableIds.AddRange(map.CurrentBrush);
                    map.PushTile(tileData, point.Item1, point.Item2);
                }


                LineStartX = LineEndX = 0;
                LineStartY = LineEndY = 0;
            }
        }

        private List<Tuple<int, int>> bresenham(int x1, int y1, int x2, int y2)
        {
            var points = new List<Tuple<int, int>>();

            int dx = Math.Abs(x2 - x1);
            int sx = x1 < x2 ? 1 : -1;
            int dy = -Math.Abs(y2 - y1);
            int sy = y1 < y2 ? 1 : -1;
            int error = dx + dy;

            while (true) {
                points.Add(Tuple.Create(x1, y1));
                if (x1 == x2 && y1 == y2) break;
                int e2 = 2 * error;

                if (e2 >= dy) {
                    if (x1 == x2) break;
                    error = error + dy;
                    x1 = x1 + sx;
                }
                if (e2 <= dx)
                {
                    if (y1 == y2) break;
                    error = error + dx;
                    y1 = y1 + sy;
                }
            }

            return points;
        }
    }
}
