namespace Ozzyria.Game.Event
{
    public interface IEventHandler
    {
        public bool CanHandle(IEvent gameEvent);
        public void Handle(IEvent gameEvent);
    }
}
