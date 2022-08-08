using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



//Dont want library in scene
public class ItemLibrary : MonoBehaviour
{

    // public  ItemLibrarySO ItemLibrary;
    // public  ItemRarityLibrarySO RarityLibrary;
    public  GemLibrarySO GemLibrary;

   


    // public ItemSO GetItemFromLibrary(string itemSoID)
    // {
    //     //TODO: If dictionary would be nice
    //     foreach (var itemSo in ItemLibrary.ItemsLibrary.Where(itemSo => itemSo.ID == itemSoID))
    //     {
    //         return itemSo;
    //     }
    //
    //     Debug.Log("Warning: No item was found with that ID in library. Returning Null");
    //     return null;
    // }

    // public ItemRaritySO GetRarityFromLibrary(string rarityID)
    // {
    //     //TODO: If dictionary would be nice
    //     foreach (var itemRaritySo in RarityLibrary.ItemRarityLibrary.Where(itemRaritySo => itemRaritySo.ID == rarityID))
    //     {
    //         return itemRaritySo;
    //     }
    //
    //     Debug.Log("Warning: No Rarity was found with that ID in library. Returning Null");
    //     return null; 
    // }

    public ActionItem GetGemFromLibrary(string gemID)
    {
        //TODO: If dictionary would be nice
        foreach (var gemSo in GemLibrary.GemLibrary.Where(gemSo => gemSo.GetItemID() == gemID))
        {
            return gemSo;
        }
    
        Debug.Log("Warning: No gem was found with that ID in library. Returning Null");
        return null; 
    }
    
    
    
}
