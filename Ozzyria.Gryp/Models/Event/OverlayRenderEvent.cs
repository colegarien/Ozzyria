using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;
using SkiaSharp;

namespace Ozzyria.Gryp.Models.Event
{
    internal class OverlayRenderEvent
    {
        public SKCanvas Canvas { get; set; }
        public Camera Camera { get; set; }
        public Map Map { get; set; }
        public MouseState MouseState {get;set;}
    }
}
