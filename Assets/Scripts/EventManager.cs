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

        public Event(int uniqueID, string eventCode, CallBackMethod newListener) {

            this.uniqueID = uniqueID;
            
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
            //Assign all listeners into delegate
              AddNewListener(newListener);
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

    /// <summary>
    /// Add a new event with a uniqueID, name, and defined listeners
    /// </summary>
    /// <param name="uniqueID"></param>
    /// <param name="name"></param>
    /// <param name="listeners"></param>
    public static void AddNewEvent(int uniqueID, string name, CallBackMethod listener)
    {
        Event newEvent = new Event(uniqueID, name, listener);
        Events.Add(newEvent);
    }

    /// <summary>
    /// Remove an event based on it's eventCode
    /// </summary>
    /// <param name="eventCode"></param>
    public static void RemoveEvent(string eventCode)
    {
        for(int idIndex = 0; idIndex < Events.Count - 1; idIndex++)
        {
            //If we found the event with this eventCode, remove it
            if (eventCode.Equals(Events[idIndex].GetEventCode()))
            {
                Events[idIndex].RemoveListener(listeners);
                return;
            }
        }
    }

    /// <summary>
    /// Remove an event based on it's uniqueID
    /// </summary>
    /// <param name="eventCode"></param>
    public static void RemoveEvent(int uniqueId)
    {
        for (int idIndex = 0; idIndex < Events.Count - 1; idIndex++)
        {
            //If we found the event with this eventCode, remove it
            if (uniqueId.Equals(Events[idIndex].GetUniqueID()))
            {
                Events[idIndex].RemoveListener(listeners);
                return;
            }
        }
    }

    public static Event[] GetAllEvents() => Events.ToArray();
}
