namespace Ozzyria.MapEditor.EventSystem
{
    interface IObserver
    {
        public abstract bool CanHandle(IEvent e);
        public abstract void Notify(IEvent e);
    }
}
