using static Ozzyria.MonoGameClient.UI.InputTracker;

namespace Ozzyria.MonoGameClient.UI.Handlers
{
    internal interface IMouseUpHandler
    {
        // TODO UI unify event handler interfaces and priority/focus interface
        public bool HandleMouseUp(InputTracker tracker, MouseButton button, int x, int y);
    }
}
