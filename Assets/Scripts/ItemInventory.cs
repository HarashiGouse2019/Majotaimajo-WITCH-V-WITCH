using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    string name;
    string description;
    Sprite image;
    EventManager.Event @ev_OnUse;
}

public class ItemInventory : MonoBehaviour
{
    /*I just thought about this just a moment ago. One of the tasks that I was given isn't really working for me...
     But I can definitely get this one up and running.
    
     Raven will only be able to take 3 items with her. She can select her items with the space bar,
    tapping how many times she needs to in order to select her item during the game.
    At the start of the game, you are given 3 Magic Potions that restore your magic occupying one slot

    The other 2 slots will be open after each battle.

    You can stack the same item on to a slot if you want to. A little number will show as to what the item is
    (which will be a prefab, as well as a scriptable object, because scriptable objects will really be useful in this case.)
     */

    public class ItemSlot
    {
        //The items occupying the slot
        Item item = null;
        
        //Boolean checking if the slots occupied
        bool occupied = false;

        //How much of the same item is on this slot
        int stackValue = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
