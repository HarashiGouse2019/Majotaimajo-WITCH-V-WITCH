using UnityEngine;
using SequenceEventUtility;

[CreateAssetMenu(fileName = "New Stage Map", menuName = "StageMap", order = 1)]
public class StageMap : ScriptableObject
{
    private static StageMap Instance;

    /* This class will handle the timing of enemy spawning, and where they spawn
     * as well as how to handle the scrolling and design of the stage's background.
     * This can mainly be used for the various of difficuties that a stage may provide
     * based on a given index. */
    
    public string mapName;

    [TextArea(2,2)]
    public string description;

    public AnimationClip stageAnimation;

    public NativeEnemy[] nativeEnemies = new NativeEnemy[1];

    public Dialogue dialogue;

    public SequenceEventScript _eventScript;

    private readonly int _screenWidth = Screen.width;
    private readonly int _screenHeight = Screen.height;

    private const int _MAX_COORD = 100;

    protected void Init()
    {
        //TODO: Stage Configuration (see trello card for more detail)
        _eventScript.Ready();
    }

    public void Setup()
    {
        Init();
    }
}
