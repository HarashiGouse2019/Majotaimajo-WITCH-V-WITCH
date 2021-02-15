//using System;
//using System.Collections;
//using UnityEngine;

//using static Keymapper;

//[SerializeField]
//public abstract class Item
//{
//    public string Name;
//    public string Description;
//    public Sprite Image;
//    public EventManager.Event @EV_OnUse;
//}

//[Serializable]
//public class ItemInventory : MonoBehaviour
//{
//    private static ItemInventory Instance;

//    /*I just thought about this just a moment ago. One of the tasks that I was given isn't really working for me...
//     But I can definitely get this one up and running.
    
//     Raven will only be able to take 3 items with her. She can select her items with the space bar,
//    tapping how many times she needs to in order to select her item during the game.
//    At the start of the game, you are given 3 Magic Potions that restore your magic occupying one slot

//    The other 3 slots will be open after each battle.

//    You can stack the same item on to a slot if you want to. A little number will show as to what the item is
//    (which will be a prefab, as well as a scriptable object, because scriptable objects will really be useful in this case.)
//     */

//    //Our array of items
//    [SerializeField]
//    private ItemSlot[] itemSlots = new ItemSlot[4];

//    //Item Position
//    int itemSlotPosition = 0;

//    //A timer
//    float inventoryTimer = 0;

//    //How long you have to hold space to use an item
//    const float USE_DURATION = 1f;

//    //The limit of secs to hold down space to tell the system you're switching
//    const float SWITCH_DURATION = 0.2f;

//    const float RESET_TIMER = 0;

//    private void Awake()
//    {
//        #region Singleton
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(Instance);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//        #endregion
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        StartCoroutine(InventorySystemCycle());
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }


//    /// <summary>
//    /// Add an item to a selected slot
//    /// </summary>
//    /// <param name="slotNumber"></param>
//    /// <param name="item"></param>
//    public static void AddNewItem(int slotNumber, Item item)
//    {
//        Instance.itemSlots[slotNumber].SetItem(item);
//    }


//    /// <summary>
//    /// Add a quantity amount of items to a selected slot
//    /// </summary>
//    /// <param name="slotNumber"></param>
//    /// <param name="item"></param>
//    /// <param name="quantity"></param>
//    public static void AddNewItem(int slotNumber, Item item, int quantity)
//    {
//        for (int iteration = 0; iteration < quantity; iteration++)
//        {
//            Instance.itemSlots[slotNumber].SetItem(item);
//        }
//    }

//    /// <summary>
//    /// Clear entire slot
//    /// </summary>
//    /// <param name="slotNumber"></param>
//    public static void ClearSlot(int slotNumber)
//    {
//        Instance.itemSlots[slotNumber].Clear();
//    }

//    void ManageSlotPosition()
//    {
//        if (itemSlotPosition == itemSlots.Length)
//            itemSlotPosition = 0;
//        else if (itemSlotPosition < 0)
//            itemSlotPosition = itemSlots.Length - 1;
//    }

//    public static int GetItemPosition() => Instance.itemSlotPosition;

//    IEnumerator InventorySystemCycle()
//    {
//        while (true)
//        {
//            //If holding down space, keep track of time.
//            if (OnKey("itemSelection") && inventoryTimer < USE_DURATION)
//            {
//                inventoryTimer += Time.deltaTime;

//                //If inventoryTimer is creater than the USE_DURATION
//                if (inventoryTimer >= USE_DURATION && itemSlots[itemSlotPosition] != null)
//                {
//                    itemSlots[itemSlotPosition].Use();
//                    inventoryTimer = RESET_TIMER;
//                }
//            }

//            //If the player releases the key in under 0.20 secs, this means they want to switch
//            if (OnKeyRelease("itemSelection"))
//            {
//                float pressDuration = inventoryTimer;
//                inventoryTimer = RESET_TIMER;

//                if (pressDuration <= SWITCH_DURATION)
//                {
//                    itemSlotPosition++;
//                    ManageSlotPosition();
//                    ItemSelectorCursor.UpdatePosition();
//                }
//            }

//            yield return null;
//        }
//    }
//}
