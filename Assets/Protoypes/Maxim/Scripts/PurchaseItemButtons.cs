using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseItemButtons : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public int _price;

 

    public void PressBuy()
    {
        var currentButtons = PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentButtons.ToString());
        if (_price <= currentButtons)
        {
            currentButtons -= _price;
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentButtons.ToString(), currentButtons);

            sceneLoader.LoadScene();
        }
    }
}
