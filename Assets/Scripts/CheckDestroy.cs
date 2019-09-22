using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class CheckDestroy : MonoBehaviour
{

    #region Private Members
    private Timer destroyTimer;
    private GameObject origin;
    #endregion

    void Awake()
    {
        destroyTimer = new Timer(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        origin = FindObjectOfType<Shoot_Trig>().origin; //Will find the gameObject that shoot the bullet out
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer.StartTimer(0);
        if (destroyTimer.currentTime[0] > 8)
        {
            origin.GetComponent<Shoot_Trig>().Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
