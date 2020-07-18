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
        GameObject itemPrefab;

        [SerializeField]
        float chanceRate;

        public float GetChanceRate() => chanceRate;
        public GameObject GetItem() => itemPrefab;
    }

    [SerializeField]
    private List<ItemChances> itemChanceRates;

    /// <summary>
    /// Drop an item based on choice
    /// </summary>
    /// <returns></returns>
    GameObject Drop()
    {
        //Get the cumulative amount of the 
        foreach(ItemChances itemChanceRate in itemChanceRates)
        {
            float chance = itemChanceRate.GetChanceRate();
            float randomVal = RNG.Range(0, 1);

            if (randomVal <= chance) return itemChanceRate.GetItem();
        }

        return null;
    }
}
