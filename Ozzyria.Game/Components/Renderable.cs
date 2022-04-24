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

    [Options(Name = "Renderable")]
    public class Renderable : Component
    {
        private SpriteType _sprite = SpriteType.Default;
        [Savable]
        public SpriteType Sprite
        {
            get => _sprite; set
            {
                if (_sprite != value)
                {
                    _sprite = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

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
    }
}
