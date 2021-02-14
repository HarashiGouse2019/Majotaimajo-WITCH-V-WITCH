using System;
using System.Collections;
using UnityEngine;
using Extensions;
[Serializable]
public class StageMap
{
    /* This class will handle the timing of enemy spawning, and where they spawn
     * as well as how to handle the scrolling and design of the stage's background.
     * This can mainly be used for the various of difficuties that a stage may provide
     * based on a given index. */

    public sealed class StageEvents
    {
        public int Interval { get; private set; }
        public EventManager.Event @Event { get; private set; }

        public StageEvents(int interval, EventManager.Event @newEvent)
        {
            Interval = interval;
            @Event = newEvent;
        }
    }

    [SerializeField]
    private string mapName;

    public string MapName => mapName;

    private float currentInterval = -1f;
    private float intervalDelta = 0.1f;

    protected StageEvents[] events;

    protected StageMap()
    {
        Init();
    }

    protected virtual void Init()
    {
        //TODO: Stage Configuration
    }

    private void Close()
    {
        MainCycle().Stop();
        intervalDelta = default;
        currentInterval = default;
    }

    void Main(params StageEvents[] events)
    {
        for (int i = 0; i < events.Length - 1; i++)
        {
            if (events[i].Interval == currentInterval)
                events[i].Event.Trigger();
        };
    }

    private void NextInterval() => currentInterval++;

    IEnumerator MainCycle()
    {
        while (true)
        {
            try
            {
                Main(events);
                NextInterval();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            yield return null;
        }
    }

    ~StageMap()
    {
        Close();
    }
}
