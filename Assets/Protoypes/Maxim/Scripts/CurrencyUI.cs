using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField]private PurchaseItem purchaseItem;
    
    [SerializeField] public TextMeshProUGUI coinTxt;

    private void OnEnable()
    {
        purchaseItem.OnCurrencyChange += UpdateCoinText;
    }

    private void OnDisable()
    {
        purchaseItem.OnCurrencyChange -= UpdateCoinText;
    }

    public void UpdateCoinText(int value)
    {
        coinTxt.text = "Coins: " + value ;
    }
}
