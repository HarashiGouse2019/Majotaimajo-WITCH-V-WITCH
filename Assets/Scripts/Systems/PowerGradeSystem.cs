using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerGradeSystem : Singleton<PowerGradeSystem>
{
    [SerializeField]
    private PlayerSpawner spawner;

    [SerializeField]
    private TextMeshProUGUI level;

    [SerializeField]
    private Image powerMeter;

    private static int _CurrentLevel;
    public static int CurrentLevel
    {
        get
        {
            return _CurrentLevel;
        }
    }

    private static int PreviousLevel;
    private static float PowerMeterValue;
    private static bool initialized = false;

    private const int DEFAULT_LEVEL = 1;
    private const int MAX_LEVEL = 6;
    private const int MIN_LEVEL = 1;
    private const float MIN_FILL = 1f;
    private const float MAX_FILL = 6f;
    private const float HUNDRED_PERCENT = 100f;

    private const string romanStrings = "Ⅰ,Ⅱ,Ⅲ,Ⅳ,Ⅴ,Ω";
    private const char COMMA = ',';

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            IncreasePower(5f);
        if (Input.GetKeyDown(KeyCode.Q))
            DecreasePower(5f);
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
        _CurrentLevel = (int)(PowerMeterValue % MAX_LEVEL) - MIN_LEVEL;
        try
        {
            Instance.level.text = romanStrings.Split(COMMA)[_CurrentLevel];
        }
        catch
        {
            _CurrentLevel = MAX_LEVEL - MIN_LEVEL;
            Instance.level.text = romanStrings.Split(COMMA)[_CurrentLevel];
        }

        CheckChange(initialized);
    }

    /// <summary>
    /// Checks if Previous Level is different from Current Level, and determines
    /// if it's a level up, or level down.
    /// </summary>
    static void CheckChange(bool playSound = true)
    {
        if (initialized == false)
        {
            initialized = !initialized;
            PreviousLevel = _CurrentLevel;

            if (PlayerSpawner.PlayerInstance)
                PlayerSpawner.PlayerInstance.EmitterCollection.ToggleEmittersAtCurrentLevel();

            return;
        }

        if (PreviousLevel < _CurrentLevel)
        {
            if (playSound) AudioManager.Play("LevelUp", _oneShot: true);
            PreviousLevel = _CurrentLevel;
            PlayerSpawner.PlayerInstance.EmitterCollection.ToggleEmittersAtCurrentLevel();
        }
        else if (PreviousLevel > _CurrentLevel)
        {
            //Play level Down noise
            if (playSound) AudioManager.Play("LevelDown", _oneShot: true);
            PreviousLevel = _CurrentLevel;
            PlayerSpawner.PlayerInstance.EmitterCollection.ToggleEmittersAtCurrentLevel();
        }
    }

    /// <summary>
    /// Add to a character's power meter 
    /// </summary>
    public static void IncreasePower(float value)
    {
        PowerMeterValue += _CurrentLevel == MAX_LEVEL ? MAX_FILL : value / HUNDRED_PERCENT;
        PowerMeterValue = Mathf.Clamp(PowerMeterValue, MIN_FILL, MAX_FILL);
        UpdateMeterUI();
    }

    /// <summary>
    /// Subtract from a character's power meter
    /// </summary>
    public static void DecreasePower(float value)
    {
        PowerMeterValue -= _CurrentLevel < MIN_LEVEL-MIN_LEVEL ? MIN_FILL-MIN_FILL: value / HUNDRED_PERCENT;
        PowerMeterValue = Mathf.Clamp(PowerMeterValue, MIN_FILL, MAX_FILL);
        UpdateMeterUI();
    }

    /// <summary>
    /// Start up Power Grade System
    /// </summary>
    public static void Init()
    {
        PowerMeterValue = DEFAULT_LEVEL;
        UpdateMeterUI();
    }
}
