using System.Collections;
using System.Collections.Generic;
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
    private List<GameObject> _currentSlottedGems = new();
    private List<GameObject> _currentItemsInInventory = new() ;
    
    void Start()
    {
     LoadGemsFromInventory();   
    }

    public void LoadGemsFromInventory()
    {
        var index = 0;
        
        DestroyCurrentItemsInInventory();
        
        foreach (var materialItem in _library.MatlerialsLibrarySo.InventoryLibrary)
        {
            if (materialItem.GetItemID().Contains("red")||materialItem.GetItemID().Contains("blue")||materialItem.GetItemID().Contains("green"))
            {
                if (SetItemOnSlot(materialItem, ref index)) ;
            }
        }
    }

    private bool SetItemOnSlot(MaterialItem materialItem, ref int index)
    {
        if (PlayerPrefs.GetInt(materialItem.GetItemID()) <= 0)
            return true;

        var gemInSlot = Instantiate(_inventoryItem, _inventorySlots[index].transform.position, Quaternion.identity);
        _currentItemsInInventory.Add(gemInSlot);
        gemInSlot.transform.parent = _inventorySlotParentTransform;
        gemInSlot.GetComponent<LevelInventorySlotLevel>().SetItemSlot(materialItem);
        index++;
        return false;
    }

    private void DestroyCurrentItemsInInventory()
    {
        foreach (var currentItem in _currentItemsInInventory)
        {
            Destroy(currentItem);
        }
    }

    private int index = 0;
    public void SlotGemInLevel(MaterialItem gem)
    {

        if (index >= _levelSlots.Length)
            return;
        Debug.Log($"Time to slot: {gem.GetDisplayName()}");
        var gemInSlot = Instantiate(_inventoryItem, _levelSlots[index].transform.position, Quaternion.identity);
        gemInSlot.transform.parent = _levelSlotParentTransform;
        index++;
        
        PlayerPrefs.SetInt(gem.GetItemID(), PlayerPrefs.GetInt(gem.GetItemID())-1);
        LoadGemsFromInventory();

    }
}
