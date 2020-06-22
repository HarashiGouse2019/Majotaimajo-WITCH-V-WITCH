using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntryInput : MonoBehaviour
{
    //Responsible for the typing of new user input,
    //as well as modding one part of the many player entries

    //Reference to GameRecords
    [SerializeField]
    GameRecords gameRecords;

    //Then we need our TextMeshPro
    [SerializeField]
    TextMeshProUGUI inputEntry;

    //Name entry
    string nameEntry = "AAAAAAAAAA";

    //And format
    string nameFormat = "[{0}]";

    //And position of name
    int stringIndex = 0;

    const int RESET = 0;

    bool submitted = false;
    static string submittedName;

    // Start is called before the first frame update
    void Start()
    {
        inputEntry = GetComponent<TextMeshProUGUI>();
        gameRecords = FindObjectOfType<GameRecords>();
        StartCoroutine(NameEntryCycle());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Record()
    {

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey) && (char.IsLetterOrDigit((char)vKey) || char.IsWhiteSpace((char)vKey)))
            {
                //First Check if it was enter
                if (vKey == KeyCode.Return)
                {
                    submitted = true;
                    submittedName = nameEntry;
                    GameRecords.WriteAtRank(GameRecords.Positioning);
                    return;
                }

                //Get our letter...
                char letter = (char)vKey;

                string updatedString = "";

                int index = 0;

                foreach(char character in nameEntry)
                {
                    if (index == stringIndex) 
                        updatedString += letter;
                    else
                        updatedString += character;

                    index++;
                }

                nameEntry = updatedString;

                inputEntry.text = string.Format(nameFormat, updatedString);

                stringIndex++;

                if (stringIndex > 9)
                    stringIndex = RESET;

                break;
            } 
        }
    }

    IEnumerator NameEntryCycle()
    {
        while (!submitted)
        {
            Record();
            yield return null;
        }
    }

    public static string GetSubmittedName() => submittedName;
}
