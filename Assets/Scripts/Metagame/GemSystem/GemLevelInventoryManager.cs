using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private GameObject[] _currentSlottedGems;
    public int _levelSlotIndex = 0;

    private void Start()
    {
        _currentSlottedGems = new GameObject[_levelSlots.Length];
    }

    private void OnEnable()
    {
        LoadGemsFromInventory();   

    }

    public void LoadGemsFromInventory()
    {
        var inventorySlotIndex = 0;
        
        DestroyCurrentItemsInInventory();
        
        foreach (var materialItem in _library.MatlerialsLibrarySo.Materials)
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


        var gemInSlot = Instantiate(_inventoryItem, Vector2.zero, Quaternion.identity);
        for (int i = 0; i < _levelSlots.Length; i++)
        {
            if (_currentSlottedGems[i] != null) continue;
            _levelSlotIndex = i;
            break;
        }
        _currentSlottedGems[_levelSlotIndex] = gemInSlot;
        
        gemInSlot.transform.position = _levelSlots[_levelSlotIndex].transform.position;
        gemInSlot.GetComponent<LevelGemSlot>().SetItemSlot(gem);
        gemInSlot.GetComponentInChildren<TMP_Text>().SetText("");
        gemInSlot.transform.parent = _levelSlotParentTransform;
    }

    public void DestroyCurrentSlottedGems()
    {
        foreach (var slottedGem in _currentSlottedGems)
        {
            Destroy(slottedGem);
        }


    }
    public bool FreeSlotExist()
    {
        return _currentSlottedGems.Any(slottedGem => slottedGem == null);
    }
    
    private void OnDisable()
    {
        _levelSlotIndex = 0;
    }

    public void SaveGemModifiersOnPlay() //Called on starting level button
    {
        foreach (var slottedGem in _currentSlottedGems)
        {
            var slottedGemData = slottedGem.GetComponent<LevelGemSlot>()._item;
            PlayerPrefs.SetFloat(slottedGemData.LevelBonusID, slottedGemData.LevelBonus);
        }
    }

}
