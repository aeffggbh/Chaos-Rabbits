using System;
using System.Collections.Generic;

/// <summary>
/// Service to store events
/// </summary>
public static class EventProvider
{
    private static Dictionary<Type, Delegate> _eventListeners = new();
    public static Dictionary<Type, Delegate> EventListeners { get { return _eventListeners; } }

    /// <summary>
    /// Subscribes a new action to the dictionary according to its type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listener"></param>
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

    /// <summary>
    /// Unsubscribe a specific action from the dictionary acording to its type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listener"></param>
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
