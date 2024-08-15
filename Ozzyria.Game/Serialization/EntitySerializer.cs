using Grecs;
using Ozzyria.Model.Components;
using Ozzyria.Model.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ozzyria.Game.Serialization
{
    public class EntitySerializer
    {
        private static bool _componentTypesInitialized = false;
        private static Dictionary<string, Type> _componentTypes = new Dictionary<string, Type>();

        public static void WriteEntity(BinaryWriter writer, Entity entity)
        {
            writer.Write(entity.id);

            var components = entity.GetComponents();
            writer.Write(components.Length);
            foreach (var component in entity.GetComponents())
            {
                WriteComponent(entity, writer, component);
            }
        }

        public static void WriteDetachedEntity(BinaryWriter writer, Entity entity)
        {
            var components = entity.GetComponents();
            writer.Write(components.Length);
            foreach (var component in entity.GetComponents())
            {
                WriteComponent(entity, writer, component);
            }
        }


        private static void WriteComponent(Entity entity, BinaryWriter writer, IComponent component)
        {
            var name = component?.GetType()?.ToString() ?? null;
            if (name == null)
            {
                writer.Write("");
                return;
            }

            writer.Write(name);

            if(component is ISerializable)
            {
                ((ISerializable)component).Write(writer);
                return;
            }
        }

        public static Entity ReadEntity(EntityContext context, BinaryReader reader)
        {
            var id = reader.ReadUInt32();
            var entity = context.CreateEntity(id);
            // remove components that don't get updated

            var numberOfComponents = reader.ReadInt32();
            var i = 0;
            var componentsRead = new IComponent[numberOfComponents];
            while (numberOfComponents != i && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                componentsRead[i] = ReadComponent(entity, reader);
                i++;
            }

            foreach (var component in entity.GetComponents())
            {
                if (!componentsRead.Contains(component))
                {
                    entity.RemoveComponent(component);
                }
            }

            return entity;
        }

        public static Entity ReadDetachedEntity(BinaryReader reader)
        {
            var entity = new Entity();

            var numberOfComponents = reader.ReadInt32();
            var componentsRead = 0;
            while (numberOfComponents != componentsRead && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                ReadComponent(entity, reader);
                componentsRead++;
            }

            return entity;
        }

        private static IComponent ReadComponent(Entity entity, BinaryReader reader)
        {
            var componentType = GetTypeForId(reader.ReadString());
            if (componentType == null)
                return null;

            var component = entity.GetComponent(componentType);
            if (component == null)
            {
                component = entity.CreateComponent(componentType);
                entity.AddComponent(component);
            }

            if (component is ISerializable)
            {
                ((ISerializable)component).Read(reader);
            }

            return component;
        }


        private static Type GetTypeForId(string identifier)
        {
            ValidateComponentTypeMappingCache();
            if (_componentTypes.ContainsKey(identifier))
            {
                return _componentTypes[identifier];
            }

            return null;
        }

        private static void ValidateComponentTypeMappingCache()
        {
            if (_componentTypesInitialized)
            {
                return;
            }

            // TODO codegen all this noise instead !!
            _componentTypes["Ozzyria.Model.Components.Movement"] = typeof(Movement);
            _componentTypes["Ozzyria.Model.Components.MovementIntent"] = typeof(MovementIntent);
            _componentTypes["Ozzyria.Model.Components.Animator"] = typeof(Animator);
            _componentTypes["Ozzyria.Model.Components.AreaChange"] = typeof(AreaChange);
            _componentTypes["Ozzyria.Model.Components.Armor"] = typeof(Armor);
            _componentTypes["Ozzyria.Model.Components.AttackIntent"] = typeof(AttackIntent);

            _componentTypes["Ozzyria.Model.Components.Location"] = typeof(Location);
            _componentTypes["Ozzyria.Model.Components.Skeleton"] = typeof(Skeleton);
            _componentTypes["Ozzyria.Model.Components.Body"] = typeof(Body);
            _componentTypes["Ozzyria.Model.Components.Weapon"] = typeof(Weapon);
            _componentTypes["Ozzyria.Model.Components.Hat"] = typeof(Hat);
            _componentTypes["Ozzyria.Model.Components.Mask"] = typeof(Mask);

            _componentTypes["Ozzyria.Model.Components.Dead"] = typeof(Dead);
            _componentTypes["Ozzyria.Model.Components.Door"] = typeof(Door);
            _componentTypes["Ozzyria.Model.Components.ExperienceBoost"] = typeof(ExperienceBoost);
            _componentTypes["Ozzyria.Model.Components.ExperienceOrbThought"] = typeof(ExperienceOrbThought);
            _componentTypes["Ozzyria.Model.Components.Item"] = typeof(Item);
            _componentTypes["Ozzyria.Model.Components.Player"] = typeof(Player);
            _componentTypes["Ozzyria.Model.Components.PlayerThought"] = typeof(PlayerThought);
            _componentTypes["Ozzyria.Model.Components.SlimeSpawner"] = typeof(SlimeSpawner);
            _componentTypes["Ozzyria.Model.Components.SlimeThought"] = typeof(SlimeThought);
            _componentTypes["Ozzyria.Model.Components.Stats"] = typeof(Stats);
            _componentTypes["Ozzyria.Model.Components.Collision"] = typeof(Collision);
            _componentTypes["Ozzyria.Model.Components.Bag"] = typeof(Bag);

            _componentTypesInitialized = true;
        }
    }
}
