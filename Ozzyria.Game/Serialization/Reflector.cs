using Ozzyria.Game.Components.Attribute;
using Grecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ozzyria.Model.Components;

namespace Ozzyria.Game.Serialization
{
    class Reflector
    {
        private static bool _isInitialized = false;
        private static Dictionary<string, Type> componentTypes = new Dictionary<string, Type>();
        private static Dictionary<Type, PropertyInfo[]> componentProperties = new Dictionary<Type, PropertyInfo[]>();
        private static Dictionary<Type, Dictionary<string, Func<object, object?>>> propertyGetters = new Dictionary<Type, Dictionary<string, Func<object, object?>>>();
        private static Dictionary<Type, Dictionary<string, Delegate>> propertySetters = new Dictionary<Type, Dictionary<string, Delegate>>();

        public static Type  GetTypeForId(string identifier)
        {
            ValidateReflectionCaches();
            if (componentTypes.ContainsKey(identifier))
            {
                return componentTypes[identifier];
            }

            return null;
        }

        public static object? GetPropertyValue(PropertyInfo p, object? instance)
        {
            ValidateReflectionCaches();
            if (instance == null || !propertyGetters.ContainsKey(instance.GetType()) || !propertyGetters[instance.GetType()].ContainsKey(p.Name))
            {
                // TODO OZ-21 : add logger
                Console.WriteLine($"[ERROR] Reflector: Cannot Get Property '{p.Name}' from '{instance?.GetType().Name ?? "null"}'");
                return null;
            }

            return propertyGetters[instance.GetType()][p.Name](instance);
        }

        public static void SetPropertyValue(PropertyInfo p, object instance, object? value)
        {
            ValidateReflectionCaches();
            if (instance == null || !propertySetters.ContainsKey(instance.GetType()) || !propertySetters[instance.GetType()].ContainsKey(p.Name))
            {
                // TODO OZ-21 : add logger
                Console.WriteLine($"[ERROR] Reflector: Cannot Set Property '{p.Name}' to '{value}' on '{instance?.GetType().Name ?? "null"}'");
                return;
            }

            propertySetters[instance.GetType()][p.Name].DynamicInvoke(instance, value);
        }

        public static PropertyInfo[] GetSavableProperties(Type type)
        {
            ValidateReflectionCaches();
            if (!componentProperties.ContainsKey(type))
            {
                // TODO OZ-21 : add logger
                Console.WriteLine($"[ERROR] Reflector: Type '{type.Name}' Properties Failed to Load");
                return null;
            }

            return componentProperties[type];
        }

        private static void ValidateReflectionCaches()
        {
            if (_isInitialized)
            {
                return;
            }

            Type componentInterface = typeof(IComponent);
            var typesOfComponents = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.IsClass
                                && t.Namespace.Equals("Ozzyria.Game.Components")
                                && componentInterface.IsAssignableFrom(t)
                      );

            foreach (var type in typesOfComponents)
            {
                componentTypes[type.ToString()] = type;

                var properties = type.GetProperties()
                    .Where(property => Attribute.IsDefined(property, typeof(SavableAttribute)))
                    .OrderBy(p => p.Name)
                    .ToArray();
                componentProperties[type] = properties;

                propertyGetters[type] = new Dictionary<string, Func<object, object?>>();
                propertySetters[type] = new Dictionary<string, Delegate>();
                foreach (var property in properties)
                {
                    // Generate Getter Delegates
                    var method = property.GetGetMethod();
                    var paramExpress = Expression.Parameter(typeof(object), "instance");
                    var instanceCast = !property.DeclaringType.IsValueType
                        ? Expression.TypeAs(paramExpress, property.DeclaringType)
                        : Expression.Convert(paramExpress, property.DeclaringType);

                    var expr =
                        Expression.Lambda<Func<object, object?>>(
                            Expression.TypeAs(
                                Expression.Call(instanceCast, method),
                                typeof(object)
                             ),
                            paramExpress);
                    propertyGetters[type][property.Name] = expr.Compile();

                    // Generate Setter Delegates
                    var i = Expression.Parameter(property.DeclaringType, "i");
                    var a = Expression.Parameter(typeof(object), "a");
                    var setterCall = Expression.Call(i, property.GetSetMethod(), Expression.Convert(a, property.PropertyType));
                    var exp = Expression.Lambda(setterCall, i, a);
                    propertySetters[type][property.Name] = exp.Compile();
                }
            }


            // TODO codegen all this noise instead !!
            componentTypes["Ozzyria.Model.Components.Movement"] = typeof(Ozzyria.Model.Components.Movement);
            componentTypes["Ozzyria.Model.Components.MovementIntent"] = typeof(Ozzyria.Model.Components.MovementIntent);
            componentTypes["Ozzyria.Model.Components.Animator"] = typeof(Ozzyria.Model.Components.Animator);
            componentTypes["Ozzyria.Model.Components.AreaChange"] = typeof(Ozzyria.Model.Components.AreaChange);
            componentTypes["Ozzyria.Model.Components.Armor"] = typeof(Ozzyria.Model.Components.Armor);
            componentTypes["Ozzyria.Model.Components.AttackIntent"] = typeof(Ozzyria.Model.Components.AttackIntent);

            componentTypes["Ozzyria.Model.Components.Location"] = typeof(Ozzyria.Model.Components.Location);
            componentTypes["Ozzyria.Model.Components.Skeleton"] = typeof(Ozzyria.Model.Components.Skeleton);
            componentTypes["Ozzyria.Model.Components.Body"] = typeof(Ozzyria.Model.Components.Body);
            componentTypes["Ozzyria.Model.Components.Weapon"] = typeof(Ozzyria.Model.Components.Weapon);
            componentTypes["Ozzyria.Model.Components.Hat"] = typeof(Ozzyria.Model.Components.Hat);
            componentTypes["Ozzyria.Model.Components.Mask"] = typeof(Ozzyria.Model.Components.Mask);

            componentTypes["Ozzyria.Model.Components.Dead"] = typeof(Ozzyria.Model.Components.Dead);
            componentTypes["Ozzyria.Model.Components.Door"] = typeof(Ozzyria.Model.Components.Door);
            componentTypes["Ozzyria.Model.Components.ExperienceBoost"] = typeof(Ozzyria.Model.Components.ExperienceBoost);
            componentTypes["Ozzyria.Model.Components.ExperienceOrbThought"] = typeof(Ozzyria.Model.Components.ExperienceOrbThought);
            componentTypes["Ozzyria.Model.Components.Item"] = typeof(Ozzyria.Model.Components.Item);
            componentTypes["Ozzyria.Model.Components.Player"] = typeof(Ozzyria.Model.Components.Player);
            componentTypes["Ozzyria.Model.Components.PlayerThought"] = typeof(Ozzyria.Model.Components.PlayerThought);
            componentTypes["Ozzyria.Model.Components.SlimeSpawner"] = typeof(Ozzyria.Model.Components.SlimeSpawner);
            componentTypes["Ozzyria.Model.Components.SlimeThought"] = typeof(Ozzyria.Model.Components.SlimeThought);
            componentTypes["Ozzyria.Model.Components.Stats"] = typeof(Ozzyria.Model.Components.Stats);
            componentTypes["Ozzyria.Model.Components.Collision"] = typeof(Ozzyria.Model.Components.Collision);
            componentTypes["Ozzyria.Model.Components.Bag"] = typeof(Ozzyria.Model.Components.Bag);


            _isInitialized = true;
        }

    }
}
