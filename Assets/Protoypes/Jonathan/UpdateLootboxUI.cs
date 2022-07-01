using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLootboxUI : MonoBehaviour
{
    [SerializeField] private LootBoxController lootBoxController;
    private void OnEnable() => lootBoxController.OnUpdateItemUI += SetUIElement;

    private void OnDisable()=> lootBoxController.OnUpdateItemUI -= SetUIElement;

    // public Image[] itemSprite;
    // public TMP_Text ItemName;
    // public TMP_Text Rarity;
    

    private void SetUIElement( GameObject itemObject, InventoryItem item)
    {
           itemObject.GetComponent<Image>().sprite = item.GetIcon();
    }

    
}
