using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapRoutine : MonoBehaviour
{
    [SerializeField]
    private string snapShootDirectory;

    int i = 0;
    float time = 10f;
    float resetTime;
    // Start is called before the first frame update

    private void Start()
    {
        resetTime = time;
    }
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 1)
        {
            ScreenCapture.CaptureScreenshot(snapShootDirectory + i.ToString("D3") + ".png", 4);
            ++i;
            time = resetTime;
        }
    }
}
