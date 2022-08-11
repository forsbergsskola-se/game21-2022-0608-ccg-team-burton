using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUIUpdater : MonoBehaviour
{

    [SerializeField] private UICurrencyUpdater currencyUpdater;
    
    [SerializeField] private TMP_Text coinAmountText;
    [SerializeField] private TMP_Text buttonAmountText;


    private void OnEnable()
    {
        currencyUpdater.OnCurrencyChanged += UpdateCurrencyUI;
    }

    private void OnDisable()
    {
        currencyUpdater.OnCurrencyChanged -= UpdateCurrencyUI;

    }

    private void UpdateCurrencyUI()
    {

        coinAmountText.SetText(PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()).ToString());
        buttonAmountText.SetText(PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentButtons.ToString()).ToString());

    }
    

}
