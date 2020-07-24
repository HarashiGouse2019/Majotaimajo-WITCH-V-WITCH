using System;

[Serializable]
public class Entry
{
    //EntryID
    public int EntryID = 1;

    //The Name of the player
    public string PlayerName = "----------";

    //The registered score
    public int PlayerScore = 0;

    //The date and time
    public DateTime DateAchieved = DateTime.Now;

    //The stage you last were on
    public int StageNumber = 1;

    //Game Percentage of finishing
    public float GameCompletionPercentage = 0.0f;

    //Now we do different formatting
    string playerEntryFormat = "{0}    {1}    {2}    {3}    Stage    {4}    {5}%";

    public void SetEntryID(int value) => EntryID = value;

    public void SetPlayerName(string name) => PlayerName = name;

    public void SetPlayerScore(int value) => PlayerScore = value;

    public void SetDateAchieved(DateTime date) => DateAchieved = date;
    public void SetStageNumber(int value) => StageNumber = value;
    public void SetGameCompletionPercentage(float value) => GameCompletionPercentage = value;
    public string GetEntryFormat() => playerEntryFormat;
}