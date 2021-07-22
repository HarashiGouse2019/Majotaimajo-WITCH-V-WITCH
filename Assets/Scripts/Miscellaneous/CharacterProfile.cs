using System;
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

    [SerializeField]
    private int speedStat, powerStat, annoyanceStat, priorityStat, magicStat, knowledgeStat, evasivenessStat;

    [SerializeField]
    private RuntimeAnimatorController characterAnimationController;

    [SerializeField]
    private PlayerPawn playerPawn;

    public int SpeedRating
    {
        get
        {
            return Mathf.Clamp(Stats.DetermineStatValueRating(speedStat), Stats.MIN_RATING, Stats.MAX_RATING);
        }
    }
    public int PowerRating
    {
        get
        {
            return Mathf.Clamp(Stats.DetermineStatValueRating(powerStat), Stats.MIN_RATING, Stats.MAX_RATING);
        }
    }
    public int AnnoyanceRating
    {
        get
        {
            return Mathf.Clamp(Stats.DetermineStatValueRating(annoyanceStat), Stats.MIN_RATING, Stats.MAX_RATING);
        }
    }
    public int PriorityRating
    {
        get
        {
            return Mathf.Clamp(Stats.DetermineStatValueRating(priorityStat), Stats.MIN_RATING, Stats.MAX_RATING);
        }
    }


    public int MagicRating
    {
        get
        {
            return Mathf.Clamp(Stats.DetermineStatValueRating(magicStat), Stats.MIN_RATING, Stats.MAX_RATING);
        }
    }
    public int KnowledgeRating
    {
        get
        {
            return Mathf.Clamp(Stats.DetermineStatValueRating(knowledgeStat), Stats.MIN_RATING, Stats.MAX_RATING);
        }
    }
    public int EvasivenessRating
    {
        get
        {
            return Mathf.Clamp(Stats.DetermineStatValueRating(evasivenessStat), Stats.MIN_RATING, Stats.MAX_RATING);
        }
    }



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

    public RuntimeAnimatorController GetRAC()
    {
        return characterAnimationController;
    }

    internal PlayerPawn GetPlayerPawn()
    {
        return playerPawn;
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

    private void OnValidate()
    {
        switch (characterAttribute)
        {
            case Stats.StatsAttribute.SPEED:
                speedStat = Stats.HIGH_RANK_VALUE;
                break;
            case Stats.StatsAttribute.POWER:
                powerStat = Stats.HIGH_RANK_VALUE;
                break;
            case Stats.StatsAttribute.ANNOYANCE:
                annoyanceStat = Stats.HIGH_RANK_VALUE;
                break;
            case Stats.StatsAttribute.BASEPRIORITY:
                priorityStat = Stats.HIGH_RANK_VALUE;
                break;
            case Stats.StatsAttribute.MAGIC:
                magicStat = Stats.HIGH_RANK_VALUE;
                break;
            case Stats.StatsAttribute.KNOWLEDGE:
                knowledgeStat = Stats.HIGH_RANK_VALUE;
                break;
            case Stats.StatsAttribute.EVASIVENESS:
                evasivenessStat = Stats.HIGH_RANK_VALUE;
                break;
            default:
                break;
        }
    }
}
