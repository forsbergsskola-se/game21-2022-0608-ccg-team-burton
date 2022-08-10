using System;
using UnityEngine;

public class FusionManager : MonoBehaviour
{
    public void InitiateUpgrade(EquipmentSO equipmentData, ActionItem upgradeMaterial)
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString()) > PlayerPrefs.GetInt(upgradeMaterial.GetItemID()))
            return;
        
            Debug.Log("Upgrading!");
            
            CalculateNewMaterialBalance(upgradeMaterial);
            UpgradeRarity(equipmentData);
    }

    private void UpgradeRarity(EquipmentSO equipmentData)
    {
        var currentRarityString = PlayerPrefs.GetString(equipmentData.RarityID);
        Enum.TryParse(currentRarityString, out Rarity currentRarity);
        currentRarity += 1;
        PlayerPrefs.SetString(equipmentData.RarityID, currentRarity.ToString());
    }

    private void CalculateNewMaterialBalance(ActionItem upgradeMaterial)
    {
        var newBalance = PlayerPrefs.GetInt(upgradeMaterial.GetItemID()) - PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString());
        PlayerPrefs.SetInt(upgradeMaterial.GetItemID(), newBalance);
    }
}
