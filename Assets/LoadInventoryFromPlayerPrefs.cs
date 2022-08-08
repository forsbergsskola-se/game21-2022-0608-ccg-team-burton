using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInventoryFromPlayerPrefs : MonoBehaviour
{
    [SerializeField]
    private ItemLibrary ItemLibrary;
    private void Start()
    {
        foreach (var item in ItemLibrary.GemLibrary.GemLibrary)
        {

                Debug.Log($"{item.GetDisplayName()} in inventory with count: {PlayerPrefs.GetInt(item.GetItemID())}");
            
   
        }
    }
}
