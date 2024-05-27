using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal interface ITool
    {
        abstract void OnMouseDown(MouseState mouseState, Camera camera, Map map);
        abstract void OnMouseMove(MouseState mouseState, Camera camera, Map map);
        abstract void OnMouseUp(MouseState mouseState, Camera camera, Map map);
    }
}
