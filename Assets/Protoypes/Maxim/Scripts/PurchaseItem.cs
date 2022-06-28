using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseItem : MonoBehaviour
{
    
    private int _currentCoins;
    public int _price;

    public Action<int> OnCurrencyChange;
    
    public void Start()
    {
        _currentCoins = 1900;
       OnCurrencyChange?.Invoke(_currentCoins);
    }
    

    public void PressBuy()
    {
        
        if (_price <= _currentCoins)
        {
            Debug.Log("Point Counter" + _currentCoins);
            _currentCoins -= _price;
            OnCurrencyChange?.Invoke(_currentCoins);
        }
    }
    

}
