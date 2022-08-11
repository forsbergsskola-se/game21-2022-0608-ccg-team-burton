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
            var calculateNewAttribute = (int) GetCurrentRarity(equipmentData) * 5;
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, calculateNewAttribute);
            
        } 
        else if (equipmentData.ID.Contains("chest"))
        {
            var calculateNewAttribute =   (int) GetCurrentRarity(equipmentData);
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, calculateNewAttribute);
            
        } 
        else if (equipmentData.ID.Contains("legs"))
        {
            var calculateNewAttribute =(int) GetCurrentRarity(equipmentData) * 5;
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, calculateNewAttribute);
        } 
        else if (equipmentData.ID.Contains("weapon"))
        {
            Debug.Log(PlayerPrefs.GetInt(equipmentData.AttributeValueID));
            Debug.Log(GetCurrentRarity(equipmentData));
            var calculateNewAttribute = 10 + (int) GetCurrentRarity(equipmentData)*5;
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, calculateNewAttribute);
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
