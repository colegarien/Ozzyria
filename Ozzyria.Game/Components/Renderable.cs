using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Renderable : Component
    {
        private int _z = (int)ZLayer.Background;
        private int _currentFrame = 0;
        private bool _isDynamic = false;
        private string _staticClip = "";

        // TODO OZ-23 reconsider this, it's kinda weird
        public float timer = 0f;

        [Savable]
        public int Z { get => _z; set
            {
                if (_z != value)
                {
                    _z = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        [Savable]
        public int CurrentFrame
        {
            get => _currentFrame; set
            {
                if (_currentFrame != value)
                {
                    _currentFrame = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        [Savable]
        public bool IsDynamic
        {
            get => _isDynamic; set
            {
                if (_isDynamic != value)
                {
                    _isDynamic = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        [Savable]
        public string StaticClip
        {
            get => _staticClip; set
            {
                if (_staticClip != value)
                {
                    _staticClip = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

    }
}
