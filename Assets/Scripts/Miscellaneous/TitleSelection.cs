using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TitleSelection : MonoBehaviour, IFocusable
{
    [SerializeField]
    MusicTheme FLOMOST;

    [SerializeField]
    Color selectedColor, unselectedColor, unavaliableColor;

    [SerializeField]
    int selectedFontSize, unselectedFontSize;

    /*TitleSelection will take all objects with the Selectable tag and is on the TitleLayer*/
    [SerializeField]
    List<TextMeshProUGUI> selectableObjects, unavailableObjects;

    //The images for each selectableObject
    List<Image> images;

    //SelectionIndex
    int SelectionIndex = 0;

    //KeyDown
    bool keyDown = true;
    int lagValue = 0;

    //Constants
    const string SELECTABLE_TAG = "Selectable";
    const string TITLE_LAYER = "Title";
    const string TAB = " ";
    const string STRING_NULL = "";
    const int NEXT_SELECTION = 1;
    const int PREVIOUS_SELECTION = -1;

    //Controlls
    float horizontalDir, verticalDir;

    EventManager.Event @StartSelected;
    EventManager.Event @PracticeSelected;
    EventManager.Event @ExitSelected;
    private int sign;

    //Determines if you control these selections or not
    bool _onFocus;

    // Start is called before the first frame update
    void Awake()
    {
        _onFocus = true;
        SetupEvents();
    }

    void Start()
    {
        FLOMOST.PlayTheme();
        GetSelections();
    }

    void GetSelections()
    {
        selectableObjects = new List<TextMeshProUGUI>();
        images = new List<Image>();

        //Get all gameobject in hierarchy
        TextMeshProUGUI[] objectsInHierarchy = GetComponentsInChildren<TextMeshProUGUI>();

        //Check the tag and layer
        foreach (TextMeshProUGUI obj in objectsInHierarchy)
        {
            if (obj.gameObject.tag == SELECTABLE_TAG && obj.gameObject.layer == LayerMask.NameToLayer(TITLE_LAYER))
            {
                SelectionEvent selectionEvent = obj.GetComponent<SelectionEvent>();
                if (selectionEvent.HasEvent())
                {
                    selectableObjects.Add(obj);
                    images.Add(obj.GetComponentInChildren<Image>());
                }
                else
                {
                    unavailableObjects.Add(obj);
                    obj.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("NullImage");
                    obj.GetComponentInChildren<Image>().gameObject.SetActive(false);
                }
            }
        }

        if (objectsInHierarchy == null)
        {
            Debug.LogError("Objects not found...");
            return;
        }

        //Update Text UI
        UpdateTextUI();

        //Begin Selection Cycle
        StartCoroutine(SelectionCycle());
    }

    /// <summary>
    /// Read for user input
    /// </summary>
    void ReadInput()
    {
        if (lagValue == 1)
        {
            lagValue = 0;
            return;
        }

        if (!keyDown)
        {
            horizontalDir = Input.GetAxisRaw("Horizontal");
            verticalDir = Input.GetAxisRaw("Vertical");

            //Check if input is being received
            if (horizontalDir == NEXT_SELECTION || verticalDir == PREVIOUS_SELECTION)
            {

                SelectionIndex += NEXT_SELECTION;
                sign = NEXT_SELECTION;

                //Check if bigger than size
                if (SelectionIndex > selectableObjects.Count - 1)
                    SelectionIndex = 0;


                //Update Text UI
                UpdateTextUI();
                AudioManager.Play("CursorMovement");

                //Button is down
                keyDown = true;
            }

            if (horizontalDir == PREVIOUS_SELECTION || verticalDir == NEXT_SELECTION)
            {
                SelectionIndex += PREVIOUS_SELECTION;
                sign = PREVIOUS_SELECTION;

                //Check if smaller than 0
                if (SelectionIndex < 0)
                    SelectionIndex = selectableObjects.Count - 1;

                //Update Text UI
                UpdateTextUI();
                AudioManager.Play("CursorMovement");

                //Button is down
                keyDown = true;
            }

        }

        //If horizontal or vertical not being pressed, keyDown is false
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            keyDown = false;
        }

        //If enter was pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            selectableObjects[SelectionIndex].GetComponent<SelectionEvent>().GetUnityEvent().Invoke();
        }

    }

    /// <summary>
    /// At the current selection index, highlight the text
    /// </summary>
    void UpdateTextUI()
    {
        for (int index = 0; index < selectableObjects.Count; index++)
        {
            TextMeshProUGUI selectableText = selectableObjects[index];
            SelectionEvent selectionEvent = selectableText.GetComponent<SelectionEvent>();
            if (index == SelectionIndex && selectionEvent.HasEvent())
            {
                //This is the current object that is selected, so highlight it
                selectableText.color = selectedColor;
                selectableText.text = TAB + selectableObjects[index].text;
                selectableText.fontSize = selectedFontSize;
                images[index].gameObject.SetActive(true);
            }
            else
            {
                selectableText.color = unselectedColor;

                selectableText.text = selectableObjects[index].text.Replace(TAB, STRING_NULL);
                selectableText.fontSize = unselectedFontSize;
                images[index].gameObject.SetActive(false);
            }
        }


        for (int index = 0 ; index < unavailableObjects.Count; index++)
        {
            TextMeshProUGUI unavaliableText = unavailableObjects[index];
            unavaliableText.fontSize = unselectedFontSize;
            unavaliableText.color = unavaliableColor;
        }
    }

    public void StopTitleBGM()
    {
        MusicManager.Stop("WVWOST");
    }

    IEnumerator SelectionCycle()
    {
        while (true)
        {
            if(_onFocus) ReadInput();
            yield return null;
        }
    }

    public void SetupEvents()
    {
        //Set up Start Selected Event
        StartSelected = EventManager.AddEvent(100, "StartSelected",
            () => GameSceneManager.LoadScene("DIFFICULTY_SELECTION"),
            () => ClearEvents());

        //Set up Practice Selected Event
        PracticeSelected = EventManager.AddEvent(101, "PracticeSelected",
            () => GameSceneManager.LoadScene("DIFFICULTY_SELECTION"),
            () => GameManager.IsPractice = true,
            () => ClearEvents());

        //Set up Exit Selected Event
        ExitSelected = EventManager.AddEvent(102, "ExitSelected",
            () => _onFocus = false,
            () => new Prompt("Are you sure you want to quit the game?", null,
                        new PromptChoices("I'm sure", () => { Application.Quit(); ClearEvents(); ReturnOnFocus(); }),
                        new PromptChoices("I changed my mind", () => { ClearEvents(); ReturnOnFocus(); }))) ;
    }

    private void ReturnOnFocus()
    {
        _onFocus = true;
        lagValue = 1;
    }

    void ClearEvents()
    {
        //Remove StartSelected Event
        EventManager.RemoveEvent(StartSelected);
        EventManager.RemoveEvent(PracticeSelected);
        EventManager.RemoveEvent(ExitSelected);
    }

    public void OnStart()
    {
        StartSelected.Trigger();
    }

    public void OnPractice()
    {
        PracticeSelected.Trigger();
    }

    public void OnExit()
    {
        ExitSelected.Trigger();
    }
}
