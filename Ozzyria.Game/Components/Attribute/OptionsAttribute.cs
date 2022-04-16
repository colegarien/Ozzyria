using System;

namespace Ozzyria.Game.Components.Attribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class OptionsAttribute : System.Attribute // TODO OZ-14 flush this out, store component type, name, etc
    {
        public string Name { get; set; } = "";
    }
}
