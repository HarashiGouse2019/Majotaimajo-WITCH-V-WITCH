using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakuPattern;
using Alarm;

public class Star_Pattern : Pattern
{
    /// <summary>
    /// A pattern that draws out a star
    /// 2 timers are needed!!!!!!
    /// </summary>

    public Star_Pattern(ref int amount, ref int step, float duration = 0.5f, float angle = 0, Timer timer = null, int size = 1, int index = 0, Action method = null)
    {

        ConstructPattern(ref amount, ref step, duration, angle, timer, size, index, method);
    }

    public override void ConstructPattern(ref int _amount, ref int _step, float _duration, float _angle, Timer _timer, int _size,int _index, Action _method)
    {
        //And create a new timer

        if (_timer == null)
        {
            _timer = new Timer(_size, true);
        }

        //Do a routine for timer 0
        _timer.StartTimer(0);

        if (_timer.currentTime[0] > _duration)
        {
            if (_step == 0) { _amount = 5; _step++; _method(); _timer.currentTime[0] = 0;}
            else if (_step == 1) { _amount = 10; _step++; _method(); _timer.currentTime[0] = 0; }
            else if (_step == 2) { _amount = 15; _step=0; _method(); _timer.currentTime[0] = 0; }
        }
        //Let's see if that works!!!
    }
}
