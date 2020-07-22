﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class CheckDestroy : MonoBehaviour
{

    #region Private Members
    private Timer destroyTimer;
    private GameObject origin;
    #endregion

    private void OnEnable()
    {
        destroyTimer.StartTimer(0);
        if (destroyTimer.currentTime[0] > 10)
        {
            gameObject.SetActive(false);
            destroyTimer.SetToZero(0, true);
        }
    }

    void Awake()
    {
        destroyTimer = new Timer(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        origin = FindObjectOfType<RotationEmitter>().GetOriginObject(); //Will find the gameObject that shoot the bullet out
    }

}
