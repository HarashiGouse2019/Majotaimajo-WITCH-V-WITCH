using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakuPattern;

public class DanmakuSequencer : MonoBehaviour
{
    public uint startStep, currentStep, nextStep;
    public float stepSpeed;

    [System.Serializable]
    public class Routine
    {
        public Pattern pattern;
        public uint stepPos;
    }

    public uint runningRoutine = 0;

    public List<Routine> routine = new List<Routine>();

    void Awake()
    {
        if (currentStep != 0) startStep = GetPreviousStep();
        nextStep = GetNextStep();
    }
    void OnEnable()
    {
        IterateSequence();   
    }

    void OnDisable()
    {
        PauseSequenceIteration();    
    }

    private void Update()
    {
        if (currentStep >= nextStep)
        {
            runningRoutine++;
            nextStep = GetNextStep();
            startStep = currentStep;
        }
    }

    public void IterateSequence()
    {
        InvokeRepeating("Sequence", 0f, stepSpeed);
    }

    public void PauseSequenceIteration()
    {
        CancelInvoke("Sequence");
    }

    void Sequence()
    {
        currentStep++;
    }

    public uint GetNextStep()
    {
        return routine[(int)runningRoutine + 1].stepPos;
    }

    public uint GetPreviousStep()
    {
        return routine[(int)runningRoutine - 1].stepPos;
    }

    public uint GetRoutineCompletionInPercentage()
    {
        //Give me the percent between patterns
        return (currentStep) / (nextStep - startStep);
    }
}
