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
    private void Start()
    {
        int i = 0;
        int j = 0;
        int index = 0;
        
        foreach (var item in ItemLibrary.ItemLibrarySos.Library)
        {
                // Debug.Log($"{item.GetDisplayName()} in inventory with count: {PlayerPrefs.GetInt(item.GetItemID())}");

                if (PlayerPrefs.GetInt(item.GetItemID()) <= 0) continue;
                var slot = Instantiate(InventorySlot, inventorySlots[index].transform.position , Quaternion.identity);
                    
                slot.transform.parent = UI.transform;
                slot.GetComponent<BUSetSlot>().SetSlot(item);
                
                index++;
                i++;
                if (i <= 1) continue;
                i = 0;
                j++;

        }
    }
}