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
        stageTheme.PlayTheme();
    }

    public void PlayBossTheme()
    {
        stageBossTheme.PlayTheme();
    }

    public StageMap LoadStageMap()
    {
        StageMap stageToLoad = difficultyStageMaps[GameManager.DifficultyIndex];
        GameManager.UpdateCurrentStage(this);
        stageToLoad.Setup();
        return stageToLoad;
    }
}
