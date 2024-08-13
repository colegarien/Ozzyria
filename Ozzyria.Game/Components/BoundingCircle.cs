using Ozzyria.Game.Components.Attribute;

namespace Ozzyria.Game.Components
{
    public class BoundingCircle : Collision
    {
        private float _radius = 10f;

        [Savable]
        public float Radius { get => _radius; set
            {
                if (_radius != value)
                {
                    _radius = value;
                    TriggerChange();
                }
            }
        }
    }
}
