using System;
using UnityEngine;

[Serializable]
public class Stats
{
    public enum StatsAttribute
    {
        SPEED,
        POWER,
        ANNOYANCE,
        BASEPRIORITY,
        MAGIC,
        KNOWLEDGE,
        EVASIVENESS
    }


    int CurrentLevel = 1;

    //Base values is the value represented at its entirety
    //Enhancement Value is the amount of points added to the base values
    //Game value is the value calculated to affect the actual gameplay of the game (which are calculated in here)
    public int BaseSpeed { get; private set; }
    public int SpeedEnhancementValue { get; private set; } = ZERO;

    public int BasePower { get; private set; }
    public int PowerEnhancementValue { get; private set; } = ZERO;

    public int BaseAnnoyance { get; private set; }
    public int AnnoyanceEnhancementValue { get; private set; } = ZERO;

    public int BasePriority { get; private set; }
    public int PriorityEnhancementValue { get; private set; } = ZERO;

    public int BaseMagic { get; private set; }
    public int MagicEnhancementValue { get; private set; } = ZERO;

    public int BaseKnowledge { get; private set; }
    public int KnowledgeEnhancementValue { get; private set; } = ZERO;

    public int BaseEvasiveness { get; private set; }
    public int EvasivenessEnhancementValue { get; private set; } = ZERO;

    //Prominent Attribute bonus (25% of current attribute)
    const float PROMINENT_ATTRIBUTE_BONUS = 0.25f;

    //Ravens base prominentAttribute
    //Player can pick a prominentAttribute after the first play through
    //But can not change it until the end of the game.
    public StatsAttribute StartingProminentAttribute { get; set; }

    public static float SPEED_REDUCTION_CAP { get; private set; } = 0.80f;

    //Ravens Default Stats
    //Get 100 + number of plays points
    //And the bosses get much harder with the number of times you play
    const int DEFAULT_SPEED = 10;
    const int DEFAULT_POWER = 5;
    const int DEFAULT_ANNOYANCE = 3;
    const int DEFAULT_PRIORITY = 13;
    const int DEFAULT_MAGIC = 25;
    const int DEFAULT_KNOWLEDGE = 10;
    const int DEFAULT_EVASIVENESS = 2;

    public const int HIGH_RANK_VALUE = 20;
    public const int MIN_RATING = 1;
    public const int MAX_RATING = 5;
    const int ZERO = 0;
    const StatsAttribute DEFAULT_ATTRIBUTE = StatsAttribute.MAGIC;

    //Constructor (Will be given Raven's base stats)
    //ProminentAttribute Boost on start
    Stats(int speed, int power, int annoyance, int basePriority, int magic, int knowledge, int evasiveness, StatsAttribute prominentAttribute)
    {
        StartingProminentAttribute = prominentAttribute;

        BaseSpeed       = Mathf.RoundToInt(prominentAttribute == StatsAttribute.SPEED           ? speed         + (speed        * PROMINENT_ATTRIBUTE_BONUS)    : speed         );
        BasePower       = Mathf.RoundToInt(prominentAttribute == StatsAttribute.POWER           ? power         + (power        * PROMINENT_ATTRIBUTE_BONUS)    : power         );
        BaseAnnoyance   = Mathf.RoundToInt(prominentAttribute == StatsAttribute.ANNOYANCE       ? annoyance     + (annoyance    * PROMINENT_ATTRIBUTE_BONUS)    : annoyance     );
        BasePriority    = Mathf.RoundToInt(prominentAttribute == StatsAttribute.BASEPRIORITY    ? basePriority  + (basePriority * PROMINENT_ATTRIBUTE_BONUS)    : basePriority  );
        BaseMagic       = Mathf.RoundToInt(prominentAttribute == StatsAttribute.MAGIC           ? magic         + (magic        * PROMINENT_ATTRIBUTE_BONUS)    : magic         );
        BaseKnowledge   = Mathf.RoundToInt(prominentAttribute == StatsAttribute.KNOWLEDGE       ? knowledge     + (knowledge    * PROMINENT_ATTRIBUTE_BONUS)    : knowledge     );
        
        //The bonus doesn't go towards EVASIVNESS, rather it goes
        //to the SPEED_REDUCTION_CAP that plays a role in the EVASIVENESS stat
        BaseEvasiveness = evasiveness;
        SPEED_REDUCTION_CAP += PROMINENT_ATTRIBUTE_BONUS / 100;
    }

    /// <summary>
    /// Create new stat
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="power"></param>
    /// <param name="annoyance"></param>
    /// <param name="basePriority"></param>
    /// <param name="magic"></param>
    /// <param name="knowledge"></param>
    /// <param name="evasiveness"></param>
    /// <returns></returns>
    public static Stats New(
        int speed                           = DEFAULT_SPEED, 
        int power                           = DEFAULT_POWER, 
        int annoyance                       = DEFAULT_ANNOYANCE, 
        int basePriority                    = DEFAULT_PRIORITY, 
        int magic                           = DEFAULT_MAGIC, 
        int knowledge                       = DEFAULT_KNOWLEDGE, 
        int evasiveness                     = DEFAULT_EVASIVENESS, 
        StatsAttribute prominentAttribute   = DEFAULT_ATTRIBUTE)
    { return new Stats(speed, power, annoyance, basePriority, magic, knowledge, evasiveness, prominentAttribute);}

    /// <summary>
    /// Upgrade a value by a certain amount
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="amount"></param>
    public void Upgrade(StatsAttribute attribute, int amount)
    {
        switch (attribute)
        {
            case StatsAttribute.SPEED:
                SpeedEnhancementValue += Mathf.RoundToInt(StartingProminentAttribute == StatsAttribute.SPEED ? amount + PROMINENT_ATTRIBUTE_BONUS : amount);
                break;

            case StatsAttribute.POWER:
                PowerEnhancementValue += Mathf.RoundToInt(StartingProminentAttribute == StatsAttribute.POWER ? amount + PROMINENT_ATTRIBUTE_BONUS : amount);
                break;

            case StatsAttribute.ANNOYANCE:
                AnnoyanceEnhancementValue += Mathf.RoundToInt(StartingProminentAttribute == StatsAttribute.ANNOYANCE ? amount + PROMINENT_ATTRIBUTE_BONUS : amount);
                break;

            case StatsAttribute.BASEPRIORITY:
                PriorityEnhancementValue += Mathf.RoundToInt(StartingProminentAttribute == StatsAttribute.BASEPRIORITY ? amount + PROMINENT_ATTRIBUTE_BONUS : amount);
                break;

            case StatsAttribute.MAGIC:
                MagicEnhancementValue += Mathf.RoundToInt(StartingProminentAttribute == StatsAttribute.MAGIC ? amount + PROMINENT_ATTRIBUTE_BONUS : amount);
                break;

            case StatsAttribute.KNOWLEDGE:
                KnowledgeEnhancementValue += Mathf.RoundToInt(StartingProminentAttribute == StatsAttribute.KNOWLEDGE ? amount + PROMINENT_ATTRIBUTE_BONUS : amount);
                break;

            case StatsAttribute.EVASIVENESS:
                EvasivenessEnhancementValue += Mathf.RoundToInt(StartingProminentAttribute == StatsAttribute.EVASIVENESS ? amount + PROMINENT_ATTRIBUTE_BONUS : amount);
                break;

            default:
                break;
        }

        //Update Level
        UpdateLevel();
    }

    /// <summary>
    /// Get the current attribute value of player stat
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public int GetCurrentAttributeValue(StatsAttribute attribute)
    {
        int result = -1;
        switch (attribute)
        {
            case StatsAttribute.SPEED:
                result = BaseSpeed + SpeedEnhancementValue;
                break;

            case StatsAttribute.POWER:
                result = BasePower + PowerEnhancementValue;
                break;

            case StatsAttribute.ANNOYANCE:
                result = BaseAnnoyance + AnnoyanceEnhancementValue;
                break;

            case StatsAttribute.BASEPRIORITY:
                result = BasePriority + PriorityEnhancementValue;
                break;

            case StatsAttribute.MAGIC:
                result = BaseMagic + MagicEnhancementValue;
                break;

            case StatsAttribute.KNOWLEDGE:
                result = BaseKnowledge + KnowledgeEnhancementValue;
                break;

            case StatsAttribute.EVASIVENESS:
                result = BaseEvasiveness + EvasivenessEnhancementValue;
                break;

            default:
                return -1;
        }

        return result;
    }

    void UpdateLevel()
    {
        //We'll add all of the enhancedValue, and see if they are divisible by 5
        //That will be the level of our player
        int cumulativePoints = 
            (SpeedEnhancementValue + 
            PowerEnhancementValue + 
            AnnoyanceEnhancementValue + 
            PriorityEnhancementValue + 
            MagicEnhancementValue + 
            KnowledgeEnhancementValue + 
            EvasivenessEnhancementValue);

        CurrentLevel = cumulativePoints / 5;
    }

    public static int DetermineStatValueRating(int statValue)
    {
        //Any state that is at or above 30 is considered 5 star
        return Mathf.CeilToInt((float)statValue / (float)HIGH_RANK_VALUE * (float)MAX_RATING);
    }

    //Get the current level of player
    public int GetCurrentLevel() => CurrentLevel;
}
