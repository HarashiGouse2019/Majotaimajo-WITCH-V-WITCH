using System;
using System.Collections;
using UnityEngine;

public abstract class EventTimeline : MonoBehaviour, IEventSetup
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
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
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

    protected virtual void Next()
    {
        TimelineIndex++;
    }

    public virtual void SetupEvents()
    {

    }


    IEnumerator TimelineCycle()
    {
        while (true)
        {
            MainTimeline();

            yield return null;
        }
    }
}
