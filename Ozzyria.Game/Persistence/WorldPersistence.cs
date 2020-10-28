using Ozzyria.Game.Component.Attribute;
using Ozzyria.Game.Component;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;

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
                        file.WriteLine($"{layer}|{tile.X}|{tile.Y}|{tile.TextureCoordX}|{tile.TextureCoordY}");
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
            foreach (var component in entity.GetAllComponents())
            {
                writer.Write(false); // signal not done reading entity
                WriteComponent(writer, component);
            }
            writer.Write(true); // signal end-of-entity
        }

        private static void WriteComponent(BinaryWriter writer, Component.Component component)
        {
            var options = GetOptionsAttribute(component);
            if (options == null)
            {
                writer.Write("");
                return;
            }

            writer.Write(options.Name);
            var props = GetSavableProperties(component);
            foreach (var p in props)
            {
                var type = p.PropertyType.IsEnum ? typeof(Enum) :
                    (p.PropertyType.BaseType == typeof(Component.Component) ? typeof(Component.Component) : p.PropertyType);
                WriteValueOfType(writer, type, GetPropertyValue(p, component));
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
            var componentType = reader.ReadString();
            if (!componentTypes.ContainsKey(componentType))
                return null;

            var component = Activator.CreateInstance(componentTypes[componentType]);
            var props = GetSavableProperties(component);

            foreach (var p in props)
            {
                var type = p.PropertyType.IsEnum ? typeof(Enum) :
                    (p.PropertyType.BaseType == typeof(Component.Component) ? typeof(Component.Component) : p.PropertyType);
                SetPropertyValue(p, component, ReadValueOfType(reader, type));
            }
            return (Component.Component)component;
        }

        private static object? GetPropertyValue(PropertyInfo p, object? instance)
        {
            if (!propertyGetters.ContainsKey(instance.GetType()))
            {
                propertyGetters[instance.GetType()] = new Dictionary<string, Func<object, object?>>();
            }
            if (!propertyGetters[instance.GetType()].ContainsKey(p.Name))
            {
                var method = p.GetGetMethod();
                var paramExpress = Expression.Parameter(typeof(object), "instance");
                var instanceCast = !p.DeclaringType.IsValueType
                    ? Expression.TypeAs(paramExpress, p.DeclaringType)
                    : Expression.Convert(paramExpress, p.DeclaringType);

                var expr =
                    Expression.Lambda<Func<object, object?>>(
                        Expression.TypeAs(
                            Expression.Call(instanceCast, method),
                            typeof(object)
                         ),
                        paramExpress);

                propertyGetters[instance.GetType()][p.Name] = expr.Compile();
            }

            return propertyGetters[instance.GetType()][p.Name](instance);
        }

        private static void SetPropertyValue(PropertyInfo p, object instance, object? value)
        {
            if (!propertySetters.ContainsKey(instance.GetType()))
            {
                propertySetters[instance.GetType()] = new Dictionary<string, Delegate>();
            }
            if (!propertySetters[instance.GetType()].ContainsKey(p.Name))
            {
                var i = Expression.Parameter(p.DeclaringType, "i");
                var a = Expression.Parameter(typeof(object), "a");
                var setterCall = Expression.Call(i, p.GetSetMethod(), Expression.Convert(a, p.PropertyType));
                var exp = Expression.Lambda(setterCall, i, a);
                propertySetters[instance.GetType()][p.Name] = exp.Compile();
            }

            propertySetters[instance.GetType()][p.Name].DynamicInvoke(instance, value);
        }

        private static OptionsAttribute GetOptionsAttribute(Component.Component component)
        {
            if (!componentOptions.ContainsKey(component.GetType())) // TODO OZ-12 think on this (see other cache type things)
            {
                componentOptions[component.GetType()] = (OptionsAttribute)component?.GetType().GetCustomAttributes(typeof(OptionsAttribute), false).FirstOrDefault();
            }

            return componentOptions[component.GetType()];
        }

        private static PropertyInfo[] GetSavableProperties(object o)
        {
            if (!componentProperties.ContainsKey(o.GetType())) // TODO OZ-12 make thing 'component name' centric (name from Options)
            {
                componentProperties[o.GetType()] = o.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(SavableAttribute)))
                .OrderBy(p => p.Name)
                .ToArray();
            }

            return componentProperties[o.GetType()];
        }

        private static void WriteValueOfType(BinaryWriter writer, Type type, object? value)
        {
            if (supportedWriteTypes.Count == 0)
            {
                // TODO OZ-12 instantiate dictionaries and such togo superfast
            }

            supportedWriteTypes[type](writer, value);
        }

        private static object? ReadValueOfType(BinaryReader reader, Type type)
        {
            if (supportedReadTypes.Count == 0)
            {
                // TODO OZ-12 instantiate dictionaries and such togo superfast
            }

            return supportedReadTypes[type](reader);
        }

        private static Dictionary<string, Type> componentTypes = new Dictionary<string, Type>{
            {"BoundingBox", typeof(BoundingBox) },
            {"BoundingCircle", typeof(BoundingCircle) },
            {"Combat", typeof(Combat) },
            {"Delay", typeof(Delay) },
            {"ExperienceBoost", typeof(ExperienceBoost) },
            {"Input", typeof(Input) },
            {"Movement", typeof(Movement) },
            {"Renderable", typeof(Renderable) },
            {"Stats", typeof(Stats) },
            {"ExperienceOrbThought", typeof(ExperienceOrbThought) },
            {"PlayerThought", typeof(PlayerThought) },
            {"SlimeThought", typeof(SlimeThought) },
        }; // TODO OZ-12 build this on instantiation / boot of program to avoid all the reflection slowness

        private static Dictionary<Type, PropertyInfo[]> componentProperties = new Dictionary<Type, PropertyInfo[]>();
        private static Dictionary<Type, OptionsAttribute> componentOptions = new Dictionary<Type, OptionsAttribute>();
        private static Dictionary<Type, Dictionary<string, Func<object, object?>>> propertyGetters = new Dictionary<Type, Dictionary<string, Func<object, object?>>>();
        private static Dictionary<Type, Dictionary<string, Delegate>> propertySetters = new Dictionary<Type, Dictionary<string, Delegate>>();

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
