using System;
using UnityEngine;

[Serializable]
public class StageEvent : ScriptableObject
{
    public int Interval;

    public StateInstructions m_event;

    public Parameter[] parameters;
}
