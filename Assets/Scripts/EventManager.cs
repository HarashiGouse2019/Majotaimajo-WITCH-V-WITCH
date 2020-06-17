using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class EventManager
{
    public delegate void CallBackMethod();
    public static CallBackMethod listeners;

    public struct Event
    {
        int uniqueID;
        string eventCode; 

        

        public Event(string eventCode, params CallBackMethod[] newListeners) {
            //Null-Checking
            if (string.IsNullOrEmpty(eventCode))
            {
                //There is no
                this.eventCode = "Unassigned";
            }
            else
            {
                this.eventCode = eventCode;
            }

            uniqueID = Events.Count;

            //Assign all listeners into delegate
            foreach(CallBackMethod listener in newListeners)
            {
                AddNewListener(listener);
            }
        }

        /// <summary>
        /// Return the uniqueId given to this event
        /// </summary>
        /// <returns></returns>
        public int GetUniqueID() => uniqueID;

        /// <summary>
        /// Return the eventCode given to this event
        /// </summary>
        /// <returns></returns>
        public string GetEventCode() => eventCode;

        public void AddNewListener(CallBackMethod listener)
        {
            if(listeners == null)
                listeners += listener;
        }

        public void RemoveListener(CallBackMethod listener)
        {
            if(listeners != null)
                listeners -= listener;
        }

        /// <summary>
        /// Trigger this event, executing all listeners assigned to it.
        /// </summary>
        public void Invoke()
        {
            if (listeners != null)
            {
                listeners.Invoke();
                return;
            }

            Debug.LogError("There are no listeners in this event...");
            return;
        }
    }

    //This associated an event with
    static List<Event> Events = new List<Event>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Add a new event with a uniqueID, name, and defined listeners
    /// </summary>
    /// <param name="uniqueID"></param>
    /// <param name="name"></param>
    /// <param name="listeners"></param>
    public static void AddNewEvent(string name, params CallBackMethod[] listeners)
    {
        Event newEvent = new Event(name, listeners);
        Events.Add(newEvent);
    }

    /// <summary>
    /// Remove an event based on it's uniqueID
    /// </summary>
    /// <param name="uniqueID"></param>
    public static void RemoveEvent(int uniqueID)
    {
        for(int idIndex = 0; idIndex < Events.Count - 1; idIndex++)
        {
            //If we found the event with this uniqueID, remove it
            if (uniqueID.Equals(Events[idIndex].GetUniqueID()))
            {
                Events[idIndex].RemoveListener();
            }
        }
    }

    public static Event[] GetAllEvents() => Events.ToArray();
}
