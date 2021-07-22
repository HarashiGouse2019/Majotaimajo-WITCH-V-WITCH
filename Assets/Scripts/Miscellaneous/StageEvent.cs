using System;
using UnityEngine;

using SequenceEventUtility;

public class StageEvent
{
    public int Interval { get; set; }

    public StateInstructions m_event { get; set; }

    public object[] parameters;

    internal StageEvent(int initInterval, StateInstructions instruction, params  object[] initParameters)
    {
        Interval = initInterval;
        m_event = instruction;
        parameters = initParameters;
    }
}
