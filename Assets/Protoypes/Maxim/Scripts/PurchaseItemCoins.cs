using System;
using UnityEngine;


public class PurchaseItemCoins : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public int _price;
 
    

    public void PressBuy()
    {
        var currentCoins = PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString());

        if (_price <= currentCoins)
        {
            currentCoins -= _price;
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), currentCoins);
            // Save current coins to inventory
            sceneLoader.LoadScene();
        }
    }
    

}
