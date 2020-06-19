using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class EventManager
{
    public delegate void CallBackMethod();
    public static CallBackMethod listeners;

    public class Event
    {
        int uniqueID;
        string eventCode;
        bool hasTriggered;

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

            hasTriggered = false;
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
        public void Trigger()
        {
            if (listeners != null)
            {
                hasTriggered = true;
                listeners.Invoke();
                return;
            }

            Debug.LogError("There are no listeners in this event...");
            return;
        }

        /// <summary>
        /// Returns if this even has been triggered
        /// </summary>
        public bool HasTriggered()
        {
            return hasTriggered;
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
    public static Event AddNewEvent(int uniqueID, string name, CallBackMethod listener)
    {
        Event newEvent = new Event(uniqueID, name, listener);
        Events.Add(newEvent);
        return newEvent;
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
                //Now delete the event itself
                Debug.Log("Event " + Events[idIndex].GetEventCode() + " has been removed");
                Events.Remove(Events[idIndex]);
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
                //Now delete the event itself
                Debug.Log("Event " + Events[idIndex].GetEventCode() + " has been removed");
                Events.Remove(Events[idIndex]);
            }
        }
    }

    /// <summary>
    /// Retuns all events of this event code
    /// </summary>
    /// <param name="eventCode"></param>
    /// <returns></returns>
    public static Event[] FindEventsOfEventCode(string eventCode)
    {
        List<Event> foundEvents = new List<Event>();
        for (int idIndex = 0; idIndex < Events.Count - 1; idIndex++)
        {
            //If we found the event with this eventCode, remove it
            if (eventCode.Equals(Events[idIndex].GetEventCode()))
            {
                //Add it to our discorvered events
                foundEvents.Add(Events[idIndex]);
            }
        }

        //Return the foundEvents
        return foundEvents.ToArray();
    }

    /// <summary>
    /// Check if all events of this kind have been triggered
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    public static bool HaveAllTriggered(this Event[] events)
    {
        foreach(Event @event in events)
        {
            if (!@event.HasTriggered()) return false;
        }

        return true;
    }

    public static void TriggerEvent(int uniqueId)
    {
        for (int idIndex = 0; idIndex < Events.Count - 1; idIndex++)
        {
            //If we found the event with this eventCode, remove it
            if (uniqueId.Equals(Events[idIndex].GetUniqueID()))
            {
                //Trigger events of this uniqueID
                Events[idIndex].Trigger();
            }
        }
    }

    public static void TriggerEvent(string eventCode)
    {
        for (int idIndex = 0; idIndex < Events.Count - 1; idIndex++)
        {
            //If we found the event with this eventCode, remove it
            if (eventCode.Equals(Events[idIndex].GetEventCode()))
            {
                //Trigger events of this eventCode
                Events[idIndex].Trigger();
            }
        }
    }

    /// <summary>
    /// Returns all events of different IDs and EventCodes
    /// </summary>
    /// <returns></returns>
    public static Event[] GetAllEvents() => Events.ToArray();
}
