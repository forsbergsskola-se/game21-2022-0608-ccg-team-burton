using System.Collections.Generic;
using UnityEngine;

public class LoadInventoryFromPlayerPrefs : MonoBehaviour
{
    [SerializeField]
    private Libraries libraries;

    [SerializeField]
    private GameObject inventorySlot;

    [SerializeField]
    private GameObject[] inventorySlots;

    [SerializeField]
    private GameObject ui;

    public List<GameObject> currentItems;
    private void Start()
    {
      UpdateInventory();
    }

    
    
    
    //Yes this is inefficient. But it is million times better than no system at all...
    private void UpdateInventory()
    {
        if (currentItems.Count > 0)
        {
            DestroyCurrentItemsInInventory();
        }
        var index = 0;
        
        foreach (var item in libraries.MatlerialsLibrarySo.InventoryLibrary)
        {
            if (PlayerPrefs.GetInt(item.GetItemID()) <= 0) continue;
            var itemInSlot = Instantiate(inventorySlot, inventorySlots[index].transform.position , Quaternion.identity);
            currentItems.Add(itemInSlot);
            itemInSlot.transform.parent = ui.transform;
            itemInSlot.GetComponent<InventorySlot>().SetItemSlot(item);
            index++;
        }
    }
    private void DestroyCurrentItemsInInventory()
    {
        foreach (var currentItem in currentItems)
        {
            Destroy(currentItem);
        }
    }
}