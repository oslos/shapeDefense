using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public delegate void EventListener(GenericEvent genericEvent);

    private Dictionary<string, List<EventListener>> eventListeners;
    private static EventManager _Instance;
    public static EventManager Instance { get { return _Instance; } }

    void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _Instance = this;

            // To persist singleton across scenes.
            DontDestroyOnLoad(this.gameObject);
        }
    }


    public void RegisterListener(string action, EventListener eventListener)
    {
        if (eventListener != null)
        {
            if (eventListeners == null)
            {
                eventListeners = new Dictionary<string, List<EventListener>>();
            }

            if (!eventListeners.ContainsKey(action) || eventListeners[action] == null)
            {
                eventListeners[action] = new List<EventListener>();
            }

            eventListeners[action].Add(eventListener);
        }
    }

    public void UnRegisterListener(string action, EventListener eventListener)
    {
        if (eventListener != null)
        {

            if (eventListeners.ContainsKey(action))
            {
                eventListeners[action].Remove(eventListener);
            }
        }
    }

    public void FireEvent(string action, GenericEvent genericEvent)
    {
        if (eventListeners == null || !eventListeners.ContainsKey(action) || eventListeners[action] == null)
        {

            return;
        }


        foreach (EventListener eventListener in eventListeners[action])
        {
            eventListener(genericEvent);
        }
    }


}
