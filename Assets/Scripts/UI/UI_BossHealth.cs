using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealth : MonoBehaviour
{
    private static UI_BossHealth Instance;

    [SerializeField]
    private Slider bossHealthSlider;

    //Values used to size slider between values 0 and 1
    public static float Value { get; private set; }
    public static float MaxValue { get; private set; }

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        } 
        #endregion
    }

    /// <summary>
    /// Get the slider associated with this UI
    /// </summary>
    static Slider GetSlider() => Instance.bossHealthSlider;

    /// <summary>
    /// Set the Value of Slider
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    static void SetValue(float value)
    {
        Value = value;

        //Update Ui
        UpdateSliderValues();
    }

    /// <summary>
    /// Set the Max Value of Slider
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    static void SetMaxValue(float value)
    {
        MaxValue = value;

        //Update Ui
        UpdateSliderValues();
    }

    /// <summary>
    /// Updates the sliderUi with the set value and maxValue.
    /// Only call this when you need to update, since UI calls are 
    /// taxing to the processor.
    /// </summary>
    static void UpdateSliderValues()
    {
        //Update the slider
        Instance.bossHealthSlider.maxValue = (float)MaxValue;
        Instance.bossHealthSlider.value = (float)Value;
    }

}
