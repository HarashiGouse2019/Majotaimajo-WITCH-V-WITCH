using System.Collections;
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
    RotationEmitter trig;
    #endregion

    void Awake()
    {
        trig = GetComponent<RotationEmitter>();
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
        Spell.Routine routineCheck = routine[(int)runningRoutine + 1];

        RunPattern(routineCheck.pattern);

        return routineCheck.stepPos;
    }

    public uint GetPreviousStep()
    {
        Spell.Routine routineCheck = routine[(int)runningRoutine];

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
        int index = 0;
        #region Assing Values
        foreach (Pattern.Block block in _pattern.blocks)
        {
            trig.SetProjectileDensity(block.amount);

            trig.SetLooping(block.loop);

            trig.SetLoopSpeed(block.loopRate);

            trig.SetBulletMember(block.bulletType);

            trig.SetBulletLimitSpeed(block.speedLimit);

            if (!block.carryOverSpeed && block.initialSpeed < block.speedLimit)
                trig.SetBulletInitialSpeed(block.initialSpeed);

            trig.SetIncrementValue(block.incrementalSpeed);

            if (block.overrideRotation)
                trig.SetAngle(block.initialRotation);

            trig.SetRotationFocus(block.rotationFocus);

            trig.SetRotationFocusLimit(block.rotationFocusLimit);

            trig.SetRotationIntensity(block.rotationIntensity);

            trig.SetRotationIntensityLimit(block.rotationIntensityLimit);


            #region Rotation
            //No idea, but Imma do it!!
            switch ((int)block.rotation)
            {
                case 0:
                    trig.SetRotationType(RotationType.NoRotation);
                    break;

                case 1:
                    trig.SetRotationType(RotationType.ClockwiseI);
                    break;

                case 2:
                    trig.SetRotationType(RotationType.ClockwiseII);
                    break;

                case 3:
                    trig.SetRotationType(RotationType.ClockwiseIII);
                    break;

                case -1:
                    trig.SetRotationType(RotationType.CounterClockwiseI);
                    break;

                case -2:
                    trig.SetRotationType(RotationType.CounterClockwiseII);
                    break;

                case -3:
                    trig.SetRotationType(RotationType.CounterClockwiseIII);
                    break;

                default:
                    break;
            }
            #endregion

            #region Distribution
            //And then again...
            switch (block.distribution)
            {
                case Pattern.Block.DistributionType.Uniformed:
                    trig.SetDistributionType(DistributionType.Uniformed);
                    break;

                case Pattern.Block.DistributionType.Biformed:
                    trig.SetDistributionType(DistributionType.Biformed);
                    break;

                case Pattern.Block.DistributionType.UniformedIncrement:
                    trig.SetDistributionType(DistributionType.UniformedIncrement);
                    break;

                case Pattern.Block.DistributionType.BiFormedIncrement:
                    trig.SetDistributionType(DistributionType.BiformedIncrement);
                    break;

                case Pattern.Block.DistributionType.Scattered:
                    trig.SetDistributionType(DistributionType.Scattered);
                    break;

                default:
                    break;
            }
            #endregion

            #region Rotation Focus Effect
            switch (block.rotationFocusEffect)
            {
                case Pattern.Block.RotationFocusEffect.Static:
                    //Do Nothing
                    break;
                case Pattern.Block.RotationFocusEffect.Increment:
                    trig.SetRotationFocusIncrement(block.rotationFocusIncrementVal);
                    break;
                default:
                    break;
            }
            #endregion

            #region Rotation Intensity Effect
            switch (block.rotationIntensityEffect)
            {
                case Pattern.Block.RotationIntensityEffect.Static:
                    //Do Nothing
                    break;
                case Pattern.Block.RotationIntensityEffect.Increment:
                    trig.SetRotationIntensityIncrement(block.rotationIntensityIncrementVal);
                    break;
                default:
                    break;
            }
            #endregion

            index++;
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

        trig.ClearValues();

        //Reset pawn stat
        pawn.priority = pawn.basePriority;
        pawn.library.spellInUse = null;

        Clear();

    }

    void Clear()
    {
        routine.Clear();
    }

    public void CallReset()
    {
        ResetAllValues();
    }
}
