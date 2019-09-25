using System.Collections;
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
    public int completedRoutines = 0;
    public int completedLoops = 0;

    public List<Spell.Routine> routine = new List<Spell.Routine>();

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

        if (currentStep > nextStep - 1)
        {
            if (!CheckIfAtLastRoutine())
            {
                runningRoutine++;
                completedRoutines++;
                nextStep = GetNextStep();
                startStep = GetPreviousStep();
            }

            //If at the end of last one

        }


        if (GetRoutineCompletionInPercentage() < 0.1f)
        {
            trig.loop = false;
            RunPattern(routine[(int)runningRoutine].pattern);
        }
    }

    public uint GetNextStep()
    {
        return routine[(int)runningRoutine + 1].stepPos;
    }

    public uint GetPreviousStep()
    {
        return routine[(int)runningRoutine].stepPos;
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

        progress = 0;
        trig.numberOfProjectiles = _pattern.block.amount;
        trig.loop = _pattern.block.loop;
        trig.loopSpeed = _pattern.block.loopRate;
        trig.bullet[0] = _pattern.block.bullet;
        trig.speed = _pattern.block.speed;
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
            case Pattern.Block.DistributionType.Scattered:
                trig.distribution = Shoot_Trig.DistributionType.Scattered;
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
            {
                enabled = false;
            }
            completedRoutines++;
            return true;
        }
        return false;
    }

    void ResetAllValues()
    {
        progress = reset;
        currentStep = reset;
        nextStep = reset;
        startStep = reset;
        completedLoops = (int)reset;
        completedRoutines = (int)reset;
        runningRoutine = reset;
    }
}
