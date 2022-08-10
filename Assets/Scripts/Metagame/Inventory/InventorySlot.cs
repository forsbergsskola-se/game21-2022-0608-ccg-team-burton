using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour//, IPointerClickHandler
{

    private Image spriteRenderer;
    [SerializeField] private TMP_Text amount;


    private void Awake()
    {
        spriteRenderer = GetComponent<Image>();
    }

    public void SetItemSlot(ActionItem item)
    {
        spriteRenderer.sprite = item.GetIcon();
        name = item.GetDisplayName();
        amount.SetText(PlayerPrefs.GetInt(item.GetItemID()).ToString()); //TODO: Move to update UI event if time
    }
}

