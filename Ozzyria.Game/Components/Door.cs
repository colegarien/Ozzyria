using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Door : Component
    {
        private string _newArea = "";
        private float _newX = 0f;
        private float _newY = 0f;


        [Savable]
        public string NewArea
        {
            get => _newArea; set
            {
                if (_newArea != value)
                {
                    _newArea = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        [Savable]
        public float NewX
        {
            get => _newX; set
            {
                if (_newX != value)
                {
                    _newX = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        [Savable]
        public float NewY
        {
            get => _newY; set
            {
                if (_newY != value)
                {
                    _newY = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
