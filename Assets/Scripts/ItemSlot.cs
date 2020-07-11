using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    int slotID = 0;

    //The items occupying the slot
    Stack<Item> itemStack = new Stack<Item>(99);

    //Boolean checking if the slots occupied
    bool occupied = false;

    //How much of the same item is on this slot
    int stackValue = 0;

    [Header("Quantity"), SerializeField]
    private TextMeshProUGUI quantity;

    [Header("Item Image"), SerializeField]
    private Image itemImage;

    /// <summary>
    /// Set the item
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        if (stackValue < 99)
        {
            itemStack.Push(item);
            SetItemImage(item.Image);
            CheckOccupancy();
            UpdateStackValue();
        }
    }

    /// <summary>
    /// Use an item from this slot
    /// </summary>
    public void Use()
    {
        if (occupied)
        {
            Item usedItem = itemStack.Pop();
            usedItem.EV_OnUse.Trigger();
        }
    }

    /// <summary>
    /// Clear out all items
    /// </summary>
    public void Clear()
    {
        itemStack.Clear();
    }

    /// <summary>
    /// Check if this slot is occupied
    /// </summary>
    /// <returns></returns>
    bool CheckOccupancy() => occupied = (itemStack != null || stackValue != 0);

    /// <summary>
    /// Update the value of item quantity
    /// </summary>
    void UpdateStackValue()
    {
        //Check if slot is occupied
        if (occupied)
        {
            stackValue = itemStack.Count;
        }
        else
        {
            stackValue = 0;
        }

        SetQuantityText(stackValue);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    void SetQuantityText(int value)
    {
        quantity.text = value.ToString();
    }

    void SetItemImage(Sprite image)
    {
        itemImage.sprite = image;
    }
}
