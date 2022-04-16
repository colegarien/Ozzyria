using System;

namespace Ozzyria.Game.Components.Attribute
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SavableAttribute : System.Attribute // TODO OZ-14 flush this out + turn component type / name into an attribute and use those for serialization
    {
    }
}
