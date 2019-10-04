using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class Disperse : Shoot_Trig
{
    readonly Timer timer = new Timer(1);
    // Start is called before the first frame update
    private void Update()
    {
        timer.StartTimer(0);
        if (timer.SetFor(2f, 0))
        {
            SpawnBullets(numberOfProjectiles, bulletMember);
        }
    }
}
