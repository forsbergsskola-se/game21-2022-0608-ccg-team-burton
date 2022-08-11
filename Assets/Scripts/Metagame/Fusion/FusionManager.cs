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

            UpgradeAttribute(equipmentData);
    }

    private void UpgradeAttribute(EquipmentSO equipmentData)
    {
 
        if (equipmentData.ID.Contains("head"))
        {
            Debug.Log("Upgrade Head here");
        } else if (equipmentData.ID.Contains("chest"))
        {
            Debug.Log("Upgrade Chest here");
        } else if (equipmentData.ID.Contains("legs"))
        {
            Debug.Log("Upgrade Legs here");
        } else if (equipmentData.ID.Contains("weapon"))
        {
            Debug.Log("Upgrade Weapon here");
        }
    }

    private void UpgradeRarity(EquipmentSO equipmentData)
    {
        Debug.Log($"equipmentdata: {equipmentData.RarityID}");
        var currentRarityString = PlayerPrefs.GetString(equipmentData.RarityID);
        Enum.TryParse(currentRarityString, out Rarity currentRarity);
        currentRarity += 1;
        Debug.Log($"equipmentdata: {equipmentData.RarityID}");

        PlayerPrefs.SetString(equipmentData.RarityID, currentRarity.ToString());
    }

    private void CalculateNewMaterialBalance(ActionItem upgradeMaterial)
    {
        var newBalance = PlayerPrefs.GetInt(upgradeMaterial.GetItemID()) - PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString());
        PlayerPrefs.SetInt(upgradeMaterial.GetItemID(), newBalance);
    }
}
