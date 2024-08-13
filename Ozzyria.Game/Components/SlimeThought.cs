using Grecs;

namespace Ozzyria.Game.Components
{
    public class SlimeThought : Component
    {
        public Delay ThinkDelay { get; set; } = new Delay();

        private int _thinkAction = 0;
        public int ThinkAction { get => _thinkAction; set
            {
                if (_thinkAction != value)
                {
                    _thinkAction = value;
                    TriggerChange();
                }
            }
        }
    }
}
