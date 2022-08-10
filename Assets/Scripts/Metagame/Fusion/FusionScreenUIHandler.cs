using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FusionScreenUIHandler : MonoBehaviour
{
    [HideInInspector] public EquipmentSO equipmentSoData;
    [HideInInspector] public ActionItem UpgradeMaterialSoData;
    
    
    //UI FIELDS
    [SerializeField] private GameObject equipmentIconGameObject;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text attributeText;
    [SerializeField] private GameObject upgradeMaterialsIconGameObject;
    [SerializeField] private TMP_Text neededMaterialText;
    [SerializeField] private TMP_Text haveMaterialText;


    private void OnEnable()
    {
        equipmentIconGameObject.GetComponent<Image>().sprite = equipmentSoData.Icon;
        rarityText.SetText("Rarity: "+equipmentSoData.Rarity);
        attributeText.SetText(equipmentSoData.AttributeDescription+" " +equipmentSoData.AttributeValue);
        upgradeMaterialsIconGameObject.GetComponent<Image>().sprite = UpgradeMaterialSoData.GetIcon();
        haveMaterialText.SetText("Have: "+PlayerPrefs.GetInt(UpgradeMaterialSoData.GetItemID()));

        Enum.TryParse(equipmentSoData.RarityID, out Rarity currentRarity);
        var neededMaterials = ((int)currentRarity + 1) +equipmentSoData.BaseUpgradeCost; //Current rarity +1 = addition cost.
        
        neededMaterialText.SetText($"Need: {neededMaterials}");
    }
 
}
