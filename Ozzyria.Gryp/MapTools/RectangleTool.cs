using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class RectangleTool : IAreaTool
    {
        public int Stroke { get; set; } = 0;

        protected override void OnCancel(MouseState mouseState, Camera camera, Map map)
        {
            // no-op
        }

        protected override void OnComplete(MouseState mouseState, Camera camera, Map map)
        {
            var tileArea = GetTileArea();

            for (int tileX = tileArea.TileX; tileX < tileArea.TileX + tileArea.TileWidth; tileX++)
            {
                for (int tileY = tileArea.TileY; tileY < tileArea.TileY + tileArea.TileHeight; tileY++)
                {
                    // Only paint around stroke
                    if (tileY - tileArea.TileY <= Stroke || (tileArea.TileY + tileArea.TileHeight - 1) - tileY <= Stroke || tileX - tileArea.TileX <= Stroke || (tileArea.TileX + tileArea.TileWidth - 1) - tileX <= Stroke)
                    {
                        var tileData = new TileData();
                        tileData.Images.AddRange(map.CurrentBrush);
                        map.PushTile(tileData, tileX, tileY);
                    }
                }
            }
        }
    }
}
