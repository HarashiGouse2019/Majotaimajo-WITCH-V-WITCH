using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Record
{
    public List<ScoreEntry> scoreEntries;
    public Record(List<ScoreEntry> entries)
    {
        scoreEntries = entries;
    }

}

public class GameRecords : MonoBehaviour
{
    private static GameRecords Instance;
    //Shows the record of different scores from the top 10 players
    //It has a stat of View and Record
    //Depending on the state, certain UI will appear
    public enum State
    {
        VIEW,
        RECORD
    }

    static State CurrentMode = State.VIEW;

    //Input Obj

    //All Entries
    [SerializeField]
    public List<ScoreEntry> PlayerEntries = new List<ScoreEntry>();

    //Positioning
    public static int Positioning { get; private set; } = 0;

    //New Entry Event
    EventManager.Event entryEvent;

    //Ready one entry has been added
    bool ready = false;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        GetAllScoreEntries();

        //New Event
        entryEvent = EventManager.AddNewEvent(50, "NewEntry", null);
        entryEvent.AddNewListener(() => SwitchTo(State.RECORD));
        entryEvent.Trigger();
        ScoreSystem.SetHighScore(10291827);
        DetermineRanking(ScoreSystem.HighScore);
    }

    // Update is called once per frame
    void Update()
    {
        if (ready == true && Input.GetKeyDown(KeyCode.Return))
            GameSceneManager.Instance.LoadScene("TITLE");
    }

    /// <summary>
    /// Switch Game Record Modes
    /// </summary>
    /// <param name="state"></param>
    public static void SwitchTo(State state)
    {
        CurrentMode = state;
    }

    public static void AddNewEntry(ScoreEntry entry)
    {
        Instance.PlayerEntries.Add(entry);
    }

    public static void WriteAtRank(int position)
    {
        Instance.PlayerEntries[position].UpdateEntry(position + 1, EntryInput.GetSubmittedName(), ScoreSystem.HighScore, DateTime.Now, 0, 0f);
        Instance.ready = true;
    }

    public static void GetAllScoreEntries()
    {
        ScoreEntry[] entries = Instance.GetComponentsInChildren<ScoreEntry>();
        Instance.PlayerEntries = entries.ToList();
    }

    public static void DetermineRanking(int score)
    {
        int index = 0;
        //Compares the score of all other users
        foreach (ScoreEntry entry in Instance.PlayerEntries)
        {
            if (score >= entry.PlayerScore)
            {
                Positioning = index;
                Debug.Log("Rank " + (Positioning + 1));
                break;
            }
            else
            {
                entry.highlighting.gameObject.SetActive(false);
            }
        }
    }
}
