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

    private void SetUIElement( GameObject itemObject, InventoryItem item)
    {
           itemObject.GetComponent<Image>().sprite = item.GetIcon();
    }
}
