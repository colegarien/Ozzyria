namespace Ozzyria.Gryp.Models.Event
{
    internal interface IEventSubscriber<T>
    {
        public void OnNotify(T e);
    }
}
