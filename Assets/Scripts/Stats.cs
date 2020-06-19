using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

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
    public int GameValueSpeed { get; private set; }

    public int BasePower { get; private set; }
    public int PowerEnhancementValue { get; private set; } = ZERO;
    public int GameValuePower { get; private set; }

    public int BaseAnnoyance { get; private set; }
    public int AnnoyanceEnhancementValue { get; private set; } = ZERO;
    public int GameValueAnnoyance { get; private set; }

    public int BasePriority { get; private set; }
    public int PriorityEnhancementValue { get; private set; } = ZERO;
    public int GameValuePriority { get; private set; }

    public int BaseMagic { get; private set; }
    public int MagicEnhancementValue { get; private set; } = ZERO;
    public int GameValueMagic { get; private set; }

    public int BaseKnowledge { get; private set; }
    public int KnowledgeEnhancementValue { get; private set; } = ZERO;
    public int GameValueKnowledge { get; private set; }

    public int BaseEvasiveness { get; private set; }
    public int EvasivenessEnhancementValue { get; private set; } = ZERO;
    public int GameValueEvasiveness { get; private set; }

    //Ravens base prominentAttribute
    public StatsAttribute StartingProminentAttribute { get; set; }

    //Ravens Default Stats
    const int DEFAULT_SPEED = 20;
    const int DEFAULT_POWER = 30;
    const int DEFAULT_ANNOYANCE = 15;
    const int DEFAULT_PRIORITY = 13;
    const int DEFAULT_MAGIC = 50;
    const int DEFAULT_KNOWLEDGE = 10;
    const int DEFAULT_EVASIVENESS = 10;
    const int ZERO = 0;
    const StatsAttribute DEFAULT_ATTRIBUTE = StatsAttribute.MAGIC;

    //Constructor (Will be given Raven's base stats)
    Stats(int speed, int power, int annoyance, int basePriority, int magic, int knowledge, int evasiveness, StatsAttribute prominentAttribute)
    {
        BaseSpeed = speed;
        BasePower = power;
        BaseAnnoyance = annoyance;
        BasePriority = basePriority;
        BaseMagic = magic;
        BaseKnowledge = knowledge;
        BaseEvasiveness = evasiveness;

        StartingProminentAttribute = prominentAttribute;

        //Update Game Values
        UpdateGameValues();

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
        int speed = DEFAULT_SPEED, 
        int power = DEFAULT_POWER, 
        int annoyance = DEFAULT_ANNOYANCE, 
        int basePriority = DEFAULT_PRIORITY, 
        int magic = DEFAULT_MAGIC, 
        int knowledge = DEFAULT_KNOWLEDGE, 
        int evasiveness = DEFAULT_EVASIVENESS, 
        StatsAttribute prominentAttribute = DEFAULT_ATTRIBUTE)
    {
        return new Stats(speed, power, annoyance, basePriority, magic, knowledge, evasiveness, prominentAttribute);
    }

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
                BaseSpeed += amount;
                break;

            case StatsAttribute.POWER:
                BasePower += amount;
                break;

            case StatsAttribute.ANNOYANCE:
                BaseAnnoyance += amount;
                break;

            case StatsAttribute.BASEPRIORITY:
                BasePriority += amount;
                break;

            case StatsAttribute.MAGIC:
                BaseMagic += amount;
                break;

            case StatsAttribute.KNOWLEDGE:
                BaseKnowledge += amount;
                break;

            case StatsAttribute.EVASIVENESS:
                BaseEvasiveness += amount;
                break;

            default:
                break;
        }

        //Update Values
        UpdateGameValues();

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

    void UpdateGameValues()
    {
        //Get our current values first
        int currentSpeed = GetCurrentAttributeValue(StatsAttribute.SPEED);
        int currentPower = GetCurrentAttributeValue(StatsAttribute.POWER);
        int currentAnnoyance = GetCurrentAttributeValue(StatsAttribute.ANNOYANCE);
        int currentPriority = GetCurrentAttributeValue(StatsAttribute.BASEPRIORITY);
        int currentMagic = GetCurrentAttributeValue(StatsAttribute.MAGIC);
        int currentKnowledge = GetCurrentAttributeValue(StatsAttribute.KNOWLEDGE);
        int currentEvasiveness = GetCurrentAttributeValue(StatsAttribute.EVASIVENESS);

        //This is where our game values are calculated for programming
        GameValueSpeed = currentSpeed + ((currentAnnoyance * currentEvasiveness) / currentKnowledge);
        GameValuePower = (currentPower * currentSpeed) / (currentPriority * currentMagic);
        GameValueAnnoyance = currentAnnoyance / ((currentSpeed * currentPower) / currentEvasiveness);
        GameValuePriority = currentPriority;
        GameValueMagic = currentMagic + ((currentPower * currentKnowledge) / currentSpeed);
        GameValueKnowledge = currentKnowledge;
        GameValueEvasiveness = currentEvasiveness + (currentAnnoyance / (currentSpeed * currentKnowledge));

        //test that it works
        Debug.Log(GameValueSpeed + "\n" +
                GameValuePower + "\n" +
                GameValueAnnoyance + "\n" +
                GameValuePriority + "\n" +
                GameValueMagic + "\n" +
                GameValueKnowledge + "\n" +
                GameValueEvasiveness + "\n");
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

    //Get the current level of player
    public int GetCurrentLevel() => CurrentLevel;
}
