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
        }
    }

    public override void SetupEvents()
    {
        _onSelectPrevious = EventManager.AddNewEvent(090, "onDifficultySelectPrevious", difficultyMenu.OnClickHorizontalPrev);

        _onSelectNext = EventManager.AddNewEvent(091, "onDifficultySelectNext", difficultyMenu.OnClickHorizontalNext);

        _onConfirm = EventManager.AddNewEvent(092, "onDifficultyConfirm", OnConfirm);
    }
}
