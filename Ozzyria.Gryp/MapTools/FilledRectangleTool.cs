using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class FilledRectangleTool : IAreaTool
    {
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
                    var tileData = new TileData();
                    tileData.DrawableIds.AddRange(map.CurrentBrush);
                    map.PushTile(tileData, tileX, tileY);
                }
            }
        }
    }
}
