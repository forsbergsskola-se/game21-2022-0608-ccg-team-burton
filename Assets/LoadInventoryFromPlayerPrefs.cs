using UnityEngine;

public class LoadInventoryFromPlayerPrefs : MonoBehaviour
{
    [SerializeField]
    private ItemLibrary ItemLibrary;

    [SerializeField]
    private GameObject InventorySlot;

    [SerializeField] private GameObject UI;
 
    private void Start()
    {
        int i = 0;
        int j = 0;
        foreach (var item in ItemLibrary.ItemLibrarySos.GemLibrary)
        {
                Debug.Log($"{item.GetDisplayName()} in inventory with count: {PlayerPrefs.GetInt(item.GetItemID())}");
                if (PlayerPrefs.GetInt(item.GetItemID()) > 0)
                {
                    var slot = Instantiate(InventorySlot, transform.position,
                        Quaternion.identity);
                    slot.transform.parent = transform;
                    slot.GetComponent<BUSetSlot>().SetSlot(item);
                    i++;
                    if (i > 1)
                    {
                        i = 0;
                        j++;
                    }

                }

        }
    }
}