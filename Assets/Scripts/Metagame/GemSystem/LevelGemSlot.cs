using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelGemSlot : MonoBehaviour, IPointerClickHandler
{
    private Image _spriteRenderer;
    private GemLevelInventoryManager slotManager => FindObjectOfType<GemLevelInventoryManager>();
    [SerializeField] private TMP_Text _amount;
    public MaterialItem _item { get; private set; }

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
        PlayerPrefs.SetInt(_item.GetItemID(), PlayerPrefs.GetInt(_item.GetItemID())-1);


        if (transform.parent.name.Contains("Inventory"))
        {
          // pressedGem.SlotGemInLevel(_item);
          slotManager.SlotGemInLevel(_item);
            
        } else if(transform.parent.name.Contains("Gem"))
        {
            PlayerPrefs.SetInt(_item.GetItemID(), PlayerPrefs.GetInt(_item.GetItemID())+1);
           // FindObjectOfType<GemLevelInventoryManager>().LoadGemsFromInventory();
            Destroy(gameObject);
            
        }
    }

    private void OnDisable()
    {
        if (transform.parent.name.Contains("Gem")) //TODO JESUS CHRIST, TAKE THE WHEEL! This is soooo bad
        {
            PlayerPrefs.SetInt(_item.GetItemID(), PlayerPrefs.GetInt(_item.GetItemID())+1);
            Destroy(gameObject);
        }
    }
}
