using static Ozzyria.MonoGameClient.UI.InputTracker;

namespace Ozzyria.MonoGameClient.UI.Handlers
{
    internal interface IMouseUpHandler
    {
        public bool HandleMouseUp(InputTracker tracker, MouseButton button, int x, int y);
    }
}
