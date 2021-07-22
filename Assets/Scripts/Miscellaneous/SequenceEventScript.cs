using System;
using System.Collections;
using UnityEngine;
using System.Linq.Expressions;
using Coroutine = Extensions.Coroutine;
using BulletPro;


namespace SequenceEventUtility
{
    public enum NativeEnemyType
    {
        NULL,
        NORMAL,
        MINI_BOSS,
        BOSS
    }

    public enum GridOrientation
    {
        LEFT,
        RIGHT
    }

    public enum StateInstructions
    {
        NONE = -1,
        PLAY_STAGE_ANIMATION,
        PLAY_DISPLAY_ANIMATION,
        WAIT_UNTIL,
        PLAY_DIALOGUE,
        CREATE_ENEMY_INSTANCE,
        INITIATE_BOSS,
        INITIATE_MINI_BOSS,
        CALL_EVENT,
        PLAY_MUSIC,
        PAUSE_MUSIC,
        PLAY_BOSS_MUSIC,
        PAUSE_BOSS_MUSIC,
        GOTO_STAGE,
        END
    }

    [Serializable]
    public abstract class SequenceEventScript : MonoBehaviour
    {
        EventManager.Event[] @preBuilt_Events;

        //Start with array initialization (has to be set up like this with "__EVENTS__", or it won't work
        protected StageEvent[] __EVENTS__;

        public static bool IsWaiting { get; private set; } = false;

        private float _currentInterval = -1f;
        private float _intervalDelta = 0.25f;

        private object[] _setParams;

        private bool _atEnd = false;

        void Init()
        {
            //Initiate Pre-Built Stage Events
            preBuilt_Events = new EventManager.Event[14]
            {
                EventManager.AddEvent(700, "PLAY_STAGE_ANIMATION", PlayStageAnimation),
                EventManager.AddEvent(701, "PLAY_DISPLAY_ANIMATION", PlayDisplayNameAnimation),
                EventManager.AddEvent(702, "WAIT_UNTIL", WaitUntil),
                EventManager.AddEvent(703, "PLAY_DIALOGUE", PlayDialogue),
                EventManager.AddEvent(704, "CREATE_ENEMY_INSTANCE", CreateEnemyInstance),
                EventManager.AddEvent(705, "INITIATE_BOSS", InitaiateBoss),
                EventManager.AddEvent(706, "INITIATE_MINI_BOSS", InitiateMiniBoss),
                EventManager.AddEvent(707, "CALL_EVENT", CallEvent),
                EventManager.AddEvent(708, "PLAY_MUSIC", PlayMusic),
                EventManager.AddEvent(709, "PAUSE_MUSIC", PauseMusic),
                EventManager.AddEvent(710, "PLAY_BOSS_MUSIC", PlayBossMusic),
                EventManager.AddEvent(711, "PAUSE_BOSS_MUSIC", PauseBossMusic),
                EventManager.AddEvent(712, "GOTO_STAGE", GotoStage),
                EventManager.AddEvent(713, "END", End)
            };

            Coroutine.Start(MainCycle());
        }

        private void NextInterval() => _currentInterval++;

        IEnumerator MainCycle()
        {
            while (!_atEnd)
            {
                try
                {
                    NextInterval();
                    Scan();
                    _atEnd = CheckEnd();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                yield return new WaitForSeconds(_intervalDelta);

                
            }
            Debug.Log("Sequence has eneded.");
        }

        private bool CheckEnd()
        {
            int lastIndex = __EVENTS__.Length - 1;
            StageEvent lastEvent = __EVENTS__[lastIndex];
            return _currentInterval > lastEvent.Interval;
        }

        void Scan()
        {
            for (int i = 0; i < __EVENTS__.Length; i++)
            {
                if (__EVENTS__[i].Interval == _currentInterval)
                {
                    int selectedEventCode = (int)__EVENTS__[i].m_event;
                    _setParams = __EVENTS__[i].parameters;
                    if (selectedEventCode > -1)
                    {
                        preBuilt_Events[selectedEventCode].Trigger();
                    } else
                    {
                        Debug.Log("Nothing Happened");
                    }

                    
                    return;
                }
            };
        }

        //All events that this system can call
        #region Event Callbacks
        void PlayStageAnimation()
        {

        }
        void PlayDisplayNameAnimation()
        {

        }
        void WaitUntil()
        {
            Func<bool> condition;
        }
        void PlayDialogue()
        {

        }

        void CreateEnemyInstance()
        {
            uint enemyID;
            GridOrientation orientation;
            int xCoord, yCoord;
            AnimationClip animation;
            BulletEmitter[] emitters;
        }
        void InitaiateBoss()
        {
            int bossHealth = (int)_setParams[0];
            int bossPatience = (int)_setParams[1];
            Debug.Log($"Initiating Mini Boss: HP {bossHealth}; Patience: {bossPatience}");
        }

        void InitiateMiniBoss()
        {
            int miniBossHealth = (int)_setParams[0];
            int miniBossPatience = (int)_setParams[1];
            Debug.Log($"Initiating Mini Boss: HP {miniBossHealth}; Patience: {miniBossPatience}");
        }

        void CallEvent()
        {

            string eventCode = _setParams[0] as string;
            Debug.Log($"Triggering Event: {eventCode}");
            EventManager.TriggerEvent(eventCode);
        }
        void PlayMusic()
        {
            string musicName = _setParams[0] as string;
            Debug.Log("Value Passed: " + musicName);
        }
        void PauseMusic()
        {

        }
        void PlayBossMusic()
        {

        }
        void PauseBossMusic()
        {

        }
        void GotoStage()
        {

        }
        void End()
        {
            Debug.Log("End");
        }
        #endregion


        public void Ready()
        {
            Init();
        }
    }
}