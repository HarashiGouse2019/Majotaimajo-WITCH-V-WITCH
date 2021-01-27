using Extensions;
using System.Collections;
using UnityEngine;
using static Keymapper;

internal partial interface ISelectable
{
    EventManager.Event _onSelectNext { get; set; }
    EventManager.Event _onSelectPrevious { get; set; }
    EventManager.Event _onConfirm { get; set; }
    EventManager.Event _onCancel { get; set; }
    int selectionIndex { get; set; }
    void SelectionCycle();
}

public partial class SelectionObject : MonoBehaviour, ISelectable, IEventSetup
{
    public EventManager.Event _onSelectNext { get; set; }
    public EventManager.Event _onSelectPrevious { get; set; }
    public EventManager.Event _onConfirm { get; set; }
    public EventManager.Event _onCancel { get; set; }
    public int selectionIndex { get; set; }

    void Awake()
    {
        SetupEvents();
    }

    void Start()
    {
        Routine.Start();
    }

    IEnumerator Routine
    {
        get
        {
            while (true)
            {
                SelectionCycle();
                yield return null;
            }
        }
    }

    public virtual void SetupEvents()
    {
        throw new System.NotImplementedException();
    }

    public void SelectionCycle()
    {
        ControlAction("left", false, _onSelectPrevious);
        ControlAction("right", false, _onSelectNext);
        ControlAction("start", false, _onConfirm);
        ControlAction("cancel", false, _onCancel);
    }
}