using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemInventoryLoader : MonoBehaviour
{

    [SerializeField] private GameObject[] _levelSlots;
    [SerializeField] private GameObject[] _inventorySlots;
    [SerializeField] private Libraries _library;
    [SerializeField] private GameObject _inventoryItem;
    [SerializeField] private Transform _inventorySlotParentTransform;
    private List<GameObject> _currentItems = new() ;

    
    // Start is called before the first frame update
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
        _currentItems.Add(gemInSlot);
        gemInSlot.transform.parent = _inventorySlotParentTransform;
        gemInSlot.GetComponent<LevelInventorySlotLevel>().SetItemSlot(materialItem);
        index++;
        return false;
    }

    private void DestroyCurrentItemsInInventory()
    {
        foreach (var currentItem in _currentItems)
        {
            Destroy(currentItem);
        }
    }
}
