using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GetItemFromLibrary : MonoBehaviour
{

    public ItemLibrarySO ItemLibrary;

    
    //Loaded from save/load system
    public string ItemSoID;

    public ItemSO FetchedItemSo;


    //TODO: DEBUG for checking if item is in library
    private void Start()
    {
        FetchedItemSo = LoadItemFromLibrary(ItemSoID);

    }


    private ItemSO LoadItemFromLibrary(string itemSoID)
    {
        foreach (var itemSo in ItemLibrary.ItemsLibrary.Where(itemSo => itemSo.ID == itemSoID))
        {
            return itemSo;
        }

        Debug.Log("Warning: No item was found with that ID in library. Returning Null");
        return null;
    }
}
