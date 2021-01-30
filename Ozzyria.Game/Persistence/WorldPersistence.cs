using System;
using System.Collections.Generic;
using System.IO;

namespace Ozzyria.Game.Persistence
{
    public class WorldPersistence
    {

        public TileMap LoadMap(string resource)
        {
            int width = 0, height = 0;
            var layers = new Dictionary<int, List<Tile>>();
            using (StreamReader file = new StreamReader("Maps\\" + resource + ".ozz"))
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
                    if (pieces.Length < 7)
                    {
                        continue;
                    }

                    var layer = int.Parse(pieces[0]);
                    var x = int.Parse(pieces[1]);
                    var y = int.Parse(pieces[2]);
                    var z = int.Parse(pieces[3]);
                    var tx = int.Parse(pieces[4]);
                    var ty = int.Parse(pieces[5]);

                    var decals = new TileDecal[int.Parse(pieces[6])];
                    for(var i = 0; i < decals.Length; i++)
                    {
                        decals[i] = new TileDecal
                        {
                            TextureCoordX = int.Parse(pieces[7 + (i*2)]),
                            TextureCoordY = int.Parse(pieces[7 + (i*2) + 1]),
                        };
                    }

                    layers[layer].Add(new Tile
                    {
                        X = x,
                        Y = y,
                        Z = z,
                        TextureCoordX = tx,
                        TextureCoordY = ty,
                        Decals = decals
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
            if (!Directory.Exists(baseMapsDirectory))
            {
                Directory.CreateDirectory(baseMapsDirectory);
            }

            using (StreamWriter file = new StreamWriter(baseMapsDirectory + "\\" + resource + ".ozz"))
            {
                file.WriteLine(map.Width);
                file.WriteLine(map.Height);
                file.WriteLine(map.Layers.Keys.Count);
                for (var layer = 0; layer < map.Layers.Keys.Count; layer++)
                {
                    foreach (var tile in map.Layers[layer])
                    {
                        var serializedTile = $"{layer}|{tile.X}|{tile.Y}|{tile.Z}|{tile.TextureCoordX}|{tile.TextureCoordY}|{tile.Decals.Length}";
                        foreach(var decal in tile.Decals)
                        {
                            serializedTile += $"|{decal.TextureCoordX}|{decal.TextureCoordY}";
                        }
                        file.WriteLine(serializedTile);
                    }
                }
                file.WriteLine("END");
            }
        }

        public EntityManager LoadEntityManager(string resource)
        {
            var entityManager = new EntityManager();
            using (BinaryReader reader = new BinaryReader(File.OpenRead("Maps\\" + resource + ".ozz")))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    entityManager.Register(ReadEntity(reader));
                }
            }

            return entityManager;
        }

        public void SaveEntityManager(string resource, EntityManager entityManager)
        {
            var baseMapsDirectory = @"C:\Users\cgari\source\repos\Ozzyria\Ozzyria.Game\Maps"; // TODO this is just to make debuggery easier for now
            if (!Directory.Exists(baseMapsDirectory))
            {
                Directory.CreateDirectory(baseMapsDirectory);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(baseMapsDirectory + "\\" + resource + ".ozz", FileMode.Create)))
            {
                foreach (var entity in entityManager.GetEntities())
                {
                    WriteEntity(writer, entity);
                }
            }
        }


        public static void WriteEntity(BinaryWriter writer, Entity entity)
        {
            writer.Write(entity.Id);
            foreach (var component in entity.GetAllComponents()) // TODO consider the fact that adding/removing property can cause old save to break, think of how to communication what parts of the components were actually saved | could possible just write property name first?
            {
                writer.Write(false); // signal not done reading entity
                WriteComponent(writer, component);
            }
            writer.Write(true); // signal end-of-entity
        }

        private static void WriteComponent(BinaryWriter writer, Component.Component component)
        {
            var options = Reflector.GetOptionsAttribute(component?.GetType());
            if (options == null)
            {
                writer.Write("");
                return;
            }

            writer.Write(options.Name);
            var props = Reflector.GetSavableProperties(component.GetType());
            foreach (var p in props)
            {
                WriteValueOfType(writer, GetSerializableBaseType(p.PropertyType), Reflector.GetPropertyValue(p, component));
            }
        }

        public static Entity ReadEntity(BinaryReader reader)
        {
            var entity = new Entity
            {
                Id = reader.ReadInt32(),
            };
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var isEndOfEntity = reader.ReadBoolean();
                if (isEndOfEntity)
                    break;

                entity.AttachComponent(ReadComponent(reader));
            }

            return entity;
        }

        private static Component.Component ReadComponent(BinaryReader reader)
        {
            var component = Reflector.CreateInstance(reader.ReadString());
            if (component == null)
                return null;

            var props = Reflector.GetSavableProperties(component.GetType());
            foreach (var p in props)
            {
                Reflector.SetPropertyValue(p, component, ReadValueOfType(reader, GetSerializableBaseType(p.PropertyType)));
            }
            return (Component.Component)component;
        }

        private static Type GetSerializableBaseType(Type type) // TODO abstract binary read/write possibly once have dependency injection
        {
            if (type.IsEnum)
                return typeof(Enum);
            else if (type.BaseType == typeof(Component.Component))
                return type.BaseType;
            else
                return type;
        }

        private static void WriteValueOfType(BinaryWriter writer, Type type, object? value) // TODO abstract binary read/write possibly once have dependency injection
        {
            supportedWriteTypes[type](writer, value);
        }

        private static object? ReadValueOfType(BinaryReader reader, Type type) // TODO abstract binary read/write possibly once have dependency injection
        {
            return supportedReadTypes[type](reader);
        }

        private static Dictionary<Type, Func<BinaryReader, object>> supportedReadTypes = new Dictionary<Type, Func<BinaryReader, object>>
        {
            { typeof(int), br => br.ReadInt32() },
            { typeof(bool), br => br.ReadBoolean() },
            { typeof(float), br => br.ReadSingle() },
            { typeof(string), br => br.ReadString() },
            { typeof(Enum), br => br.ReadInt32() },
            { typeof(Component.Component), br => ReadComponent(br) },
        };

        private static Dictionary<Type, Action<BinaryWriter, object?>> supportedWriteTypes = new Dictionary<Type, Action<BinaryWriter, object?>>
        {
            { typeof(int), (bw, value) => bw.Write((int)value) },
            { typeof(bool), (bw, value) => bw.Write((bool)value) },
            { typeof(float), (bw, value) => bw.Write((float)value) },
            { typeof(string), (bw, value) => bw.Write((string)value) },
            { typeof(Enum), (bw, value) => bw.Write((int)value) },
            { typeof(Component.Component), (bw, value) => WriteComponent(bw, (Component.Component)value) },
        };

    }
}
