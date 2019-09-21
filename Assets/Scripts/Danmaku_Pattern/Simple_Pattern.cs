using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakuPattern;
using Alarm;

public class Simple_Pattern : Pattern
{
    public Simple_Pattern(int amount = 1, float duration = 1, float angle = 0, Timer timer = null, int size = 1, int index = 0, Action method = null)
    {

            ConstructPattern(amount, duration, angle, timer, size, index, method);
    }

    public override void ConstructPattern(int _amount, float _duration, float _angle, Timer _timer, int _size, int _index, Action _method)
    {
        //Here's the pattern (or loop) we'll use.
        if (_timer == null) {
            _timer = new Timer(_size, true);
        }

        //loopTimer.StartTimer(0);
        //if (loopTimer.currentTime[0] > loopSpeed)
        //{
        //    SpawnBullets(numberOfProjectiles);
        //    loopTimer.timeStarted[0] = false;
        //    loopTimer.currentTime[0] = 0;
        //}
        //Now we convert that to this set up
            _timer.StartTimer(_index);
            if (_timer.currentTime[_index] > _duration)
            {
                _method();
                _timer.timeStarted[_index] = false;
                _timer.currentTime[_index] = 0;
            }
        

        //Let's see if that works!!!
    }
}
