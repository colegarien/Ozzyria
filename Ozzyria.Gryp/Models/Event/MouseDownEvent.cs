using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.Models.Event
{
    internal class MouseDownEvent
    {
        public MouseState MouseState { get; set; }
        public Camera Camera { get; set; }
        public Map Map { get; set; }
    }
}
