using System;
using System.Collections;
using UnityEngine;

public abstract class EventTimeline : MonoBehaviour
{
    public class TimelineException : Exception
    {
        public TimelineException() { }
        public TimelineException(string message) : base(message) { }
        public TimelineException(string message, Exception inner) : base(message, inner) { }
    }

    protected static Pawn Entity;

    protected static int TimelineIndex = -1;

    // Start is called before the first frame update
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {

    }

    /// <summary>
    /// InitializeTimeline
    /// </summary>
    public void Initialize(Pawn entity)
    {
        TimelineIndex = 0;
        Entity = entity;
        StartCoroutine(TimelineCycle());
    }

    protected virtual void MainTimeline()
    {
        //MainTimeline goes through here.
        //The timelines will be constructed in this way:
        switch (TimelineIndex)
        {
            //You can check for anything that happens in this moment
            //Call the Next method to increment 
            case 0:
                break;
        }
    }

    protected virtual void Next(params EventManager.Event[] events)
    {
        TimelineIndex++;
        OnNext(events);
    }

    void OnNext(params EventManager.Event[] events)
    {
        foreach (EventManager.Event _event in events)
        {
            _event.Trigger();
        }
    }

    IEnumerator TimelineCycle()
    {
        while (true)
        {
            if(!Dialogue.IsRunning) MainTimeline();

            yield return null;
        }
    }
}
