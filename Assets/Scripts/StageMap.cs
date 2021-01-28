using UnityEngine;

[CreateAssetMenu(fileName = "New Stage Map", menuName = "StageMap")]
public class StageMap : ScriptableObject
{
    /* This class will handle the timing of enemy spawning, and where they spawn
     * as well as how to handle the scrolling and design of the stage's background.
     * This can mainly be used for the various of difficuties that a stage may provide
     * based on a given index. */

    [SerializeField]
    private string mapName;

    public string MapName => mapName;
}
