using Ozzyria.Game.ECS;

namespace Ozzyria.Test.ECS.Stub
{
    internal class ComponentC : Component
    {
        private float floatyFloat;
        public float FloatyFloat {
            get => floatyFloat;
            set {
                if (floatyFloat != value)
                {
                    floatyFloat = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
