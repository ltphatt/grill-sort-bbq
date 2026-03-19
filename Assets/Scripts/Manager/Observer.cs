using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    static Dictionary<EventMessage, List<Action<object[]>>> Listeners = new();

    public static void AddObserver(EventMessage message, Action<object[]> callback)
    {
        if (!Listeners.ContainsKey(message))
        {
            Listeners[message] = new List<Action<object[]>>();
        }

        Listeners[message].Add(callback);
    }

    public static void RemoveObserver(EventMessage message, Action<object[]> callback)
    {
        if (Listeners.ContainsKey(message))
        {
            Listeners[message].Remove(callback);
        }
    }

    public static void Notify(EventMessage message, params object[] data)
    {
        if (!Listeners.ContainsKey(message))
        {
            return;
        }

        foreach (var callback in Listeners[message])
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
