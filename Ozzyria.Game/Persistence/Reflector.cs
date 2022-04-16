using Ozzyria.Game.Components.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ozzyria.Game.Persistence
{
    class Reflector
    {
        private static bool _isInitialized = false;
        private static Dictionary<string, Type> componentTypes = new Dictionary<string, Type>();
        private static Dictionary<Type, PropertyInfo[]> componentProperties = new Dictionary<Type, PropertyInfo[]>();
        private static Dictionary<Type, OptionsAttribute> componentOptions = new Dictionary<Type, OptionsAttribute>();
        private static Dictionary<Type, Dictionary<string, Func<object, object?>>> propertyGetters = new Dictionary<Type, Dictionary<string, Func<object, object?>>>();
        private static Dictionary<Type, Dictionary<string, Delegate>> propertySetters = new Dictionary<Type, Dictionary<string, Delegate>>();


        public static Type  GetTypeForId(string identifier)
        {
            ValidateReflectionCaches();
            return componentTypes.ContainsKey(identifier)
                ? componentTypes[identifier]
                : null;
        }

        public static object? CreateInstance(string identifier)
        {
            ValidateReflectionCaches();
            return componentTypes.ContainsKey(identifier)
                ? Activator.CreateInstance(componentTypes[identifier])
                : null;
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

        public static OptionsAttribute GetOptionsAttribute(Type type)
        {
            ValidateReflectionCaches();
            if (!componentOptions.ContainsKey(type))
            {
                // TODO OZ-21 : add logger
                Console.WriteLine($"[ERROR] Reflector: Type '{type.Name}' Missing OptionsAttribute");
                return null;
            }

            return componentOptions[type];
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

            var types = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.IsClass
                                && t.Namespace.Equals("Ozzyria.Game.Components")
                                && t.GetCustomAttributes(typeof(OptionsAttribute), false).Any()
                      );

            foreach (var type in types)
            {
                var options = (OptionsAttribute)type?.GetCustomAttributes(typeof(OptionsAttribute), false).FirstOrDefault();
                if (options == null)
                    continue;
                componentOptions[type] = options;
                componentTypes[options.Name] = type;

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

            _isInitialized = true;
        }

    }
}
