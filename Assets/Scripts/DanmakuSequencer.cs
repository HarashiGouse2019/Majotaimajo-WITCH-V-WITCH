using System;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuSequencer : MonoBehaviour
{
    #region Public Members
    public bool allowOverride = false;

    public SequencerStatistics statistics = new SequencerStatistics();

    public SequenceCompletionTracker completion = new SequenceCompletionTracker();

    public List<Routine> routines;

    public Spell spellOrigin;

    public bool enableSequenceLooping;
    #endregion

    #region Private Members
    readonly int reset = 0;
    Emitter emitter;
    #endregion

    #region Properties
    public bool HasInitialized { get; private set; } = false;
    #endregion

    void Awake()
    {
        emitter = GetComponent<Emitter>();
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

    void IterateSequence()
    {
        if (statistics.currentStep != 0) statistics.startStep = GetPreviousStep();
        statistics.nextStep = GetNextStep();
        InvokeRepeating("Sequence", 0f, statistics.stepSpeed);
    }

    void PauseSequenceIteration()
    {
        CancelInvoke("Sequence");
    }

    void Sequence()
    {

        statistics.currentStep++;

        if (statistics.currentStep == statistics.nextStep)
        {
            if (!CheckIfAtLastRoutine())
            {
                statistics.runningRoutine++;
                completion.completedRoutines++;
                statistics.nextStep = GetNextStep();
                statistics.startStep = GetPreviousStep();
                completion.progress = 0;
            }
        }

        if (GetRoutineCompletionInPercentage() < 0.1f)
        {
            RunPattern(routines[(int)statistics.runningRoutine].pattern);
        }
    }

    public int GetNextStep()
    {
        Routine routineCheck = routines[statistics.runningRoutine + 1];

        RunPattern(routineCheck.pattern);

        return routineCheck.stepPos;
    }

    public int GetPreviousStep()
    {

        Routine routineCheck = routines[(int)statistics.runningRoutine];

        RunPattern(routineCheck.pattern);

        return routineCheck.stepPos;

    }

    public float GetRoutineCompletionInPercentage()
    {

        //Give me the percent between patterns
        completion.progress = (statistics.currentStep - statistics.startStep) / (statistics.nextStep - statistics.startStep);
        return completion.progress;

    }

    //Patterns
    void RunPattern(Pattern _pattern)
    {
        #region Assing Values
        emitter.SetProjectileDensity(_pattern.amount);

        emitter.SetLooping(_pattern.loop);

        emitter.SetLoopSpeed(_pattern.loopRate);

        emitter.SetBulletMember(_pattern.bulletType);

        emitter.SetBulletLimitSpeed(_pattern.speedLimit);

        if (!_pattern.carryOverSpeed && _pattern.initialSpeed < _pattern.speedLimit)
            emitter.SetBulletInitialSpeed(_pattern.initialSpeed);

        emitter.SetIncrementValue(_pattern.incrementalSpeed);

        if (_pattern.overrideRotation)
            emitter.SetAngle(_pattern.initialRotation);

        emitter.SetRotationFocus(_pattern.rotationFocus);

        emitter.SetRotationFocusLimit(_pattern.rotationFocusLimit);

        emitter.SetRotationIntensity(_pattern.rotationIntensity);

        emitter.SetRotationIntensityLimit(_pattern.rotationIntensityLimit);


        #region Rotation
        //No idea, but Imma do it!!
        switch ((int)_pattern.rotation)
        {
            case 0:
                emitter.SetRotationType(RotationType.NoRotation);
                break;

            case 1:
                emitter.SetRotationType(RotationType.ClockwiseI);
                break;

            case 2:
                emitter.SetRotationType(RotationType.ClockwiseII);
                break;

            case 3:
                emitter.SetRotationType(RotationType.ClockwiseIII);
                break;

            case -1:
                emitter.SetRotationType(RotationType.CounterClockwiseI);
                break;

            case -2:
                emitter.SetRotationType(RotationType.CounterClockwiseII);
                break;

            case -3:
                emitter.SetRotationType(RotationType.CounterClockwiseIII);
                break;

            default:
                break;
        }
        #endregion

        #region Distribution
        //And then again...
        switch (_pattern.distribution)
        {
            case DistributionType.Uniformed:
                emitter.SetDistributionType(DistributionType.Uniformed);
                break;

            case DistributionType.Biformed:
                emitter.SetDistributionType(DistributionType.Biformed);
                break;

            case DistributionType.UniformedIncrement:
                emitter.SetDistributionType(DistributionType.UniformedIncrement);
                break;

            case DistributionType.BiformedIncrement:
                emitter.SetDistributionType(DistributionType.BiformedIncrement);
                break;

            case DistributionType.Scattered:
                emitter.SetDistributionType(DistributionType.Scattered);
                break;

            default:
                break;
        }
        #endregion

        #region Rotation Focus Effect
        switch (_pattern.rotationFocusEffect)
        {
            case RotationFocusEffect.Static:
                //Do Nothing
                break;
            case RotationFocusEffect.Increment:
                emitter.SetRotationFocusIncrement(_pattern.rotationFocusIncrementVal);
                break;
            default:
                break;
        }
        #endregion

        #region Rotation Intensity Effect
        switch (_pattern.rotationIntensityEffect)
        {
            case RotationIntensityEffect.Static:
                //Do Nothing
                break;
            case RotationIntensityEffect.Increment:
                emitter.SetRotationIntensityIncrement(_pattern.rotationIntensityIncrementVal);
                break;
            default:
                break;
        }
        #endregion

        #endregion
    }

    bool CheckIfAtLastRoutine()
    {

        if (statistics.currentStep == routines[routines.Count - 1].stepPos)
        {
            return CheckEnabledLooping();
        }

        return false;
    }

    bool CheckEnabledLooping()
    {

        if (statistics.currentStep == routines[routines.Count - 1].stepPos)
        {
            if (enableSequenceLooping == true)
            {
                statistics.currentStep = reset;
                statistics.runningRoutine = reset;

                statistics.nextStep = routines[(int)statistics.runningRoutine + 1].stepPos;
                statistics.startStep = routines[(int)statistics.runningRoutine].stepPos;

                completion.completedLoops++;
                completion.progress = reset + (completion.completedRoutines - completion.completedLoops);
            }
            else
                ResetAllValues();
            completion.completedRoutines++;

            return true;
        }
        return false;
    }

    void ResetAllValues()
    {
        //Get the pawn
        Pawn pawn = emitter.ParentPawn;

        completion.progress = (float)reset;
        statistics.currentStep = reset;
        statistics.nextStep = reset;
        statistics.startStep = reset;
        completion.completedLoops = (int)reset;
        completion.completedRoutines = (int)reset;
        statistics.runningRoutine = reset;

        HasInitialized = false;

        enabled = false;

        emitter.ClearValues();

        //Reset pawn stat
        pawn.priority = pawn.basePriority;
        pawn.library.spellInUse = null;

        Clear();

    }

    void Clear()
    {
        routines.Clear();
    }

    public void CallReset()
    {
        ResetAllValues();
    }
}
