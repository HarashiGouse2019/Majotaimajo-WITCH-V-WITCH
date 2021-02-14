using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private string stageName;

    [SerializeField]
    private MusicTheme stageTheme, stageBossTheme;

    [SerializeField]
    private StageMap[] difficultyStageMaps = new StageMap[3];

    private void OnEnable()
    {
        LoadStageMap();
        PlayStageTheme();
    }

    public void PlayStageTheme()
    {
        stageTheme.Play();
    }

    public void PlayBossTheme()
    {
        stageBossTheme.Play();
    }

    public StageMap LoadStageMap()
    {
        StageMap stageToLoad = difficultyStageMaps[GameManager.DifficultyIndex];
        
        Debug.Log($"Loaded in StageMap: {stageToLoad.MapName}");
        return stageToLoad;
    }
}
