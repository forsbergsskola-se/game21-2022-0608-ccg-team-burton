using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PurchaseItem : MonoBehaviour
{
    public SceneLoader sceneLoader;
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
            // Save current coins to inentory
            OnCurrencyChange?.Invoke(_currentCoins);
            sceneLoader.LoadScene();
        }
    }
    

}
