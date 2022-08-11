using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionManager : MonoBehaviour
{
    [SerializeField] private PlayOneShotSound _soundOneShot;

    
    public void InitiateUpgrade(EquipmentSO equipmentData, MaterialItem upgradeMaterial)
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString()) > PlayerPrefs.GetInt(upgradeMaterial.GetItemID()))
            return;
        
            CalculateNewMaterialBalance(upgradeMaterial);
            UpgradeRarity(equipmentData);

            UpgradeAttribute(equipmentData);
            _soundOneShot.PlaySound();
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

    private void CalculateNewMaterialBalance(MaterialItem upgradeMaterial)
    {
        var newMaterialCount = PlayerPrefs.GetInt(upgradeMaterial.GetItemID()) - PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString());
        PlayerPrefs.SetInt(upgradeMaterial.GetItemID(), newMaterialCount);
    }
}
