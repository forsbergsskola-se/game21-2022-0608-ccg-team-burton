using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FusionScreenUIHandler : MonoBehaviour
{
    public Action OnInventoryChange;
    [HideInInspector] public EquipmentSO EquipmentSoData;
    [HideInInspector] public ActionItem UpgradeMaterialSoData;

    [SerializeField] private FusionManager fusionManager;
    
    //UI FIELDS
    [SerializeField] private GameObject equipmentIconGameObject;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text attributeText;
    [SerializeField] private GameObject upgradeMaterialsIconGameObject;
    [SerializeField] private TMP_Text neededMaterialText;
    [SerializeField] private TMP_Text haveMaterialText;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private PlayOneShotSound sound;

    
    private void OnEnable()
    {
        UpdateUpgradeUI();
    }

    private void UpdateUpgradeUI()
    {
        CalculateNeededMaterials();
        UpdateUIElements();
        MaxLevelCheck();
    }

    private void CalculateNeededMaterials()
    {
        var currentRarity = PlayerPrefs.GetString(EquipmentSoData.RarityID);
        Enum.TryParse(currentRarity, out Rarity rarity);
        PlayerPrefs.SetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString(), (int) (rarity+1+EquipmentSoData.BaseUpgradeCost));
        
    }
    private void UpdateUIElements()
    {
        equipmentIconGameObject.GetComponent<Image>().sprite = EquipmentSoData.Icon;
        rarityText.SetText("Rarity: "+PlayerPrefs.GetString(EquipmentSoData.RarityID));
        
        SetAttributeText();
        
        upgradeMaterialsIconGameObject.GetComponent<Image>().sprite = UpgradeMaterialSoData.GetIcon();
        haveMaterialText.SetText("Have: "+PlayerPrefs.GetInt(UpgradeMaterialSoData.GetItemID()));
        neededMaterialText.SetText($"Need: {PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString())}");
    }

    private void SetAttributeText()
    {
        //HAAAACKYYYYYYYYY
        if (EquipmentSoData.ID.Contains("legs") || EquipmentSoData.ID.Contains("head"))
        {
            attributeText.SetText(EquipmentSoData.AttributeDescription + " " +
                                  PlayerPrefs.GetFloat(EquipmentSoData.AttributeValueID) + "%");
        }
        else
        {
            attributeText.SetText(EquipmentSoData.AttributeDescription + " " +
                                  PlayerPrefs.GetFloat(EquipmentSoData.AttributeValueID));
        }
    }

    public void PressUpgradeButton()
    {
            fusionManager.InitiateUpgrade(EquipmentSoData,UpgradeMaterialSoData);
            //play sound here
            sound.PlaySound();
            OnInventoryChange?.Invoke();
            UpdateUpgradeUI();

    }

    public void MaxLevelCheck()
    {
        if (PlayerPrefs.GetString(EquipmentSoData.RarityID).Contains("Legendary"))
        {
            upgradeButton.SetActive(false);
            neededMaterialText.SetText("Max level reached");
            return;
        }
        upgradeButton.SetActive(true);
    }
}