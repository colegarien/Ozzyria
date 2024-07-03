using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp.MapTools
{
    internal class SelectTool : IAreaTool
    {

        protected override void OnCancel(MouseState mouseState, Camera camera, Map map)
        {
            map.SelectedRegion = null;
        }

        protected override void OnComplete(MouseState mouseState, Camera camera, Map map)
        {
            map.SelectedRegion = GetTileArea();
        }
    }
}
