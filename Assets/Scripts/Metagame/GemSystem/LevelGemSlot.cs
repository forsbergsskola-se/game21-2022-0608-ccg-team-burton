using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelGemSlot : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private TMP_Text _amount;
    private Image _spriteRenderer;
    private GemLevelInventoryManager _slotManager => FindObjectOfType<GemLevelInventoryManager>();
    private MaterialItem _item { get; set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<Image>();
    }

    public void SetItemSlot(MaterialItem item)
    {
        _item = item;
        _spriteRenderer.sprite = item.GetIcon();
        name = item.GetDisplayName();
        _amount.SetText(PlayerPrefs.GetInt(item.GetItemID()).ToString()); //TODO: Move to update UI event if time
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent.name.Contains("Inventory") && _slotManager.FreeSlotExist())
        {
            PlayerPrefs.SetInt(_item.GetItemID(), PlayerPrefs.GetInt(_item.GetItemID())-1);

          _slotManager.SlotGemInLevel(_item);
            
        } else if(transform.parent.name.Contains("Gem"))
        {
            Destroy(gameObject);
            
        }
        _slotManager.LoadGemsFromInventory();
    }

    private void OnDisable()
    {
        if (!transform.parent.name.Contains("Gem")) return; //TODO JESUS CHRIST, TAKE THE WHEEL! This is baaaaaad
        PlayerPrefs.SetInt(_item.GetItemID(), PlayerPrefs.GetInt(_item.GetItemID())+1);
        Destroy(gameObject);
    }
}
