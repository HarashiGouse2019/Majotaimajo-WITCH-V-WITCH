
public class SequencerStatistics
{
    public float startStep = 0;

    public float currentStep = 0;

    public float nextStep = 0;

    public float stepSpeed = 1f;

    public int runningRoutine;

    public SequencerStatistics()
    {
        startStep = 0;
        currentStep = 0;
        nextStep = 0;
        stepSpeed = 1f;
        runningRoutine = 0;
    }
}

public class SequenceCompletionTracker
{
    public float progress;

    public int completedRoutines;

    public int completedLoops;

    public SequenceCompletionTracker()
    {
        progress = 0;
        completedRoutines = 0;
        completedLoops = 0;
    }
}
