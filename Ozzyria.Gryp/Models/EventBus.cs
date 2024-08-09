using Ozzyria.Gryp.Models.Event;

namespace Ozzyria.Gryp.Models
{
    internal class EventBus
    {
        public static List<object> _subscribers = [];

        public static void Subscribe(object subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public static void Notify<T>(T e)
        {
            foreach (var subscriber in _subscribers)
            {
                if (e != null && subscriber is IEventSubscriber<T>)
                {
                    (subscriber as IEventSubscriber<T>)?.OnNotify(e);
                }
            }
        }
    }
}
