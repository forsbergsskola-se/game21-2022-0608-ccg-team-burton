using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FusionScreenUIHandler : MonoBehaviour
{
    public Action OnInventoryChange;
    [HideInInspector] public EquipmentSO EquipmentSoData;
    [HideInInspector] public MaterialItem UpgradeMaterialSoData;

    
    
    //PRIVATE VARIABLES
    [SerializeField] private FusionManager _fusionManager;
    
    //UI FIELDS
    [SerializeField] private GameObject _equipmentIconGameObject;
    [SerializeField] private TMP_Text _rarityText;
    [SerializeField] private TMP_Text _attributeText;
    [SerializeField] private GameObject _upgradeMaterialsIconGameObject;
    [SerializeField] private TMP_Text _neededMaterialText;
    [SerializeField] private TMP_Text _haveMaterialText;
    [SerializeField] private GameObject _upgradeButton;

    
    private void OnEnable()
    {
        UpdateUpgradeUI();
    }

    public void PressUpgradeButton()
    {
        _fusionManager.InitiateUpgrade(EquipmentSoData,UpgradeMaterialSoData);
        OnInventoryChange?.Invoke();
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
        var rarityIndex = 0;
        _equipmentIconGameObject.GetComponent<Image>().sprite = EquipmentSoData.Icon[rarityIndex];
        _rarityText.SetText("Rarity: "+PlayerPrefs.GetString(EquipmentSoData.RarityID));
        
        SetAttributeText();
        
        _upgradeMaterialsIconGameObject.GetComponent<Image>().sprite = UpgradeMaterialSoData.GetIcon();
        _haveMaterialText.SetText("Have: "+PlayerPrefs.GetInt(UpgradeMaterialSoData.GetItemID()));
        _neededMaterialText.SetText($"Need: {PlayerPrefs.GetInt(PlayerPrefsKeys.NeededUpgradeMaterial.ToString())}");
    }

    private void SetAttributeText()
    {
        if (EquipmentSoData.ID.Contains("legs") || EquipmentSoData.ID.Contains("head"))
        {
            _attributeText.SetText(EquipmentSoData.AttributeDescription + " " + PlayerPrefs.GetFloat(EquipmentSoData.AttributeValueID) + "%");
        }
        else
        {
            _attributeText.SetText(EquipmentSoData.AttributeDescription + " " + PlayerPrefs.GetFloat(EquipmentSoData.AttributeValueID));
        }
    }

    private void MaxLevelCheck()
    {
        if (PlayerPrefs.GetString(EquipmentSoData.RarityID).Contains("Legendary"))
        {
            _upgradeButton.SetActive(false);
            _neededMaterialText.SetText("Max level reached");
            return;
        }
        _upgradeButton.SetActive(true);
    }
}