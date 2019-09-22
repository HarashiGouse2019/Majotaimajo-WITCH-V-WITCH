using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

namespace DanmakuPattern
{
    public class Pattern : MonoBehaviour
    {
        /// <summary>
        /// Create a unique shooting routine.
        /// </summary>
        /// 
        /// <param name="amount"></param>
        /// How many bullets you want instaniated.
        /// 
        /// <param name="duration"></param>
        /// How long it takes for each bullet to instantiate.
        /// 
        /// <param name="timer"></param>
        /// A reference to a Timer class
        /// 
        /// <param name="size"></param>
        /// The size of the timer (how many total alarms there will be)
        /// 
        /// <param name="timerIndex"></param>
        /// The timer index for the set Timer class
        ///
        /// <param name="method">
        /// The method/function to call
        ///

        public static Pattern pattern;

        public Pattern(int amount = 1, float duration = 1, float angle = 0, Timer timer = null, int size = 1, int index = 0, Action method = null) {
            ConstructPattern(amount, duration, angle, timer, size, index, method);
        }

        public virtual void ConstructPattern(int _amount, float _duration, float angle, Timer _timer, int _size, int _index, Action _method) { }
        public virtual void ConstructPattern(ref int _amount, float _duration, float angle, Timer _timer, int _size, int _index, Action _method) { }
        public virtual void ConstructPattern(ref int _amount, ref int step, float _duration, float angle, Timer _timer, int _size, int _index, Action _method) { }
        public virtual void ConstructPattern(ref int _amount, ref int step, ref float angle, float _duration, Timer _timer, int _size, int _index, Action _method) { }
        public virtual void ConstructPattern(int _amount, ref int step, float _duration, float angle, Timer _timer, int _size, int _index, Action<int> _method) { }
        public virtual void ConstructPattern(int _amount, ref int step, ref float angle, float _duration,  Timer _timer, int _size, int _index, Action<int> _method) { }
    }
}
