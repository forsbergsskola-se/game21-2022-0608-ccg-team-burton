using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLootboxUI : MonoBehaviour
{
    [SerializeField] private LootBoxController lootBoxController;
    private void OnEnable() => lootBoxController.OnUpdateItemUI += SetUI;

    private void OnDisable()=> lootBoxController.OnUpdateItemUI -= SetUI;

    public Image[] itemSprite;
    public TMP_Text ItemName;
    public TMP_Text Rarity;
    

    private void SetUI( int i, Item item)
    {
        if (item is Armor armor)
        {
           itemSprite[i].sprite = armor.ItemSprite;
 
            
        }

    }

    
}
