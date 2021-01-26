using UnityEngine;

[CreateAssetMenu(fileName = "New Character Profile", menuName = "Character Profile")]
public class CharacterProfile : ScriptableObject
{

    [SerializeField]
    private string characterName;

    [SerializeField]
    private string characterDescription;

    [SerializeField]
    private Sprite characterPortrait;

    [SerializeField, TextArea(1, 10)]
    private string characterBackstory;

    [SerializeField]
    private Stats.StatsAttribute characterAttribute;

    [SerializeField, Range(1, 5)]
    private int speedStat, powerStat, annoyanceStat, priorityStat, magicStat, knowledgeStat, evasivenessStat;

    public string GetName()
    {
        return characterName;
    }

    public string GetDescription()
    {
        return characterDescription;
    }

    public Sprite GetPortrait()
    {
        return characterPortrait;
    }

    public string GetBackstory()
    {
        return characterBackstory;
    }

    public Stats.StatsAttribute GetAttribute()
    {
        return characterAttribute;
    }

    public int GetSpeed()
    {
        return speedStat;
    }

    public int GetPower()
    {
        return powerStat;
    }

    public int GetAnnoyance()
    {
        return annoyanceStat;
    }

    public int GetPriority()
    {
        return priorityStat;
    }

    public int GetMagic()
    {
        return magicStat;
    }

    public int GetKnowledge()
    {
        return knowledgeStat;
    }

    public int GetEvasiveness()
    {
        return evasivenessStat;
    }

    public Stats InitStatValues()
    {
        return Stats.New(speedStat, powerStat, annoyanceStat, priorityStat, magicStat, knowledgeStat, evasivenessStat, characterAttribute);
    }
}
