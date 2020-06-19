using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleSelection : MonoBehaviour
{
    /*TitleSelection will take all objects with the Selectable tag and is on the TitleLayer*/
    [SerializeField]
    List<TextMeshProUGUI> selectableObjects;

    [SerializeField]
    Color selectedColor, unselectedColor;

    [SerializeField]
    int selectedFontSize, unselectedFontSize;

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

    // Start is called before the first frame update
    void Awake()
    {
        GetSelections();
    }

    void GetSelections()
    {
        selectableObjects = new List<TextMeshProUGUI>();

        //Get all gameobject in hierarchy
        TextMeshProUGUI[] objectsInHierarchy = GetComponentsInChildren<TextMeshProUGUI>();

        //Check the tag and layer
        foreach (TextMeshProUGUI obj in objectsInHierarchy)
        {
            Debug.Log(obj.name);
            if (obj.gameObject.tag == SELECTABLE_TAG && obj.gameObject.layer == LayerMask.NameToLayer(TITLE_LAYER))
            {
                selectableObjects.Add(obj);
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
                UpdateTextUI();
            }

            if (horizontalDir == PREVIOUS_SELECTION || verticalDir == NEXT_SELECTION)
            {
                SelectionIndex += PREVIOUS_SELECTION;
                UpdateTextUI();
            }
            keyDown = true;
        }

        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            keyDown = false;
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
            }
            else
            {
                selectableObjects[index].color = unselectedColor;
                selectableObjects[index].text = selectableObjects[index].text.Replace(TAB, STRING_NULL);
                selectableObjects[index].fontSize = unselectedFontSize;
            }
        }
    }

    IEnumerator SelectionCycle()
    {
        while (true)
        {
            ReadInput();
            yield return null;
        }
    }
}
