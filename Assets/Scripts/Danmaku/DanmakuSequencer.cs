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

    ICaster caster;

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
        try
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
                RunPattern(routines[statistics.runningRoutine].pattern);
            }
        }
        catch
        {
            return;
        }
    }

    public int GetNextStep()
    {
        try
        {
            Routine routineCheck = routines[statistics.runningRoutine + 1];

            RunPattern(routineCheck.pattern);

            return routineCheck.stepPos;
        }
        catch
        {
            return -1;
        }
    }

    public int GetPreviousStep()
    {

        Routine routineCheck = routines[statistics.runningRoutine];

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
        emitter.SetCaster(caster);

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

        emitter.SetSound(_pattern.soundName);

        #region Rotation
        emitter.SetRotationType(_pattern.rotation);
        #endregion

        #region Distribution

        emitter.SetDistributionType(_pattern.distribution);
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

        #region Spawning Spacing and Count, etc
        emitter.SetSpawnXInterval(_pattern.spawningXInterval);
        emitter.SetSpawnYInterval(_pattern.spawningYInterval);
        emitter.SetIntervalCountLimit(_pattern.spawningCountLimit);
        emitter.SetProjectileLifeTime(_pattern.projectileLifeTime);
        emitter.SetAnimateProjectileOnDestroy(_pattern.animateOnDestroy);
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

                statistics.nextStep = routines[statistics.runningRoutine + 1].stepPos;
                statistics.startStep = routines[statistics.runningRoutine].stepPos;

                completion.completedLoops++;
                completion.progress = reset + (completion.completedRoutines - completion.completedLoops);

                emitter.ResetIntervalCount();
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

        completion.progress = reset;
        statistics.currentStep = reset;
        statistics.nextStep = reset;
        statistics.startStep = reset;
        completion.completedLoops = reset;
        completion.completedRoutines = reset;
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

    /// <summary>
    /// Set the caster origin
    /// </summary>
    /// <param name="caster"></param>
    public void SetCaster(ICaster caster)
    {
        this.caster = caster;
    }
    public void CallReset()
    {
        ResetAllValues();
    }
}
