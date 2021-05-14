using System;
using System.Collections;
using UnityEngine;
using Extensions;

public enum StateInstructions
{
    NONE = -1,
    PLAY_STAGE_ANIMATION,
    PLAY_DISPLAY_ANIMATION,
    WAIT_UNTIL,
    PLAY_DIALOGUE,
    CREATE_ENEMY_INSTANCE,
    INITIATE_BOSS,
    CALL_EVENT,
    PLAY_MUSIC,
    PAUSE_MUSIC,
    PLAY_BOSS_MUSIC,
    PAUSE_BOSS_MUSIC,
    GOTO_STAGE,
    END
}

[CreateAssetMenu(fileName = "New Stage Map", menuName = "StageMap", order = 1)]
public class StageMap : ScriptableObject
{
    private static StageMap Instance;

    /* This class will handle the timing of enemy spawning, and where they spawn
     * as well as how to handle the scrolling and design of the stage's background.
     * This can mainly be used for the various of difficuties that a stage may provide
     * based on a given index. */
    

    private EventManager.Event[] @preBuilt_Events;

   
    public string mapName;

    [TextArea(2,2)]
    public string description;

    public AnimationClip stageAnimation;

    public GameObject[] nativeEnemies;

    public Dialogue dialogue;

    public StageEvent[] sequenceEvents;

    public static bool IsWaiting { get; private set; } = false;

    private float _currentInterval = -1f;
    private float _intervalDelta = 0.1f;

    private readonly int _screenWidth = Screen.width;
    private readonly int _screenHeight = Screen.height;

    private const int _MAX_COORD = 100;

    protected StageMap()
    {
        Init();
    }

    protected virtual void Init()
    {
        //TODO: Stage Configuration


        //Initiate Pre-Built Stage Events
        preBuilt_Events = new EventManager.Event[14];
        preBuilt_Events[0] = EventManager.AddEvent(700, "PLAY_STAGE_ANIMATION", PlayStageAnimation);
        preBuilt_Events[1] = EventManager.AddEvent(701, "PLAY_DISPLAY_ANIMATION", PlayDisplayNameAnimation);
        preBuilt_Events[2] = EventManager.AddEvent(702, "WAIT_UNTIL", WaitUntil);
        preBuilt_Events[3] = EventManager.AddEvent(703, "PLAY_DIALOGUE", PlayDialogue);
        preBuilt_Events[4] = EventManager.AddEvent(704, "CREATE_ENEMY_INSTANCE", CreateEnemyInstance);
        preBuilt_Events[5] = EventManager.AddEvent(705, "INITIATE_BOSS", InitaiateBoss);
        preBuilt_Events[6] = EventManager.AddEvent(706, "CALL_EVENT", CallEvent);
        preBuilt_Events[7] = EventManager.AddEvent(707, "PLAY_MUSIC", PlayMusic);
        preBuilt_Events[8] = EventManager.AddEvent(708, "PAUSE_MUSIC", PauseMusic);
        preBuilt_Events[9] = EventManager.AddEvent(709, "PLAY_BOSS_MUSIC", PlayBossMusic);
        preBuilt_Events[10] = EventManager.AddEvent(710, "PAUSE_BOSS_MUSIC", PauseBossMusic);
        preBuilt_Events[11] = EventManager.AddEvent(711, "GOTO_STAGE", GotoStage);
        preBuilt_Events[12] = EventManager.AddEvent(712, "END", End);
    }

    private void Close()
    {
        MainCycle().Stop();
        _intervalDelta = default;
        _currentInterval = default;
    }

    void Main()
    {
        for (int i = 0; i < sequenceEvents.Length - 1; i += IsWaiting ? 0 : i++)
        {
            if (sequenceEvents[i].Interval == _currentInterval)
            {
                int selectedEventCode = (int)sequenceEvents[i].m_event;
                preBuilt_Events[selectedEventCode].Trigger();
            }
        };
    }

    private void NextInterval() => _currentInterval++;

    IEnumerator MainCycle()
    {
        while (true)
        {
            try
            {
                Main();
                NextInterval();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            yield return new WaitForSeconds(_intervalDelta);
        }
    }

    static void PlayStageAnimation()
    {

    }
    static void PlayDisplayNameAnimation()
    {

    }
    static void WaitUntil()
    {

    }
    static void PlayDialogue()
    {

    }
    static void CreateEnemyInstance()
    {

    }
    static void InitaiateBoss()
    {

    }
    static void CallEvent()
    {

    }
    static void PlayMusic()
    {

    }
    static void PauseMusic()
    {

    }
    static void PlayBossMusic()
    {

    }
    static void PauseBossMusic()
    {

    }
    static void GotoStage()
    {

    }
    static void End()
    {

    }

    ~StageMap()
    {
        Close();
    }
}
