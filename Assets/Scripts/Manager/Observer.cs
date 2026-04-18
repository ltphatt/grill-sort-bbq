using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    private static readonly Dictionary<EventMessage, List<Action<object[]>>> listeners = new();

    public static void AddObserver(EventMessage message, Action<object[]> callback)
    {
        if (!listeners.ContainsKey(message))
        {
            listeners[message] = new List<Action<object[]>>();
        }

        listeners[message].Add(callback);
    }

    public static void RemoveObserver(EventMessage message, Action<object[]> callback)
    {
        if (listeners.ContainsKey(message))
        {
            listeners[message].Remove(callback);
        }
    }

    public static void Notify(EventMessage message, params object[] data)
    {
        if (!listeners.ContainsKey(message))
        {
            return;
        }

        foreach (var callback in listeners[message])
        {
            try
            {
                callback?.Invoke(data);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error invoking callback for event '{message}': {ex.Message}");
            }
        }
    }
}
