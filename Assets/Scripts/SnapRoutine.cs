using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapRoutine : MonoBehaviour
{
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
            ScreenCapture.CaptureScreenshot(@"C:\Users\Tokusunei\Documents\Repository Folder\GitHub\Unity\Majoutaimajou-Witch-V-Witch\Majotaimajo-WITCH-V-WITCH\魔女対魔女（WITCH V WITCH)\SnapShoots" + i.ToString("D3") + ".png", 4);
            ++i;
            time = resetTime;
        }
    }
}
