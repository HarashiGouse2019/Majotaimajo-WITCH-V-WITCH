using System;
using System.Collections;
using System.Collections.Generic;
using Alarm;
public class ScoreSystem : UnityEngine.MonoBehaviour
{
    public static ScoreSystem Instance;

    //Score System
    int timesHit;
    readonly int hitpoint = 30;
    readonly int stallpoint = 1;

    public static int Score { get; private set; } = 0;
    public static int HighScore { get; private set; } = 0;

    Timer timer = new Timer(1);

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Update()
    {
        Score += stallpoint;
    }

    public void Amplify()
    {
        Score += (hitpoint * timesHit) + stallpoint;
        timer.StartTimer(0);
        timer.currentTime[0] = 0;
    }

    public void StreakDuration(int _value)
    {
        if (timer.SetFor(_value, 0))
        {
            BreakStreak();
        }
    }

    public void BreakStreak()
    {
        timesHit = 0;
        timer.SetToZero(0);
    }

    public int GetScore()
    {
        return Score;
    }

    public static void SetHighScore(int score)
    {
        HighScore = score;
    }

    public static void ClearScore()
    {
        Score = 0;
    }
    public static void ClearHighScore()
    {
        HighScore = 0;
    }
}
