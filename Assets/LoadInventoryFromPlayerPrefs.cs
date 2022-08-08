using System.Collections.Generic;
using UnityEngine;

public class LoadInventoryFromPlayerPrefs : MonoBehaviour
{
    [SerializeField]
    private ItemLibrary ItemLibrary;

    [SerializeField]
    private GameObject InventorySlot;

    [SerializeField]
    private GameObject[] inventorySlots;

    [SerializeField]
    private GameObject UI;

    public List<GameObject> currentItems;
    private void Start()
    {
      UpdateInventory();
    }

    private void DestroyCurrentItemsInInventory()
    {
        foreach (var currentItem in currentItems)
        {
            Destroy(currentItem);
        }
    }
    
    //Yes this is inefficient. But it is million times better than no system at all...
    public void UpdateInventory()
    {
        DestroyCurrentItemsInInventory();
        var index = 0;
        
        foreach (var item in ItemLibrary.ItemLibrarySos.Library)
        {
            // Debug.Log($"{item.GetDisplayName()} in inventory with count: {PlayerPrefs.GetInt(item.GetItemID())}");

            if (PlayerPrefs.GetInt(item.GetItemID()) <= 0) continue;
            var itemInSlot = Instantiate(InventorySlot, inventorySlots[index].transform.position , Quaternion.identity);
            currentItems.Add(itemInSlot);
            itemInSlot.transform.parent = UI.transform;
            itemInSlot.GetComponent<BUSetSlot>().SetSlot(item);
                
            index++;


        }
    }
}