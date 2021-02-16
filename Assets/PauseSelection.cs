using System.Collections;
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

    [SerializeField]
    SelectionEvent currentOptionAction = null;

    const string SELECTABLE_TAG = "Selectable";

    protected override void Start()
    {
        GetOptions();
    }

    void GetOptions()
    {
        options = new List<TextMeshProUGUI>();
        optionGraphics = new List<Image>();
        optionActions = new List<SelectionEvent>();

        //Get all gameobject in hierarchy
        TextMeshProUGUI[] objectsInHierarchy = GetComponentsInChildren<TextMeshProUGUI>();

        if (objectsInHierarchy == null)
        {
            Debug.LogError("Objects not found...");
            return;
        }

        //Check the tag and layer
        foreach (TextMeshProUGUI obj in objectsInHierarchy)
        {

            if (obj.gameObject.CompareTag(SELECTABLE_TAG))
            {
                SelectionEvent selectionEvent = obj.GetComponent<SelectionEvent>();
                if (selectionEvent.HasEvent())
                {
                    options.Add(obj);
                    optionGraphics.Add(obj.GetComponentInChildren<Image>());
                    optionActions.Add(obj.GetComponent<SelectionEvent>());
                }
            }
        }

        
        //Update Text UI
        UpdateUI();
    }

    void OnConfirm()
    {
        Debug.Log("ACTION CONFIRMED!!!!");
        currentOptionAction.GetUnityEvent().Invoke();
        confirmSound.Play();
    }

    void OnCancel()
    {
        cancelSound.Play();
    }

    void NextOption()
    {
        selectionIndex++;
        UpdateUI();
        cursorSound.Play();
    }

    void PreviousOption()
    {
        selectionIndex--;
        UpdateUI();
        cursorSound.Play();
    }

    /// <summary>
    /// At the current selection index, highlight the text
    /// </summary>
    void UpdateUI()
    {
        selectionIndex = (selectionIndex > options.Count - 1) ?
        selectionIndex - options.Count : (selectionIndex < 0) ?
        selectionIndex + options.Count : selectionIndex;

        currentOptionAction = optionActions[selectionIndex];

        for (int index = 0; index < options.Count; index++)
        {
            TextMeshProUGUI selectableText = options[index];
            
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
        _onSelectPrevious = EventManager.AddEvent(390, "onPauseOptionSelectPrevious", PreviousOption);
        _onSelectNext = EventManager.AddEvent(391, "onPauseOptionSelectNext", NextOption);
        _onConfirm = EventManager.AddEvent(392, "onPauseOptionConfirm", OnConfirm);
        _onCancel = EventManager.AddEvent(393, "onPauseOptionCancel", OnCancel);
    }
}
