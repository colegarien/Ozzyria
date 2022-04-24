using System;

namespace Ozzyria.Game.Components.Attribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class OptionsAttribute : System.Attribute
    {
        public string Name { get; set; } = "";
    }
}
