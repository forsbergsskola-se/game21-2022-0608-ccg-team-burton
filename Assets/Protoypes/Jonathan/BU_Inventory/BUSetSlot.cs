using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BUSetSlot : MonoBehaviour//, IPointerClickHandler
{

    private Image spriteRenderer;
    [SerializeField] private TMP_Text amount;
    private ActionItem item;
    private LoadInventoryFromPlayerPrefs inventoryHandler;


    private void Awake()
    {
        spriteRenderer = GetComponent<Image>();
        inventoryHandler = FindObjectOfType<LoadInventoryFromPlayerPrefs>();
    }

    public void SetSlot(ActionItem item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetIcon();
        name = item.GetDisplayName();
        amount.SetText(PlayerPrefs.GetInt(item.GetItemID()).ToString()); //TODO: Move to update UI event if time
    }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     Debug.Log($"Pointer on: {name}!");
    //     PlayerPrefs.SetInt(item.GetItemID(), PlayerPrefs.GetInt(item.GetItemID()) - 1);
    //     amount.SetText(PlayerPrefs.GetInt(item.GetItemID()).ToString()); //TODO: Move to update UI event if time
    //     
    //     if (PlayerPrefs.GetInt(item.GetItemID()) <= 0)
    //     {
    //         inventoryHandler.UpdateInventory();
    //     }
    //
    // }
}

