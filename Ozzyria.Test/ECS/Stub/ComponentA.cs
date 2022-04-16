using Ozzyria.Game.ECS;

namespace Ozzyria.Test.ECS.Stub
{
    internal class ComponentA : Component
    {
        private string _value = "";

        public string Value { get => _value; set
            {
                if (_value != value)
                {
                    _value = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
    }
}
