﻿using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class PaintTool : ITool
    {
        public uint BrushResource { get; set; } = 1;
        public int BrushTextureX { get; set; } = 0;
        public int BrushTextureY { get; set; } = 0;

        protected bool wantsToPaint = false;

        public void OnMouseDown(MouseState mouseState, Camera camera, Map map)
        {
            if(mouseState.IsRightDown)
            {
                wantsToPaint = true;
            }
        }

        public void OnMouseMove(MouseState mouseState, Camera camera, Map map)
        {
        }

        public void OnMouseUp(MouseState mouseState, Camera camera, Map map)
        {
            if(!mouseState.IsRightDown && wantsToPaint)
            {
                wantsToPaint = false;

                var mouseWorldX = camera.ViewToWorld(mouseState.MouseX - camera.ViewX);
                var mouseWorldY = camera.ViewToWorld(mouseState.MouseY - camera.ViewY);
                var mouseTileX = (int)Math.Floor(mouseWorldX / 32);
                var mouseTileY = (int)Math.Floor(mouseWorldY / 32);

                map.PaintArea(new TileData
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
}
