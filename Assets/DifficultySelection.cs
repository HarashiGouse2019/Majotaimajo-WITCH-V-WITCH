using System.Collections;
using UnityEngine;
using static Keymapper;

public class DifficultySelection : MonoBehaviour, IEventSetup
{
    private int _difficultyIndex = 0;
    private bool readyToSelect = false;
    EventManager.Event _onSelectionNext, _onSelectionPrevious, _onConfirm;

    [SerializeField]
    SimpleScroll difficultyMenu;

    void Start()
    {
        SetupEvents();
    }

    IEnumerator SelectionCycle()
    {
        while (true)
        {
            ControlAction("start", false, _onConfirm);
            ControlAction("left", false, _onSelectionPrevious);
            ControlAction("right", false, _onSelectionNext);

            yield return null;
        }
    }

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
            Debug.Log($"You have selected {_difficultyIndex}");
            GameManager.UpdateGameDifficulty(_difficultyIndex);
        }
    }

    public void SetupEvents()
    {
        _onSelectionPrevious = EventManager.AddNewEvent(90, "selectPrevious", difficultyMenu.OnClickHorizontalPrev);

        _onSelectionNext = EventManager.AddNewEvent(91, "selectNext", difficultyMenu.OnClickHorizontalNext);

        _onConfirm = EventManager.AddNewEvent(92, "onDifficultyConfirm", OnConfirm);

        StartCoroutine(SelectionCycle());
    }
}
