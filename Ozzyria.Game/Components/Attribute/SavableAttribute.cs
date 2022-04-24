using System;

namespace Ozzyria.Game.Components.Attribute
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SavableAttribute : System.Attribute
    {
    }
}
