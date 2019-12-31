﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuSequencer : MonoBehaviour
{
    #region Public Members
    public bool allowOverride = false;

    public float startStep, currentStep, nextStep;
    public float stepSpeed;

    public float progress;

    public List<Spell.Routine> routine = new List<Spell.Routine>();

    public uint runningRoutine = 0;
    public int completedRoutines = 0;
    public int completedLoops = 0;

    public bool enableSequenceLooping;
    #endregion

    #region Private Members
    readonly uint reset = 0;
    Shoot_Trig trig;
    #endregion

    void Awake()
    {
        trig = GetComponent<Shoot_Trig>();
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
        GetRoutineCompletionInPercentage();
    }

    public void IterateSequence()
    {
        if (currentStep != 0) startStep = GetPreviousStep();
        nextStep = GetNextStep();
        InvokeRepeating("Sequence", 0f, stepSpeed);
    }

    public void PauseSequenceIteration()
    {
        CancelInvoke("Sequence");
    }

    void Sequence()
    {
        Debug.Log("Doing sequence desu!!!");
        currentStep++;

        if (currentStep == nextStep)
        {
            if (!CheckIfAtLastRoutine())
            {
                runningRoutine++;
                completedRoutines++;
                nextStep = GetNextStep();
                startStep = GetPreviousStep();
                progress = 0;
            }
        }

        if (GetRoutineCompletionInPercentage() < 0.1f)
        {
            RunPattern(routine[(int)runningRoutine].pattern);
        }
    }

    public uint GetNextStep()
    {
        Spell.Routine routineCheck = null;

        routineCheck = routine[(int)runningRoutine + 1];

        RunPattern(routineCheck.pattern);

        return routineCheck.stepPos;
    }

    public uint GetPreviousStep()
    {
        Spell.Routine routineCheck = null;

        routineCheck = routine[(int)runningRoutine];

        RunPattern(routineCheck.pattern);

        return routineCheck.stepPos;
    }

    public float GetRoutineCompletionInPercentage()
    {
        //Give me the percent between patterns
        progress = (currentStep - startStep) / (nextStep - startStep);
        return progress;
    }

    //Patterns
    void RunPattern(Pattern _pattern)
    {
        Debug.Log("Okay! So it's hitting this function now!!!");
        #region Assing Values
        trig.numberOfProjectiles = _pattern.block.amount;

        trig.loop = _pattern.block.loop;

        trig.loopSpeed = _pattern.block.loopRate;

        trig.bulletMember = _pattern.block.bulletType;

        trig.speed = _pattern.block.speed;

        trig.incrementVal = _pattern.block.incrementVal;

        if (_pattern.block.overrideRotation)
            trig.g_angle = _pattern.block.initialRotation;

        trig.rotationFocus = _pattern.block.rotationFocus;

        trig.rotationIntensity = _pattern.block.rotationIntensity;
        #endregion
        #region Rotation
        //No idea, but Imma do it!!
        switch ((int)_pattern.block.rotation)
        {
            case 0:
                trig.rotation = Shoot_Trig.RotationType.NoRotation;
                break;

            case 1:
                trig.rotation = Shoot_Trig.RotationType.ClockwiseI;
                break;

            case 2:
                trig.rotation = Shoot_Trig.RotationType.ClockwiseII;
                break;

            case 3:
                trig.rotation = Shoot_Trig.RotationType.ClockwiseIII;
                break;

            case -1:
                trig.rotation = Shoot_Trig.RotationType.CounterClockwiseI;
                break;

            case -2:
                trig.rotation = Shoot_Trig.RotationType.CounterClockwiseII;
                break;

            case -3:
                trig.rotation = Shoot_Trig.RotationType.CounterClockwiseIII;
                break;

            default:
                break;
        }
        #endregion
        #region Distribution
        //And then again...
        switch (_pattern.block.distribution)
        {
            case Pattern.Block.DistributionType.Uniformed:
                trig.distribution = Shoot_Trig.DistributionType.Uniformed;
                break;

            case Pattern.Block.DistributionType.Biformed:
                trig.distribution = Shoot_Trig.DistributionType.Biformed;
                break;

            case Pattern.Block.DistributionType.Increment:
                trig.distribution = Shoot_Trig.DistributionType.Increment;
                break;

            case Pattern.Block.DistributionType.Scattered:
                trig.distribution = Shoot_Trig.DistributionType.Scattered;
                break;

            default:
                break;
        }
        #endregion
        #region Rotation Focus Effect
        switch (_pattern.block.rotationFocusEffect)
        {
            case Pattern.Block.RotationFocusEffect.Static:
                //Do Nothing
                break;
            case Pattern.Block.RotationFocusEffect.Increment:
                trig.rotationFocusIncrementVal = _pattern.block.rotationFocusIncrementVal;
                break;
            default:
                break;
        }
        #endregion
        #region Rotation Intensity Effect
        switch (_pattern.block.rotationIntensityEffect)
        {
            case Pattern.Block.RotationIntensityEffect.Static:
                //Do Nothing
                break;
            case Pattern.Block.RotationIntensityEffect.Increment:
                trig.rotationIntensityIncrementVal = _pattern.block.rotationIntensityIncrementVal;
                break;
            default:
                break;
        }
        #endregion
    }

    bool CheckIfAtLastRoutine()
    {
        if (currentStep == routine[routine.Count - 1].stepPos)
        {
            if (enableSequenceLooping == true)
            {
                currentStep = reset;
                runningRoutine = reset;

                nextStep = routine[(int)runningRoutine + 1].stepPos;
                startStep = routine[(int)runningRoutine].stepPos;

                completedLoops++;
                progress = reset + (completedRoutines - completedLoops);
            }
            else
                ResetAllValues();
            completedRoutines++;
            return true;
        }

        return false;
    }

    void ResetAllValues()
    {
        //Get the pawn
        Pawn pawn = GetComponent<Pawn>();

        progress = (float)reset;
        currentStep = reset;
        nextStep = reset;
        startStep = reset;
        completedLoops = (int)reset;
        completedRoutines = (int)reset;
        runningRoutine = reset;
        enabled = false;
        trig.loop = false;

        //Reset pawn stat
        pawn.priority = pawn.basePriority;
        pawn.library.spellInUse = null;

        //Clear routine
        routine.Clear();
    }
}
