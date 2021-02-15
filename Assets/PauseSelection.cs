using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseSelection : SelectionObject
{
    [SerializeField]
    private List<TextMeshProUGUI> options;
    private List<Image> optionGraphics;
    private List<SelectionEvent> optionActions;

    [SerializeField]
    private Color selectedColor, unselectedColor;

    [SerializeField]
    int selectedFontSize, unselectedFontSize;

    SelectionEvent currentOptionAction = null;

    const string SELECTABLE_TAG = "Selectable";
    const string PAUSE_LAYER = "Title";

    // Start is called before the first frame update
    void Start()
    {
        GetOptions();
    }

    void GetOptions()
    {
        options = new List<TextMeshProUGUI>();
        optionGraphics = new List<Image>();

        //Get all gameobject in hierarchy
        TextMeshProUGUI[] objectsInHierarchy = GetComponentsInChildren<TextMeshProUGUI>();

        //Check the tag and layer
        foreach (TextMeshProUGUI obj in objectsInHierarchy)
        {
            if (obj.gameObject.tag == SELECTABLE_TAG && obj.gameObject.layer == LayerMask.NameToLayer(PAUSE_LAYER))
            {
                SelectionEvent selectionEvent = obj.GetComponent<SelectionEvent>();
                if (selectionEvent.HasEvent())
                {
                    options.Add(obj);
                    optionGraphics.Add(obj.GetComponentInChildren<Image>());
                    optionActions.Add(obj.GetComponentInChildren<SelectionEvent>() ?? null);
                }
            }
        }

        if (objectsInHierarchy == null)
        {
            Debug.LogError("Objects not found...");
            return;
        }

        //Update Text UI
        UpdateUI();
    }

    void OnConfirm()
    {
        
    }

    void OnCancel()
    {

    }

    void NextOption()
    {
        selectionIndex++;
        UpdateUI();
    }

    void PreviousOption()
    {
        selectionIndex--;
        UpdateUI();
    }

    /// <summary>
    /// At the current selection index, highlight the text
    /// </summary>
    void UpdateUI()
    {
        selectionIndex = (selectionIndex > options.Count - 1) ?
        selectionIndex - options.Count : (selectionIndex < 0) ?
        selectionIndex + options.Count : selectionIndex;

        for (int index = 0; index < options.Count; index++)
        {
            TextMeshProUGUI selectableText = options[index];
            currentOptionAction = optionActions[index];
            if (index == selectionIndex && currentOptionAction.HasEvent())
            {
                //This is the current object that is selected, so highlight it
                selectableText.color = selectedColor;
                selectableText.fontSize = selectedFontSize;
                optionGraphics[index].gameObject.SetActive(true);
            }
            else
            {
                selectableText.color = unselectedColor;
                selectableText.fontSize = unselectedFontSize;
                optionGraphics[index].gameObject.SetActive(false);
            }
        }
    }

    public override void SetupEvents()
    {
        _onSelectPrevious = EventManager.AddEvent(390, "onCharacterSelectPrevious", PreviousOption, () => cursorSound.Play());
        _onSelectNext = EventManager.AddEvent(391, "onCharacterSelectNext", NextOption, () => cursorSound.Play());
        _onConfirm = EventManager.AddEvent(392, "onCharacterConfirm", OnConfirm, () => confirmSound.Play());
        _onCancel = EventManager.AddEvent(393, "onCharacterCancel", OnCancel, () => cancelSound.Play());
    }
}
