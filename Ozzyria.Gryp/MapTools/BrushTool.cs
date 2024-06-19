using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class BrushTool : ITool
    {
        public uint BrushResource { get; set; } = 1;
        public int BrushTextureX { get; set; } = 0;
        public int BrushTextureY { get; set; } = 0;


        public void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            // nothing special to be done
        }

        public void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
            if(mouseState.IsLeftDown)
            {
                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                if (map.Layers.Count > 0)
                {
                    map.PushTile(new TileData
                    {
                        Images = new List<TextureCoords>() {
                            new TextureCoords()
                            {
                                Resource = BrushResource,
                                TextureX = BrushTextureX,
                                TextureY = BrushTextureY,
                            }
                        },
                    }, mouseTileX, mouseTileY);
                }
            }
        }

        public void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            // nothing special to be done
        }
    }
}
