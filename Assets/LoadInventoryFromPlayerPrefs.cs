using UnityEngine;

public class LoadInventoryFromPlayerPrefs : MonoBehaviour
{
    [SerializeField]
    private ItemLibrary ItemLibrary;
    private void Start()
    {
        foreach (var item in ItemLibrary.ItemLibrarySos.GemLibrary)
        {
                Debug.Log($"{item.GetDisplayName()} in inventory with count: {PlayerPrefs.GetInt(item.GetItemID())}");
        }
    }
}