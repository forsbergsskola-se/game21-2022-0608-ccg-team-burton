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


    
    private void OnEnable()
    {

        UpdateUpgradeUI();
    }

    private void UpdateUpgradeUI()
    {
        CalculateNeededMaterials();
        UpdateUIElements();  
    }


    public void CalculateNeededMaterials()
    {
        Enum.TryParse(EquipmentSoData.RarityID, out Rarity currentRarity);
        EquipmentSoData.NeededUpgradeMaterial = ((int)currentRarity + 1) +EquipmentSoData.BaseUpgradeCost;
    }
    private void UpdateUIElements()
    {
        equipmentIconGameObject.GetComponent<Image>().sprite = EquipmentSoData.Icon;
        rarityText.SetText("Rarity: "+EquipmentSoData.Rarity);
        attributeText.SetText(EquipmentSoData.AttributeDescription+" " +EquipmentSoData.AttributeValue);
        upgradeMaterialsIconGameObject.GetComponent<Image>().sprite = UpgradeMaterialSoData.GetIcon();
        haveMaterialText.SetText("Have: "+PlayerPrefs.GetInt(UpgradeMaterialSoData.GetItemID()));
        neededMaterialText.SetText($"Need: {EquipmentSoData.NeededUpgradeMaterial}");
    }

    public void PressUpgradeButton()
    {
            fusionManager.InitiateUpgrade(EquipmentSoData,UpgradeMaterialSoData);
            UpdateUpgradeUI();
            OnInventoryChange?.Invoke();
    }
}
