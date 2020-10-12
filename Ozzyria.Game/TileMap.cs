using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Ozzyria.Game
{
    public class Tile
    {
        public const int DIMENSION = 32;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int TextureCoordX { get; set; } = 0;
        public int TextureCoordY { get; set; } = 0;
    }

    public class TileMap
    {
        // TODO make these not public
        public int width = 32;
        public int height = 32;

        public IDictionary<int, List<Tile>> layers;

        public TileMap()
        {
            layers = new Dictionary<int, List<Tile>>();
            using (System.IO.StreamReader file = new System.IO.StreamReader("Maps\\test.ozz")) // TODO not hardcode this
            {
                width = int.Parse(file.ReadLine());
                height = int.Parse(file.ReadLine());
                var numberOfLayers = int.Parse(file.ReadLine());
                for(var i = 0; i < numberOfLayers; i++)
                {
                    layers[i] = new List<Tile>();
                }

                string line;
                while((line = file.ReadLine().Trim()) != "" && line != "END")
                {
                    var pieces = line.Split("|");
                    if(pieces.Length != 5)
                    {
                        continue;
                    }

                    var layer = int.Parse(pieces[0]);
                    var x = int.Parse(pieces[1]);
                    var y = int.Parse(pieces[2]);
                    var tx = int.Parse(pieces[3]);
                    var ty = int.Parse(pieces[4]);

                    layers[layer].Add(new Tile
                    {
                        X = x,
                        Y = y,
                        TextureCoordX = tx,
                        TextureCoordY = ty
                    });
                }                
            }
        }
    }
}
