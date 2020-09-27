using System.Collections.Generic;

namespace Ozzyria.MapEditor.EventSystem
{
    class EventQueue
    {
        public static List<IObserver> observers = new List<IObserver>();
        public static List<IEvent> pendingEvents = new List<IEvent>();

        public static void AttachObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public static void Queue(IEvent e)
        {
            pendingEvents.Add(e);
        }

        public static void DispatchEvents()
        {
            for(var i = 0; i < pendingEvents.Count; i++)
            {
                var e = pendingEvents[i];
                foreach (var observer in observers)
                {
                    if (observer.CanHandle(e))
                        observer.Notify(e);
                }
            }
            pendingEvents.Clear();
        }
    }
}
