using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntry : MonoBehaviour
{
    public static ScoreEntry Instance;

    //EntryID
    public int EntryID { get; private set; }

    //The Name of the player
    public string PlayerName { get; private set; }

    //The registered score
    public int PlayerScore { get; private set; }

    //The date and time
    public DateTime DataAchieved { get; private set; }

    //The stage you last were on
    public int StageNumber { get; private set; }

    //Game Percentage of finishing
    public float GameCompletionPercentage { get; private set; }

    //Now we do different formatting
    string playerEntryFormat = "{0}    {1}    {2}    {3}    Stage    {4}    {5}%";

    //Highlighting
    [SerializeField]
    public Image highlighting;

    //The text mesh pro
    public TextMeshProUGUI entryText;

    private void Awake()
    {
        Instance = this;
        GetHighlighting();
        GetTextMeshProUGUI();
    }

    public void UpdateEntry(int entryID, string playerName, int playerScore, DateTime dataAchieved, int stageNumber, float gameCompletionPercentage)
    {
        EntryID = entryID;
        PlayerName = playerName;
        PlayerScore = playerScore;
        DataAchieved = dataAchieved;
        StageNumber = stageNumber;
        GameCompletionPercentage = gameCompletionPercentage;

        string newEntry = string.Format(playerEntryFormat, entryID, playerName, playerScore.ToString("D10", CultureInfo.InvariantCulture), dataAchieved.Date, stageNumber, gameCompletionPercentage);

        entryText.text = newEntry;
    }

    void GetHighlighting()
    {
        highlighting = GetComponentInChildren<Image>();
    }

    void GetTextMeshProUGUI()
    {
        entryText = GetComponent<TextMeshProUGUI>();
    }
}
