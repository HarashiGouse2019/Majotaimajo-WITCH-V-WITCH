using UnityEngine;
using TMPro;
using System.Collections;
using Extensions;

public class FPSCounter : Singleton<FPSCounter>
{
    //FrameRate Options
    public enum FrameRate
    {
        FPS30 = 30,
        FPS60 = 60,
        FPS120 = 120,
        UNLIMITED = -1
    }

    //Shows the current fps of the game
    [SerializeField]
    private FrameRate targetFrameRate = FrameRate.FPS60;
    private static int fpsAccumulator = 0;
    private static float fpsNextPeriod = 0f;
    private static int currentFPS;

    //TextMeshPro
    [SerializeField]
    private TextMeshProUGUI fpsText;

    //Constants
    const float fpsMeasurePeriod = 0.5f;
    const string display = "[{0}] FPS";

    void Awake()
    {
        Application.targetFrameRate = (int)targetFrameRate;
    }

    // Start is called before the first frame update
    void Start()
    {
        FPSCounterCycle().Start();
    }

    IEnumerator FPSCounterCycle()
    {
        fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;

        while (true)
        {
            //measure average frames per second
            fpsAccumulator++;

            if (fpsText != null && Time.realtimeSinceStartup > fpsNextPeriod)
            {
                currentFPS = (int)(fpsAccumulator / fpsMeasurePeriod);
                fpsAccumulator = 0;
                fpsNextPeriod += fpsMeasurePeriod;
                fpsText.text = string.Format(display, currentFPS);
            }

            yield return new WaitForSeconds(0.001f);
        }
    }

    public static int GetCurrectFPS() => currentFPS;
}
