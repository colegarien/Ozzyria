using System;

namespace Ozzyria.Game.Component.Attribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class OptionsAttribute : System.Attribute // TODO OZ-18 flush this out, store component type, name, etc
    {
        public string Name { get; set; } = "";
    }
}
