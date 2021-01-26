using System.Collections;
using UnityEngine;
using static Keymapper;

internal interface ISelectable
{
    EventManager.Event _onSelectNext { get; set; }
    EventManager.Event _onSelectPrevious { get; set; }
    EventManager.Event _onConfirm { get; set; }
    int selectionIndex { get; set; }
    void SelectionCycle();

}

public class SelectionObject : MonoBehaviour, ISelectable, IEventSetup
{
    public EventManager.Event _onSelectNext { get; set; }
    public EventManager.Event _onSelectPrevious { get; set; }
    public EventManager.Event _onConfirm { get; set; }
    public int selectionIndex { get; set; }

    void Start()
    {
        SetupEvents();
        StartCoroutine(Routine());
    }
    
    IEnumerator Routine()
    {
        while (true)
        {
            SelectionCycle();
            yield return null;
        }
    }

    public virtual void SelectionCycle()
    {
        ControlAction("left", false, _onSelectPrevious ?? null);
        ControlAction("right", false, _onSelectNext ?? null);
        ControlAction("start", false, _onConfirm ?? null);
    }

    public virtual void SetupEvents()
    {
        
    }
}