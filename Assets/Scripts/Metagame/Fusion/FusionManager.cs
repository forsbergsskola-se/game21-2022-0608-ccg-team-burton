using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{
    public void InitiateUpgrade(EquipmentSO equipmentData, ActionItem upgradeMaterial)
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString()) > PlayerPrefs.GetInt(upgradeMaterial.GetItemID()))
        {
            Debug.Log("Not enough materials!");
        }
        else
        {
            Debug.Log("Upgrading!");
            var newBalance =PlayerPrefs.GetInt(upgradeMaterial.GetItemID()) - PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString());
            PlayerPrefs.SetInt(upgradeMaterial.GetItemID(), newBalance);
            
            var currentRarityString = PlayerPrefs.GetString(equipmentData.RarityID);
            Enum.TryParse(currentRarityString, out Rarity currentRarity);
            currentRarity += 1;
            PlayerPrefs.SetString(equipmentData.RarityID, currentRarity.ToString());





        }
        
        
    }
}
