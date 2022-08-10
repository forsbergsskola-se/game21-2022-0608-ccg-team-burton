using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUIUpdater : MonoBehaviour
{

    [SerializeField] private Initialization init;
    
    [SerializeField] private TMP_Text coinAmountText;
    [SerializeField] private TMP_Text buttonAmountText;


    private void OnEnable()
    {
        init.OnInitComplete += UpdateCurrencyUI;
    }

    private void OnDisable()
    {
        init.OnInitComplete -= UpdateCurrencyUI;
    }

    private void UpdateCurrencyUI()
    {

        coinAmountText.SetText(PlayerPrefs.GetInt("CurrentCoins").ToString());
        buttonAmountText.SetText(PlayerPrefs.GetInt("CurrentButtons").ToString());

    }
    

}
