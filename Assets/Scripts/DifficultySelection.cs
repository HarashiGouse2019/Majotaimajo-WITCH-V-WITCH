using UnityEngine;

public class DifficultySelection : SelectionObject
{
    private int _difficultyIndex = 0;
    private bool readyToSelect = false;

    [SerializeField]
    SimpleScroll difficultyMenu;

    public void UpdateDifficultyIndex(int index)
    {
        _difficultyIndex = index;
    }

    public void ToggleReadyToSelect(int sign)
    {
        readyToSelect = (sign == 1);
    }

    void OnConfirm()
    {
        //Add stuff here
        if (readyToSelect)
        {
            _difficultyIndex = difficultyMenu.index;
            Debug.Log($"You have selected {_difficultyIndex}");
            GameManager.UpdateGameDifficulty(_difficultyIndex);
            GameSceneManager.UnloadScene("DIFFICULTY_SELECTION");
            GameSceneManager.LoadScene("CHARACTER_SELECTION", true);
        }
    }

    void OnCancel()
    {
        if (readyToSelect)
        {
            Debug.Log("Cancelled Difficulty");
            GameSceneManager.UnloadScene("DIFFICULTY_SELECTION");
            GameSceneManager.LoadScene("TITLE", true);
        }
    }

    public override void SetupEvents()
    {
        _onSelectPrevious = EventManager.AddEvent(090, "onDifficultySelectPrevious", difficultyMenu.OnClickHorizontalPrev);
        _onSelectNext = EventManager.AddEvent(091, "onDifficultySelectNext", difficultyMenu.OnClickHorizontalNext);
        _onConfirm = EventManager.AddEvent(092, "onDifficultyConfirm", OnConfirm);
        _onCancel = EventManager.AddEvent(093, "onDifficultyCancel", OnCancel);
    }
}
