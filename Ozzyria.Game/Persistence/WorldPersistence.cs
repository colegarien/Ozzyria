using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ozzyria.Game.Persistence
{
    public class WorldPersistence
    {

        public TileMap LoadMap(string resource)
        {
            int width = 0, height = 0;
            var layers = new Dictionary<int, List<Tile>>();
            using (System.IO.StreamReader file = new System.IO.StreamReader("Maps\\" + resource + ".ozz"))
            {
                width = int.Parse(file.ReadLine());
                height = int.Parse(file.ReadLine());
                var numberOfLayers = int.Parse(file.ReadLine());
                for (var i = 0; i < numberOfLayers; i++)
                {
                    layers[i] = new List<Tile>();
                }

                string line;
                while ((line = file.ReadLine().Trim()) != "" && line != "END")
                {
                    var pieces = line.Split("|");
                    if (pieces.Length != 5)
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

            return new TileMap
            {
                Width = width,
                Height = height,
                Layers = layers
            };
        }

        public void SaveMap(string resource, TileMap map)
        {
            var baseMapsDirectory = @"C:\Users\cgari\source\repos\Ozzyria\Ozzyria.Game\Maps"; // TODO this is just to make debuggery easier for now
            if (!System.IO.Directory.Exists(baseMapsDirectory))
            {
                System.IO.Directory.CreateDirectory(baseMapsDirectory);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(baseMapsDirectory + "\\" + resource + ".ozz"))
            {
                file.WriteLine(map.Width);
                file.WriteLine(map.Height);
                file.WriteLine(map.Layers.Keys.Count);
                for (var layer = 0; layer < map.Layers.Keys.Count; layer++)
                {
                    foreach (var tile in map.Layers[layer])
                    {
                        file.WriteLine($"{layer}|{tile.X}|{tile.Y}|{tile.TextureCoordX}|{tile.TextureCoordY}");
                    }
                }
                file.WriteLine("END");
            }
        }

        public EntityManager LoadEntityManager(string resource)
        {
            var entityManager = new EntityManager();
            var options = GetOptions();
            using (System.IO.StreamReader file = new System.IO.StreamReader("Maps\\" + resource + ".ozz"))
            {
                Entity currentEntity = new Entity();

                string line;
                while ((line = file.ReadLine().Trim()) != "" && line != "END")
                {
                    if (line == "!---")
                    {
                        currentEntity = new Entity();
                    }
                    else if (line == "---!")
                    {
                        entityManager.Register(currentEntity);
                    }
                    else
                    {
                        var data = file.ReadLine().Trim();
                        var type = Type.GetType(line);
                        currentEntity.AttachComponent((Component.Component)JsonSerializer.Deserialize(data, type, options));
                    }
                }
            }

            return entityManager;
        }

        public void SaveEntityManager(string resource, EntityManager entityManager)
        {
            var baseMapsDirectory = @"C:\Users\cgari\source\repos\Ozzyria\Ozzyria.Game\Maps"; // TODO this is just to make debuggery easier for now
            if (!System.IO.Directory.Exists(baseMapsDirectory))
            {
                System.IO.Directory.CreateDirectory(baseMapsDirectory);
            }

            var options = GetOptions();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(baseMapsDirectory + "\\" + resource + ".ozz"))
            {
                foreach (var entity in entityManager.GetEntities())
                {
                    file.WriteLine("!---");
                    foreach (var component in entity.GetAllComponents())
                    {
                        file.WriteLine(component.GetType());
                        file.WriteLine(JsonSerializer.Serialize(component, component.GetType(), options));
                    }
                    file.WriteLine("---!");
                }
                file.WriteLine("END");
            }
        }

        private JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions();
        }

    }
}
