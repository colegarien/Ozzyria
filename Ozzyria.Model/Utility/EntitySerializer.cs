using Grecs;
using Grecs;
using Ozzyria.Model.Components;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.Utility
{
    public class EntitySerializer
    {
        private static Dictionary<string, Type> _componentIdentiferToType = new Dictionary<string, Type>
        {
            {"Animator", typeof(Animator)},
            {"AreaChange", typeof(AreaChange)},
            {"Armor", typeof(Armor)},
            {"AttackIntent", typeof(AttackIntent)},
            {"Bag", typeof(Bag)},
            {"Body", typeof(Body)},
            {"Collision", typeof(Collision)},
            {"Dead", typeof(Dead)},
            {"Door", typeof(Door)},
            {"ExperienceBoost", typeof(ExperienceBoost)},
            {"ExperienceOrbThought", typeof(ExperienceOrbThought)},
            {"Hat", typeof(Hat)},
            {"Item", typeof(Item)},
            {"Location", typeof(Location)},
            {"Mask", typeof(Mask)},
            {"Movement", typeof(Movement)},
            {"MovementIntent", typeof(MovementIntent)},
            {"Player", typeof(Player)},
            {"PlayerThought", typeof(PlayerThought)},
            {"Skeleton", typeof(Skeleton)},
            {"SlimeSpawner", typeof(SlimeSpawner)},
            {"SlimeThought", typeof(SlimeThought)},
            {"Stats", typeof(Stats)},
            {"Weapon", typeof(Weapon)},
        };

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
            if (!(component is ISerializable))
                return;


            var serializeable = component as ISerializable;
            writer.Write(serializeable.GetComponentIdentifier());
            serializeable.Write(writer);
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
            var componentIdentifier = reader.ReadString();
            if (!_componentIdentiferToType.ContainsKey(componentIdentifier)) {
                return null;
            }

            var componentType = _componentIdentiferToType[componentIdentifier];
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


    }
}
