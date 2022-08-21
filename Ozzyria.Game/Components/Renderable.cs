using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{

    public enum SpriteType  // OZ-23 : refactor this to not be an enum (make more data-driven)
    {
        Default = 1,
        Player = 2,
        Slime = 3,
        Particle = 4,
    }

    public class Renderable : Component
    {
        private int _z = (int)ZLayer.Background;
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

        private string _currentClip = "";
        [Savable]
        public string CurrentClip
        {
            get => _currentClip; set
            {
                if (_currentClip != value)
                {
                    _currentClip = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        private int _currentFrame = 0;
        [Savable]
        public int CurrentFrame
        {
            get => _currentFrame; set
            {
                if (_currentFrame != value)
                {
                    _currentFrame  = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

    }
}
