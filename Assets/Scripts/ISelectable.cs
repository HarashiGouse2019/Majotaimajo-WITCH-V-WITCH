﻿using Extensions;
using System.Collections;
using UnityEngine;
using static Keymapper;

internal interface ISelectable
{
    EventManager.Event _onSelectNext { get; set; }
    EventManager.Event _onSelectPrevious { get; set; }
    EventManager.Event _onConfirm { get; set; }
    EventManager.Event _onCancel { get; set; }
    Audio cursorSound { get; set; }
    Audio confirmSound { get; set; }
    Audio cancelSound { get; set; }
    int selectionIndex { get; set; }
    void SelectionCycle();
}

public abstract class SelectionObject : MonoBehaviour, ISelectable, IEventSetup
{
    public EventManager.Event _onSelectNext { get; set; }
    public EventManager.Event _onSelectPrevious { get; set; }
    public EventManager.Event _onConfirm { get; set; }
    public EventManager.Event _onCancel { get; set; }
    public Audio cursorSound { get; set; }
    public Audio confirmSound { get; set; }
    public Audio cancelSound { get; set; }
    public int selectionIndex { get; set; }

    void Awake()
    {
        SetupEvents();
    }

    void Start()
    {
        cursorSound = AudioManager.Find("selection");
        confirmSound = AudioManager.Find("confirmSelection");
        cancelSound = AudioManager.Find("cancelSelection");
        StartCoroutine(Routine);
    }

    private IEnumerator Routine
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
        
    }

    public void SelectionCycle()
    {
        ControlAction("left", false, _onSelectPrevious);
        ControlAction("right", false, _onSelectNext);
        ControlAction("start", false, _onConfirm);
        ControlAction("cancel", false, _onCancel);
    }
}