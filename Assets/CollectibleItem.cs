using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleItem : MonoBehaviour, IEventSetup
{

    [SerializeField]
    UnityEvent onCollect = new UnityEvent();

    //Values for Score, Magic, or Lives
    [SerializeField]
    int scoreIncrementValue;

    [SerializeField]
    float magicIncrementValue;

    [SerializeField]
    int liveIncrementValue;

    //Events
    EventManager.Event @ev_IncreasePoints;
    EventManager.Event @ev_IncreaseMagic;
    EventManager.Event @ev_IncreaseLives;

    // Start is called before the first frame update

    void Start()
    {
        SetupEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupEvents()
    {
        ev_IncreasePoints = EventManager.AddNewEvent(250, "IncreasePoints",
            () => ScoreSystem.AddToScore(scoreIncrementValue));
    }

    public void TriggerIncreasePoints()
    {
        ev_IncreasePoints.Trigger();
        ev_IncreasePoints.Reset();
    }

    public void TriggerIncreaseMagic()
    {
        ev_IncreasePoints.Trigger();
        ev_IncreasePoints.Reset();
    }
}
