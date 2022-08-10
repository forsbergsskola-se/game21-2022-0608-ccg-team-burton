using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CurrencyShopUI : MonoBehaviour
{
    [SerializeField]private PurchaseItemCoins purchaseCommonBox;
    [SerializeField]private PurchaseItemButtons purchaseRareBox;
    [SerializeField]private PurchaseItemButtons purchaseEpicBox;
    
    [SerializeField] public TextMeshProUGUI coinsTxt;
    [SerializeField] public TextMeshProUGUI buttonsTxt;

    private void OnEnable()
    {
        // purchaseCommonBox.OnCurrencyChange += UpdateCoinText;
        // purchaseRareBox.OnCurrencyChange += UpdateButtonsText;
        // purchaseEpicBox.OnCurrencyChange += UpdateButtonsText;
    }

    private void OnDisable()
    {
        // purchaseCommonBox.OnCurrencyChange -= UpdateCoinText;
        // purchaseRareBox.OnCurrencyChange += UpdateButtonsText;
        // purchaseEpicBox.OnCurrencyChange += UpdateButtonsText;
    }

    // public void UpdateCoinText(int value)
    // {
    //     coinsTxt.text = "Coins: " + value ;
    // }
    //
    // public void UpdateButtonsText(int value)
    // {
    //     buttonsTxt.text = "Buttons: " + value;
    // }
}
