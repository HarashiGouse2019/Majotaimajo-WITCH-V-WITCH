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

    static Timer ScoreTimer = new Timer(1);

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
        ScoreTimer.StartTimer(0);
        ScoreTimer.currentTime[0] = 0;
    }

    public void StreakDuration(int _value)
    {
        if (ScoreTimer.SetFor(_value, 0))
        {
            BreakStreak();
        }
    }

    public void BreakStreak()
    {
        timesHit = 0;
        ScoreTimer.SetToZero(0);
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
        ScoreTimer.SetToZero(0, true);
        Score = 0;
    }
    public static void ClearHighScore()
    {
        ScoreTimer.SetToZero(0, true);
        HighScore = 0;
    }
}
