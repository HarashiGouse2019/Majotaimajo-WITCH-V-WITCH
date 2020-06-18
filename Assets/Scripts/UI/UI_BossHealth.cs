using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealth : MonoBehaviour
{
    private static UI_BossHealth Instance;

    [SerializeField, Tooltip("The slider associated with the boss' health.")]
    private Slider bossHealthSlider;

    [SerializeField, Tooltip("The text mesh pro associated with how much to lower her hp.")]
    private TextMeshProUGUI bossLayerCount;

    //Values used to size slider between values 0 and 1
    public static float Value { get; private set; }
    public static float MaxValue { get; private set; }

    //Health Counter (Acts as a layer)
    public static uint Layer { get; private set; }

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
    public static Slider GetSlider() => Instance.bossHealthSlider;

    /// <summary>
    /// Set the Value of Slider
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    public static void SetValue(float value)
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
    public static void SetMaxValue(float value)
    {
        MaxValue = value;

        //Update Ui
        UpdateSliderValues();
    }

    /// <summary>
    /// Set's how many layers of their HP do they have.
    /// </summary>
    /// <param name="value"></param>
    public static void SetLayerCount(uint value)
    {
        Layer = value;

        //Update UI
        UpdateLayerValue();
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

    /// <summary>
    /// Update value that represents the layer of HP
    /// </summary>
    static void UpdateLayerValue()
    {
        //Update the text
        Instance.bossLayerCount.text = Layer.ToString();
    }

}
