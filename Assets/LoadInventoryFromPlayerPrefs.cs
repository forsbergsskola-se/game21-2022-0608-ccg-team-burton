using System.Collections.Generic;
using UnityEngine;

public class LoadInventoryFromPlayerPrefs : MonoBehaviour
{
    [SerializeField]
    private ItemLibrary itemLibrary;

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
    public void UpdateInventory()
    {
        if (currentItems.Count > 0)
        {
            DestroyCurrentItemsInInventory();
            
        }
        var index = 0;
        
        foreach (var item in itemLibrary.ItemLibrarySos.Library)
        {
            // Debug.Log($"{item.GetDisplayName()} in inventory with count: {PlayerPrefs.GetInt(item.GetItemID())}");

            if (PlayerPrefs.GetInt(item.GetItemID()) <= 0) continue;
            var itemInSlot = Instantiate(inventorySlot, inventorySlots[index].transform.position , Quaternion.identity);
            currentItems.Add(itemInSlot);
            itemInSlot.transform.parent = ui.transform;
            itemInSlot.GetComponent<BUSetSlot>().SetSlot(item);
                
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