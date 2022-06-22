using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



//Dont want library in scene
public class GetFromLibrary : MonoBehaviour
{

    public static ItemLibrarySO ItemLibrary;
    public static ItemRarityLibrarySO RarityLibrary;
    public static GemLibrarySO GemLibrary;

    //
    // //Loaded from save/load system
    // public string ItemSoID;
    //
    // public ItemSO FetchedItemSo;
    //
    //
    // //TODO: DEBUG for checking if item is in library
    // private void Start()
    // {
    //     FetchedItemSo = LoadItemFromLibrary(ItemSoID);
    // }


    public static ItemSO GetItemFromLibrary(string itemSoID)
    {
        Debug.Log($"LOADING ITEM {itemSoID}");
        //TODO: If dictionary would be nice
        foreach (var itemSo in ItemLibrary.ItemsLibrary.Where(itemSo => itemSo.ID == itemSoID))
        {
            return itemSo;
        }

        Debug.Log("Warning: No item was found with that ID in library. Returning Null");
        return null;
    }

    public static ItemRaritySO GetRarityFromLibrary(string rarityID)
    {
        Debug.Log($"LOADING ITEM {rarityID}");
        //TODO: If dictionary would be nice
        foreach (var itemRaritySo in RarityLibrary.ItemRarityLibrary.Where(itemRaritySo => itemRaritySo.RarityID == rarityID))
        {
            return itemRaritySo;
        }

        Debug.Log("Warning: No Rarity was found with that ID in library. Returning Null");
        return null; 
    }

    public static GemSO GetGemFromLibrary(string gemID)
    {
        Debug.Log($"LOADING ITEM {gemID}");
        //TODO: If dictionary would be nice
        foreach (var gemSo in GemLibrary.GemLibrary.Where(gemSo => gemSo.ID == gemID))
        {
            return gemSo;
        }

        Debug.Log("Warning: No gem was found with that ID in library. Returning Null");
        return null; 
    }
    
    
    
}
