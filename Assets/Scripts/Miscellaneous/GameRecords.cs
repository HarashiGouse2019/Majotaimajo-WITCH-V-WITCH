﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Record
{
    public List<Entry> scoreEntries;
    public Record(List<Entry> entries)
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
    public static List<Entry> Entries;
    public List<Entry> entries;

    public static List<ScoreEntryObj> EntryObjects;
    public List<ScoreEntryObj> entryObjects;

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
        Entries = new List<Entry>();
        entries = Entries;

        EntryObjects = new List<ScoreEntryObj>();
        entryObjects = EntryObjects;

        //I get all highscore entries (just the gameObjects)
        GetAllScoreEntries(false);

        //New Event
        entryEvent = EventManager.AddEvent(50, "NewEntry",
            () => SwitchTo(State.RECORD));

        entryEvent.Trigger();
        DetermineRanking(ScoreSystem.HighScore);
    }

    // Update is called once per frame
    void Update()
    {
        if (ready == true && Input.GetKeyDown(KeyCode.Return))
            GameSceneManager.LoadScene("TITLE", false);
    }

    /// <summary>
    /// Switch Game Record Modes
    /// </summary>
    /// <param name="state"></param>
    public static void SwitchTo(State state)
    {
        CurrentMode = state;
    }

    public static void AddNewEntry(ScoreEntryObj entry)
    {
        EntryObjects.Add(entry);
    }

    public static void WriteAtRank(int position)
    {
        EntryObjects[position].UpdateEntry(position + 1, EntryInput.GetSubmittedName(), ScoreSystem.HighScore, DateTime.Now, 1, 0f);

        Record newRecord = new Record(Entries);
        SaveRecord(newRecord);

        //Now we clear highscore in the system
        ScoreSystem.ClearScore();
        ScoreSystem.ClearHighScore();
        Instance.ready = true;
    }

    public static void GetAllScoreEntries(bool considerPositioning)
    {
        ScoreEntryObj[] objs = Instance.GetComponentsInChildren<ScoreEntryObj>();

        //Load Json File
        try
        {
            Record savedRecord = LoadRecord();
            int index = 0;
            foreach (ScoreEntryObj obj in objs)
            {
                //Get entry information saved from json
                Entry entry = savedRecord.scoreEntries[index];

                if (considerPositioning && index == Positioning)
                    continue;

                //Update UI Text 
                obj.UpdateEntry (
                    entry.EntryID,
                    entry.PlayerName,
                    entry.PlayerScore,
                    entry.DateAchieved,
                    entry.StageNumber,
                    entry.GameCompletionPercentage
                );

                Entries.Add(obj.entry);

                index++;
            }
        }
        catch
        {
            EntryObjects = objs.ToList();
        }
        EntryObjects = objs.ToList();
    }

    public static void DetermineRanking(int score)
    {
        int index = 0;

        //Compares the score of all other users
        foreach (ScoreEntryObj obj in EntryObjects)
        {
            if (score > obj.GetEntry().PlayerScore)
            {
                Positioning = index;
                Debug.Log("Rank " + (Positioning + 1));
                UpdateHighlighting();
                Entries.Add(EntryObjects[index].GetEntry());
                GetAllScoreEntries(true);
                break;
            }

            index++;
        }
    }

    public static void UpdateHighlighting()
    {
        for (int index = 0; index < EntryObjects.Count; index++)
        {
            if (index == Positioning)
                EntryObjects[index].highlighting.gameObject.SetActive(true);
            else
                EntryObjects[index].highlighting.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Will shift a rank up or down depending on the current ranking
    /// </summary>
    public static void ShiftList()
    {
        //I'll have to take the current highscore's ranking position,
        //And moving the other entries into 
        for (int index = 0; index < EntryObjects.Count; index++)
        {

        }
    }

    public static void SaveRecord(Record record)
    {
        string recordJson = JsonUtility.ToJson(record);

        if (File.Exists(Application.persistentDataPath + @"/TOP.json"))
            File.WriteAllText(Application.persistentDataPath + @"/TOP.json", recordJson);
        else
        {
            File.CreateText(Application.persistentDataPath + @"/TOP.json");
            File.WriteAllText(Application.persistentDataPath + @"/TOP.json", recordJson);
        }
    }

    public static Record LoadRecord() => JsonUtility.FromJson<Record>(File.ReadAllText(Application.persistentDataPath + @"/TOP.json"));
}