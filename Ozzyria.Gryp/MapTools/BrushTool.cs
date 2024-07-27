using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class BrushTool : ITool
    {
        bool isBrushing = false;
        bool isErasing = true;

        public override void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if(mouseState.IsLeftDown && !isBrushing)
            {
                isBrushing = true;
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                var tileData = new TileData();
                tileData.DrawableIds.AddRange(map.CurrentBrush);
                map.PushTile(tileData, mouseTileX, mouseTileY);
            }
            else if(mouseState.IsRightDown && !isErasing)
            {
                isErasing = true;
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                // erase tile data
                map.PushTile(new TileData { DrawableIds = new List<string>() { }, }, mouseTileX, mouseTileY);
            }
        }

        public override void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if (mouseState.IsLeftDown && isBrushing)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                var tileData = new TileData();
                tileData.DrawableIds.AddRange(map.CurrentBrush);
                map.PushTile(tileData, mouseTileX, mouseTileY);
            }
            else if (mouseState.IsRightDown && isErasing)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                // erase tile data
                map.PushTile(new TileData {DrawableIds = new List<string>() {}, }, mouseTileX, mouseTileY);
            }
        }

        public override void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            if (!mouseState.IsLeftDown && isBrushing)
            {
                isBrushing = false;
            }

            if (!mouseState.IsRightDown && isErasing)
            {
                isErasing = false;
            }
        }
    }
}
