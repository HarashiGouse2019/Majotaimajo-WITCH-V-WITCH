using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class TitleSelection : MonoBehaviour, IEventSetup
{
    [SerializeField]
    Color selectedColor, unselectedColor;

    [SerializeField]
    int selectedFontSize, unselectedFontSize;

    /*TitleSelection will take all objects with the Selectable tag and is on the TitleLayer*/
    List<TextMeshProUGUI> selectableObjects;

    //The images for each selectableObject
    List<Image> images;

    //SelectionIndex
    int SelectionIndex = 0;

    //KeyDown
    bool keyDown = false;

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

    // Start is called before the first frame update
    void Awake()
    {
        GetSelections();

        SetupEvents();

        //Play Title Music
        MusicManager.Play("WVWOST");
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
                selectableObjects.Add(obj);
                images.Add(obj.GetComponentInChildren<Image>());
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

        if (!keyDown)
        {
            horizontalDir = Input.GetAxisRaw("Horizontal");
            verticalDir = Input.GetAxisRaw("Vertical");

            //Check if input is being received
            if (horizontalDir == NEXT_SELECTION || verticalDir == PREVIOUS_SELECTION)
            {
                SelectionIndex += NEXT_SELECTION;

                //Check if bigger than size
                if (SelectionIndex > selectableObjects.Count - 1)
                    SelectionIndex = 0;

                //Update Text UI
                UpdateTextUI();
            }

            if (horizontalDir == PREVIOUS_SELECTION || verticalDir == NEXT_SELECTION)
            {
                SelectionIndex += PREVIOUS_SELECTION;

                //Check if smaller than 0
                if (SelectionIndex < 0)
                    SelectionIndex = selectableObjects.Count - 1;

                //Update Text UI
                UpdateTextUI();
            }

            //Button is down
            keyDown = true;
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
            if (index == SelectionIndex)
            {
                //This is the current object that is selected, so highlight it
                selectableObjects[index].color = selectedColor;
                selectableObjects[index].text = TAB + selectableObjects[index].text;
                selectableObjects[index].fontSize = selectedFontSize;
                images[index].gameObject.SetActive(true);
                AudioManager.Play("CursorMovement");
            }
            else
            {
                selectableObjects[index].color = unselectedColor;
                selectableObjects[index].text = selectableObjects[index].text.Replace(TAB, STRING_NULL);
                selectableObjects[index].fontSize = unselectedFontSize;
                images[index].gameObject.SetActive(false);
            }
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
            ReadInput();
            yield return null;
        }
    }

    public void SetupEvents()
    {
        //Set up Start Selected Event
        StartSelected = EventManager.AddNewEvent(100, "StartSelected",
            () => GameSceneManager.Instance.LoadScene("LUU_STAGE"),
            () => GameManager.StartGame(),
            () => StopTitleBGM(),
            () => ClearEvents());

        //Set up Practice Selected Event
        PracticeSelected = EventManager.AddNewEvent(101, "PracticeSelected", 
            () => GameSceneManager.Instance.LoadScene("LUU_STAGE"),
            () => GameManager.IsPractice = true,
            () => GameManager.StartGame(),
            () => GameManager.StartGame(),
            () => ClearEvents());

        //Set up Exit Selected Event
        ExitSelected = EventManager.AddNewEvent(102, "ExitSelected", 
            () => Application.Quit(), 
            () => ClearEvents());
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
