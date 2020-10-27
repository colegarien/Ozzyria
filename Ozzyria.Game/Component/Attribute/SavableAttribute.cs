using System;

namespace Ozzyria.Game.Component.Attribute
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SavableAttribute : System.Attribute // TODO OZ-12 flush this out + turn component type / name into an attribute and use those for serialization
    {
    }
}
