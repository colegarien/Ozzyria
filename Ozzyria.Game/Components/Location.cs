using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
    public class Location : Component
    {
        private string _area = "";

        [Savable]
        public string Area
        {
            get => _area; set
            {
                if (_area != value)
                {
                    _area = value;
                    TriggerChange();
                }
            }
        }
    }
}
