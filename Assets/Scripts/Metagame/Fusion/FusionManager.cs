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
            //TODO: IMplement when sound is working
            // _soundOneShot.PlaySound();
    }

    private void UpgradeAttribute(EquipmentSO equipmentData)
    {
 
        if (equipmentData.ID.Contains("head"))
        {
            var newAttributeValue = (int) GetCurrentRarity(equipmentData) * equipmentData.AttributeUpgradeStepSize;
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, newAttributeValue);
            
        } 
        else if (equipmentData.ID.Contains("chest"))
        {
            var newAttributeValue =   (int) GetCurrentRarity(equipmentData)*equipmentData.AttributeUpgradeStepSize;
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, newAttributeValue);
            
        } 
        else if (equipmentData.ID.Contains("legs"))
        {
            var newAttributeValue =(int) GetCurrentRarity(equipmentData) * equipmentData.AttributeUpgradeStepSize;
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, newAttributeValue);
        } 
        else if (equipmentData.ID.Contains("weapon"))
        {
            var newAttributeValue = 10 + (int) GetCurrentRarity(equipmentData)*equipmentData.AttributeUpgradeStepSize;
            PlayerPrefs.SetFloat(equipmentData.AttributeValueID, newAttributeValue);
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
