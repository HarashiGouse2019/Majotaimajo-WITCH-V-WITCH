using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerGradeSystem : Singleton<PowerGradeSystem>
{
    /*TODO: Create a list of emitters from 
     * a character's Witch Sign that should be enabled at a certain power
     * level.*/

    [SerializeField]
    private TextMeshProUGUI level;

    [SerializeField]
    private Image powerMeter;

    private static int CurrentLevel;
    private static int PreviousLevel;
    private static float PowerMeterValue;
    private static bool initialized = false;

    private const int DEFAULT_LEVEL = 3;
    private const int MAX_LEVEL = 6;
    private const int MIN_LEVEL = 1;
    private const float MIN_FILL = 1f;
    private const float MAX_FILL = 6f;
    private const float HUNDRED_PERCENT = 100f;

    private const string romanStrings = "Ⅰ,Ⅱ,Ⅲ,Ⅳ,Ⅴ,Ω";
    private const char COMMA = ',';


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Update power meter
    /// </summary>
    static void UpdateMeterUI()
    {
        Instance.powerMeter.fillAmount = PowerMeterValue == MAX_FILL ? 
            MAX_FILL : 
            PowerMeterValue % MIN_FILL;

        UpdateLevelUI();
    }

    /// <summary>
    /// Update level UI
    /// </summary>
    static void UpdateLevelUI()
    {
        CurrentLevel = (int)(PowerMeterValue % MAX_LEVEL) - MIN_LEVEL;
        try
        {
            Instance.level.text = romanStrings.Split(COMMA)[CurrentLevel];
        }
        catch
        {
            CurrentLevel = MAX_LEVEL - MIN_LEVEL;
            Instance.level.text = romanStrings.Split(COMMA)[CurrentLevel];
        }

        CheckChange(initialized);
    }

    /// <summary>
    /// Checks if Previous Level is different from Current Level, and determines
    /// if it's a level up, or level down.
    /// </summary>
    static void CheckChange(bool playSound = true)
    {
        if (initialized == false) initialized = !initialized;

        if (PreviousLevel < CurrentLevel)
        {
            //Play level Up noise
            if (playSound) AudioManager.Play("LevelUp", _oneShot: true);
            PreviousLevel = CurrentLevel;
        }
        else if (PreviousLevel > CurrentLevel)
        {
            //Play level Down noise
            if (playSound) AudioManager.Play("LevelDown", _oneShot: true);
            PreviousLevel = CurrentLevel;
        }
    }

    /// <summary>
    /// Add to a character's power meter 
    /// </summary>
    public static void IncreasePower(float value)
    {
        PowerMeterValue += CurrentLevel == MAX_LEVEL ? MAX_FILL : value / HUNDRED_PERCENT;
        PowerMeterValue = Mathf.Clamp(PowerMeterValue, MIN_FILL, MAX_FILL);
        UpdateMeterUI();
    }

    /// <summary>
    /// Subtract from a character's power meter
    /// </summary>
    public static void DecreasePower(float value)
    {
        PowerMeterValue -= CurrentLevel < MIN_LEVEL-MIN_LEVEL ? MIN_FILL-MIN_FILL: value / HUNDRED_PERCENT;
        PowerMeterValue = Mathf.Clamp(PowerMeterValue, MIN_FILL, MAX_FILL);
        UpdateMeterUI();
    }

    /// <summary>
    /// Start up Power Grade System
    /// </summary>
    void Init()
    {
        PowerMeterValue = DEFAULT_LEVEL;

        UpdateMeterUI();
        UpdateLevelUI();
    }
}
