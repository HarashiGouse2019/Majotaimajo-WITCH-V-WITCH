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


    /// <summary>
    /// Collect the item
    /// </summary>
    public void Collect()
    {
        onCollect.Invoke();
    }

    public void SetupEvents()
    {
        ev_IncreasePoints = EventManager.AddEvent(250, "IncreasePoints",
            () => ScoreSystem.AddToScore(scoreIncrementValue));

        ev_IncreaseMagic = EventManager.AddEvent(260, "IncreaseMagic",
            () => GameManager.Instance.IncrementMagic(magicIncrementValue));
    }

    /// <summary>
    /// Trigger Event to Increase Points
    /// </summary>
    public void TriggerIncreasePoints()
    {
        Debug.Log("Increased Points");
        ev_IncreasePoints.Trigger();
        ev_IncreasePoints.Reset();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Trigger  Event to Increase Magic
    /// </summary>
    public void TriggerIncreaseMagic()
    {
        Debug.Log("Increased Magic");
        ev_IncreaseMagic.Trigger();
        ev_IncreaseMagic.Reset();
        gameObject.SetActive(false);
    }
}
