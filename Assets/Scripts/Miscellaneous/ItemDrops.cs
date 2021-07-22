using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RNG = UnityEngine.Random;

public class ItemDrops : MonoBehaviour
{
    [Serializable]
    public class ItemChances
    {
        [SerializeField]
        string itemName;

        [SerializeField]
        float chanceRate;

        public float GetChanceRate() => chanceRate;
        public GameObject GetItem() => ObjectPooler.GetMember(itemName);
    }

    [SerializeField]
    private List<ItemChances> itemChanceRates;

    /// <summary>
    /// Drop an item based on choice
    /// </summary>
    /// <returns></returns>
    public void Drop()
    {
        //Get the cumulative amount of the 
        foreach(ItemChances itemChanceRate in itemChanceRates)
        {
            float chance = itemChanceRate.GetChanceRate();
            float randomVal = RNG.Range(0f, 1f);
            if (randomVal <= chance)
            {
                GameObject item = itemChanceRate.GetItem();

                if (!item.activeInHierarchy)
                {
                    item.SetActive(true);
                    item.transform.position = transform.position;
                    item.transform.rotation = Quaternion.identity;

                    
                }

                //Throw it up a bit.
                Vector2 upforce = new Vector2(0, 10f);
                item.GetComponent<Rigidbody2D>().AddForce(upforce);
            }
        }
    }
}
