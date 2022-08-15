using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemLevelInventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _levelSlots;
    [SerializeField] private GameObject[] _inventorySlots;
    [SerializeField] private Libraries _library;
    [SerializeField] private GameObject _inventoryItem;
    [SerializeField] private Transform _inventorySlotParentTransform;
    [SerializeField] private Transform _levelSlotParentTransform;
    private List<GameObject> _currentItemsInInventory = new();
    private int _levelSlotIndex = 0;

 
    private void OnEnable()
    {
        LoadGemsFromInventory();   

    }

    public void LoadGemsFromInventory()
    {
        var inventorySlotIndex = 0;
        
        DestroyCurrentItemsInInventory();
        
        foreach (var materialItem in _library.MatlerialsLibrarySo.InventoryLibrary)
        {
            if (materialItem.GetItemID().Contains("red")||materialItem.GetItemID().Contains("blue")||materialItem.GetItemID().Contains("green"))
            {
                if (SetItemOnSlot(materialItem, ref inventorySlotIndex)) ;
            }
        }
    }

    private bool SetItemOnSlot(MaterialItem materialItem, ref int inventorySlotIndex)
    {
        if (PlayerPrefs.GetInt(materialItem.GetItemID()) <= 0)
            return true;

        var gemInSlot = Instantiate(_inventoryItem, _inventorySlots[inventorySlotIndex].transform.position, Quaternion.identity);
        _currentItemsInInventory.Add(gemInSlot);
        gemInSlot.transform.parent = _inventorySlotParentTransform;
        gemInSlot.GetComponent<LevelGemSlot>().SetItemSlot(materialItem);
        inventorySlotIndex++;
        return false;
    }

    private void DestroyCurrentItemsInInventory()
    {
        foreach (var currentItem in _currentItemsInInventory)
        {
            Destroy(currentItem);
        }
    }

    public void SlotGemInLevel(MaterialItem gem)
    {

        if (!FreeSlotExist())
            return;
        Debug.Log($"Time to slot: {gem.GetDisplayName()}");
        var gemInSlot = Instantiate(_inventoryItem, _levelSlots[_levelSlotIndex].transform.position, Quaternion.identity);
        gemInSlot.GetComponent<LevelGemSlot>().SetItemSlot(gem);
        gemInSlot.GetComponentInChildren<TMP_Text>().SetText("");
        gemInSlot.transform.parent = _levelSlotParentTransform;
        _levelSlotIndex++;
        
        LoadGemsFromInventory();
    }
    public bool FreeSlotExist()
    {
        return _levelSlotIndex < _levelSlots.Length;
    }
    
    private void OnDisable()
    {
        _levelSlotIndex = 0;
    }

}
