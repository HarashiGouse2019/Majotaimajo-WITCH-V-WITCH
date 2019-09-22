﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuSequencer : MonoBehaviour
{
    #region Public Members
    public float startStep, currentStep, nextStep;
    public float stepSpeed;

    public float progress;

    [System.Serializable]
    public class Routine
    {
        public Pattern pattern;
        public uint stepPos;
    }

    public uint runningRoutine = 0;
    public uint completedRoutines = 0;

    public List<Routine> routine = new List<Routine>(); 
    #endregion

    #region Private Members
    readonly uint reset = 0;

    Shoot_Trig trig;

    bool locked = false; 
    #endregion

    void Awake()
    {
        if (currentStep != 0) startStep = GetPreviousStep();
        nextStep = GetNextStep();
        trig = gameObject.GetComponent<Shoot_Trig>();
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
            if (!CheckIfAtLastRoutine())
            {
                runningRoutine++;
                nextStep = GetNextStep();
                startStep = currentStep;
            }

            //If at the end of last one

        }
        if (GetRoutineCompletionInPercentage() == 0)
        {
            Unlock();
            RunPattern(routine[(int)runningRoutine].pattern);
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
        GetRoutineCompletionInPercentage();
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

    public float GetRoutineCompletionInPercentage()
    {
        //Give me the percent between patterns
        progress = (currentStep / (nextStep - startStep)) - completedRoutines;
        return progress;
    }

    //Patterns
    void RunPattern(Pattern _pattern)
    {
        if (locked == Unlock())
        {
            trig.numberOfProjectiles = _pattern.build[(int)runningRoutine].amount;
            trig.loop = _pattern.build[(int)runningRoutine].loop;
            trig.loopSpeed = _pattern.build[(int)runningRoutine].loopRate;
            trig.bullet[0] = _pattern.build[(int)runningRoutine].bullet;
            #region Rotation
            //No idea, but Imma do it!!
            switch (_pattern.build[(int)runningRoutine].rotation)
            {
                case Pattern.Block.RotationType.NoRotation:
                    trig.rotation = Shoot_Trig.RotationType.NoRotation;
                    break;
                case Pattern.Block.RotationType.ClockwiseI:
                    trig.rotation = Shoot_Trig.RotationType.ClockwiseI;
                    break;
                case Pattern.Block.RotationType.ClockwiseII:
                    trig.rotation = Shoot_Trig.RotationType.ClockwiseII;
                    break;
                case Pattern.Block.RotationType.ClockwiseIII:
                    trig.rotation = Shoot_Trig.RotationType.ClockwiseIII;
                    break;
                case Pattern.Block.RotationType.CounterClockwiseI:
                    trig.rotation = Shoot_Trig.RotationType.CounterClockwiseI;
                    break;
                case Pattern.Block.RotationType.CounterClockwiseII:
                    trig.rotation = Shoot_Trig.RotationType.CounterClockwiseII;
                    break;
                case Pattern.Block.RotationType.CounterClockwiseIII:
                    trig.rotation = Shoot_Trig.RotationType.CounterClockwiseIII;
                    break;
                default:
                    break;
            }
            #endregion

            #region Distribution
            //And then again...
            switch (_pattern.build[(int)runningRoutine].distribution)
            {
                case Pattern.Block.DistributionType.Uniformed:
                    trig.distribution = Shoot_Trig.DistributionType.Uniformed;
                    break;
                case Pattern.Block.DistributionType.Biformed:
                    trig.distribution = Shoot_Trig.DistributionType.Biformed;
                    break;
                case Pattern.Block.DistributionType.Scattered:
                    trig.distribution = Shoot_Trig.DistributionType.Scattered;
                    break;
                default:
                    break;
            }
            #endregion
            Lock();
        }
    }

    bool CheckIfAtLastRoutine()
    {
        if (currentStep == routine[routine.Count - 1].stepPos)
        {
            currentStep = reset;
            nextStep = GetNextStep();
            startStep = currentStep;
            runningRoutine = reset;
            return true;
        }
        return false;
    }

    bool Lock()
    {
        locked = true;
        return locked;
    }

    bool Unlock()
    {
        locked = false;
        return locked;
    }
}
