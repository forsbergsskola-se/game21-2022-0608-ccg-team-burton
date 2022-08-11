using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            var calculateNewAttribute = PlayerPrefs.GetInt(equipmentData.AttributeValueID) +
                                        (int) GetCurrentRarity(equipmentData) * 5;
            Debug.Log($"ID: {equipmentData.ID}");
            Debug.Log("Calculated: " +calculateNewAttribute);
            Debug.Log($"BEFORE: {PlayerPrefs.GetFloat(equipmentData.AttributeValueID, calculateNewAttribute)}");
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, calculateNewAttribute);
            Debug.Log($"After: {PlayerPrefs.GetFloat(equipmentData.AttributeValueID, calculateNewAttribute)}");
            
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
        var currentRarity = GetCurrentRarity(equipmentData);
        currentRarity += 1;

        PlayerPrefs.SetString(equipmentData.RarityID, currentRarity.ToString());
    }

    private Rarity GetCurrentRarity(EquipmentSO equipmentData)
    {
        var currentRarityString = PlayerPrefs.GetString(equipmentData.RarityID);
        Enum.TryParse(currentRarityString, out Rarity currentRarity);
        return currentRarity;
    }

    private void CalculateNewMaterialBalance(ActionItem upgradeMaterial)
    {
        var newBalance = PlayerPrefs.GetInt(upgradeMaterial.GetItemID()) - PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString());
        PlayerPrefs.SetInt(upgradeMaterial.GetItemID(), newBalance);
    }
}
