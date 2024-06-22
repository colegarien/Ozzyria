using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal abstract class ITool
    {

        public bool Enabled { get; set; } = false;

        public abstract void OnMouseDown(MouseState mouseState, Camera camera, Map map);
        public abstract void OnMouseMove(MouseState mouseState, Camera camera, Map map);
        public abstract void OnMouseUp(MouseState mouseState, Camera camera, Map map);
    }
}
