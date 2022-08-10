using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FusionScreenUIHandler : MonoBehaviour
{
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
        equipmentIconGameObject.GetComponent<Image>().sprite = EquipmentSoData.Icon;
        rarityText.SetText("Rarity: "+EquipmentSoData.Rarity);
        attributeText.SetText(EquipmentSoData.AttributeDescription+" " +EquipmentSoData.AttributeValue);
        upgradeMaterialsIconGameObject.GetComponent<Image>().sprite = UpgradeMaterialSoData.GetIcon();
        haveMaterialText.SetText("Have: "+PlayerPrefs.GetInt(UpgradeMaterialSoData.GetItemID()));

        Enum.TryParse(EquipmentSoData.RarityID, out Rarity currentRarity);
        EquipmentSoData.NeededUpgradeMaterial = ((int)currentRarity + 1) +EquipmentSoData.BaseUpgradeCost; //Current rarity +1 = addition cost.
        
        neededMaterialText.SetText($"Need: {EquipmentSoData.NeededUpgradeMaterial}");
    }

    public void PressUpgradeButton()
    {
            fusionManager.InitiateUpgrade(EquipmentSoData,UpgradeMaterialSoData);
        
    }
 
}
