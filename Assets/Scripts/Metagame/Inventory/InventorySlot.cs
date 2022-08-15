using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    private Image spriteRenderer;
    [SerializeField] private TMP_Text amount;


    private void Awake()
    {
        spriteRenderer = GetComponent<Image>();
    }

    public void SetItemSlot(MaterialItem item)
    {
        spriteRenderer.sprite = item.GetIcon();
        name = item.GetDisplayName();
        amount.SetText(PlayerPrefs.GetInt(item.GetItemID()).ToString()); //TODO: Move to update UI event if time
    }
 
}

