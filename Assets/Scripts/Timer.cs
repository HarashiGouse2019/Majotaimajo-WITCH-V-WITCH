using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alarm
{
    [System.Serializable]
    public class Timer
    {
        #region Public Members
        public static Timer timer;
        /// <summary>
        /// The time since the timer was activated.
        /// </summary>
        public float[] currentTime;
        public float[] resetTime;

        /// <summary>
        /// If the current time has started
        /// </summary>
        public bool[] timeStarted; //If the time has started


        #endregion

        #region Private Members
        private int size;
        readonly private bool debugEnabled = false;
        readonly private bool showFloatingPoint = false;
        #endregion

        //Timer Constructor
        public Timer(int size)
        {
            SetSize(size);
            Initialize(GetSize());
        }

        public Timer(int size, bool debug)
        {
            SetSize(size);
            debugEnabled = debug;
            switch (debug)
            {
                case false:
                    Initialize(GetSize());
                    break;
                case true:
                    Initialize(GetSize());
                    UnityEngine.Debug.Log("Timer(s) Initiated.");
                    UnityEngine.Debug.Log("Timer size is " + GetSize() + ".");
                    break;
            }
        }

        public Timer(int size, bool debug, bool floatingPoint)
        {
            SetSize(size);
            debugEnabled = debug;
            showFloatingPoint = floatingPoint;
            switch (debug)
            {
                case false:
                    Initialize(GetSize());
                    break;
                case true:
                    Initialize(GetSize());
                    UnityEngine.Debug.Log("Timer(s) Initiated.");
                    UnityEngine.Debug.Log("Timer size is " + GetSize() + ".");
                    break;
            }
        }



        private void RunTimers()
        {
            #region Run Timers
            ////Current time, when initialized, is set to 0. At the start of the game, assign the value of current time to reset time.
            for (int i = 0; i < GetSize(); i++)
            {
                if (timeStarted[i] == true)
                {
                    currentTime[i] += UnityEngine.Time.deltaTime;
                    if (debugEnabled)
                    {
                        if (showFloatingPoint == false)
                            UnityEngine.Debug.Log("Timer[" + i + "]; Current time: " + UnityEngine.Mathf.FloorToInt(currentTime[i]));
                        else
                            UnityEngine.Debug.Log("Timer[" + i + "]; Current time: " + currentTime[i]);
                    }
                }
                else
                    currentTime[i] = 0f;
            }
            #endregion
        }

        ///<summary>
        ///Activate a timer by index
        ///</summary>
        public void StartTimer(int index)
        {
            if (debugEnabled) UnityEngine.Debug.Log("Timer[" + index + "]; Start!");
            //Starts the timer
            timeStarted[index] = true;
            RunTimers();
        }

        /// <summary>
        /// Set a timer to zero
        /// </summary>
        public void SetToZero(int index, bool stop = false)
        {
            currentTime[index] = 0f;
            if (stop == true)
            {
                timeStarted[index] = false;
                if (debugEnabled) UnityEngine.Debug.Log("Timer [" + index + "]; stopped");
                
            }
        }

        /// <summary>
        /// Set a timer for this amount of seconds.
        /// </summary>
        public bool SetFor(float duration, int index)
        {
            if (currentTime[index] > duration)
            {
                if (debugEnabled) UnityEngine.Debug.Log("Timer[" + index + "] returned a value of 1!");
                SetToZero(index);
                return true;
            }
            return false;
        }

        public bool SetFor(float duration, int index, bool stop)
        {
            if (currentTime[index] > duration)
            {
                if (debugEnabled) UnityEngine.Debug.Log("Timer[" + index + "] returned a value of 1!");
                SetToZero(index, stop);
                return true;
            }

            return false;

        }

        /// <summary>
        /// Initialize all Pre-Defined Timers
        /// </summary>
        Timer Initialize(int _size = 12)
        {
            if (timer == null)
            {
                timer = this;
            }

            #region Initiate Timers
            currentTime = new float[_size];
            resetTime = new float[_size];
            timeStarted = new bool[_size];

            //Current time, when initialized, is set to 0. At the start of the game, assign the value of current time to reset time.
            for (int i = 0; i < resetTime.Length; i++)
            {
                resetTime[i] = currentTime[i];
            }
            #endregion

            return timer;
        }

        void SetSize(int _size)
        {
            size = _size;
        }

        public int GetSize()
        {
            return size;
        }
    }
}