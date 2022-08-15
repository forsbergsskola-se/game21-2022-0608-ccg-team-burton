using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelInventorySlotLevel : MonoBehaviour, IPointerClickHandler
{
    private Image _spriteRenderer;
    [SerializeField] private TMP_Text _amount;
    private MaterialItem _item;

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
        Debug.Log($"Item: {_item.GetDisplayName()}");
    }
}
