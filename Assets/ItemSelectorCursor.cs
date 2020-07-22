using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectorCursor : MonoBehaviour
{
    private static ItemSelectorCursor Instance;
    /*Takes in 4 positions that it changes to depending on what Item you are picking
     Have reference to the ItemInventory, and put in your position.
     Cursor will always be on the select item. Player has to hold down the button to use that item*/

    //Reference to camera. This will be important
    [SerializeField]
    Camera GameUiCamera;

    [SerializeField]
    Vector3[] cursorPositions = new Vector3[4];

    private void Awake()
    {
        Instance = this;
    }

    public static void UpdatePosition()
    {
        Instance.transform.localPosition = Instance.cursorPositions[ItemInventory.GetItemPosition()];
    }
}
