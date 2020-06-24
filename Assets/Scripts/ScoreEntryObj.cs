using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntryObj : MonoBehaviour
{
    public static ScoreEntryObj Instance;

    public Entry entry = new Entry();

    //Highlighting
    [SerializeField]
    public Image highlighting;

    //The text mesh pro
    public TextMeshProUGUI entryText;

    //Top Entry
    public ScoreEntryObj topEntry;

    //Bottom Entry
    public ScoreEntryObj bottomEntry;

    private void Awake()
    {
        Instance = this;
        GetHighlighting();
        GetTextMeshProUGUI();
    }

    public void UpdateEntry(int entryID, string playerName, int playerScore, DateTime dateAchieved, int stageNumber, float gameCompletionPercentage)
    {
        entry.SetEntryID(entryID);
        entry.SetPlayerName(playerName);
        entry.SetPlayerScore(playerScore);
        entry.SetDateAchieved(dateAchieved);
        entry.SetStageNumber(stageNumber);
        entry.SetGameCompletionPercentage(gameCompletionPercentage);

        string newEntry = string.Format(entry.GetEntryFormat(), entryID, playerName, playerScore.ToString("D10", CultureInfo.InvariantCulture), dateAchieved, stageNumber, gameCompletionPercentage);

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

    public Entry GetEntry() => entry;
}
