using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private string stageName;

    [SerializeField]
    private MusicTheme stageTheme, stageBossTheme;

    [SerializeField]
    private StageMap[] difficultyStageMaps = new StageMap[3];

    public void PlayStageTheme()
    {
        stageTheme.Play();
    }

    public void PlayBossTheme()
    {
        stageBossTheme.Play();
    }

    public StageMap LoadStageMap(string mapName)
    {
        StageMap mapToLoad = Find(mapName);
        //TODO: Do whatever with this data;

        return mapToLoad;
    }

    StageMap Find(string name)
    {
        if (difficultyStageMaps != null || difficultyStageMaps.Length == 0)
            return null;

        for (int index = 0; index < difficultyStageMaps.Length; index++)
        {
            StageMap currentStageMap = difficultyStageMaps[index];
            if (currentStageMap.MapName.Equals(name))
                return currentStageMap;
        }

        return null;
    }
}
