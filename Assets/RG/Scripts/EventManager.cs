using System.Collections.Generic;
using System;

using UnityEngine;

public class EventManager
{
    private static Dictionary<ActionType, Delegate> EventTable = new Dictionary<ActionType, Delegate>();

    public static void Subscribe<T>(ActionType eventType, Action<T> listener)
    {
        if (!EventTable.ContainsKey(eventType))
            EventTable[eventType] = listener;
        else
            EventTable[eventType] = Delegate.Combine(EventTable[eventType], listener);
    }

    public static void Unsubscribe<T>(ActionType eventType, Action<T> listener)
    {
        if (EventTable.ContainsKey(eventType))
        {
            EventTable[eventType] = Delegate.Remove(EventTable[eventType], listener);
            if (EventTable[eventType] == null)
                EventTable.Remove(eventType);
        }
    }

    public static void Raise<T>(ActionType eventType, T arg)
    {
        if (EventTable.ContainsKey(eventType))
            (EventTable[eventType] as Action<T>)?.Invoke(arg);
    }
}
