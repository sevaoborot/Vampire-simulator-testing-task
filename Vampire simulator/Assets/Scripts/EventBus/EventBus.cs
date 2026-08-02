using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    private Dictionary<string, List<object>> _eventCallbacks = new Dictionary<string, List<object>>(); //signal name and action to be done

    public void Subscribe<T>(Action<T> callback)
    {
        string key = typeof(T).Name;
        if (_eventCallbacks.ContainsKey(key)) _eventCallbacks[key].Add(callback);
        else _eventCallbacks.Add(key, new List<object>() { callback });
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        string key = typeof(T).Name;
        if (_eventCallbacks.ContainsKey(key)) _eventCallbacks[key].Remove(callback);
        else
        {
            Debug.LogError("Trying to unsubscribe from unexisting event.");
            throw new InvalidCastException();
        }
    }

    public void Invoke<T>(T signal)
    {
        string key = typeof(T).Name;
        if (_eventCallbacks.ContainsKey(key))
            foreach (var obj in _eventCallbacks[key])
            {
                var callback = obj as Action<T>;
                callback?.Invoke(signal);
            }
    }
}
