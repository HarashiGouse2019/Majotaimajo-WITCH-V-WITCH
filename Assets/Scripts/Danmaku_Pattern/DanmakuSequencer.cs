using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TreeEditor;
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
        #region Assing Values
        foreach (Pattern.Block block in _pattern.blocks)
        {
            trig.numberOfProjectiles = block.amount;

            trig.loop = block.loop;

            trig.loopSpeed = block.loopRate;

            trig.bulletMember = block.bulletType;

            trig.speedLimit = block.speedLimit;

            if (!block.carryOverSpeed && block.initialSpeed < block.speedLimit)
                trig.speed = block.initialSpeed;

            trig.incrementVal = block.incrementalSpeed;

            if (block.overrideRotation)
                trig.g_angle = block.initialRotation;

            trig.rotationFocus = block.rotationFocus;

            trig.rotationFocusLimit = block.rotationFocusLimit;

            trig.rotationIntensity = block.rotationIntensity;

            trig.rotationIntensityLimit = block.rotationIntensityLimit;


            #region Rotation
            //No idea, but Imma do it!!
            switch ((int)block.rotation)
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
            switch (block.distribution)
            {
                case Pattern.Block.DistributionType.Uniformed:
                    trig.distribution = Shoot_Trig.DistributionType.Uniformed;
                    break;

                case Pattern.Block.DistributionType.Biformed:
                    trig.distribution = Shoot_Trig.DistributionType.Biformed;
                    break;

                case Pattern.Block.DistributionType.UniformedIncrement:
                    trig.distribution = Shoot_Trig.DistributionType.UniformedIncrement;
                    break;

                case Pattern.Block.DistributionType.BiFormedIncrement:
                    trig.distribution = Shoot_Trig.DistributionType.BiformedIncrement;
                    break;

                case Pattern.Block.DistributionType.Scattered:
                    trig.distribution = Shoot_Trig.DistributionType.Scattered;
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
                    trig.rotationFocusIncrementVal = block.rotationFocusIncrementVal;
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
                    trig.rotationIntensityIncrementVal = block.rotationIntensityIncrementVal;
                    break;
                default:
                    break;
            }
            #endregion
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
        Debug.Log("Resetting");
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
        trig.distribution = default;
        trig.rotation = default;
        trig.speed = reset;
        trig.speedLimit = (int)reset;
        trig.loopSpeed = reset;
        trig.numberOfProjectiles = 1;
        trig.g_angle = reset;
        trig.incrementVal = (int)reset;
        trig.rotationFocus = reset;
        trig.rotationIntensity = reset;
        trig.rotationFocusIncrementVal = reset;
        trig.rotationIntensityIncrementVal = reset;
        trig.rotationIntensityLimit = reset;
        trig.rotationFocusLimit = reset;

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
