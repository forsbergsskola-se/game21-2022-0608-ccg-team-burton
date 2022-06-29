using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseItemButtons : MonoBehaviour
{
    public SceneLoader sceneLoader;
    private int _currentButtons;
    public int _price;

    public Action<int> OnCurrencyChange;

    public void Start()
    {
        _currentButtons = 200;
        OnCurrencyChange?.Invoke(_currentButtons);
    }
    

    public void PressBuy()
    {
        
        if (_price <= _currentButtons)
        {
            _currentButtons -= _price;
            // Save current coins to inentory
            OnCurrencyChange?.Invoke(_currentButtons);
            sceneLoader.LoadScene();
        }
    }
}
