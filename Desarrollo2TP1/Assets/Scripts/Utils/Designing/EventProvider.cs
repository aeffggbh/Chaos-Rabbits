using System;
using System.Collections.Generic;

/// <summary>
/// Service to store and trigger events
/// </summary>
/// 
public static class EventProvider
{
    private static Dictionary<Type, Delegate> _eventListeners = new();
    public static Dictionary<Type, Delegate> EventListeners { get { return _eventListeners; } }

    public static void Subscribe<T>(Action<T> listener) where T : IEvent
    {
        if (_eventListeners.TryGetValue(typeof(T), out var existingDelegate))
        {
            //it's added to that type's list
            _eventListeners[typeof(T)] = Delegate.Combine(existingDelegate, listener);
        }
        else
            _eventListeners[typeof(T)] = listener;
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : IEvent
    {
        if (_eventListeners.TryGetValue(typeof(T), out var existingDelegate))
        {
            //if there's a whole list of events of type T, then I'll just remove this one
            var newDelegate = Delegate.Remove(existingDelegate, listener);

            if (newDelegate == null)
                //if the list is now empty, I remove it entirely.
                _eventListeners.Remove(typeof(T));
            else
                _eventListeners[typeof(T)] = newDelegate;
        }
    }

}
