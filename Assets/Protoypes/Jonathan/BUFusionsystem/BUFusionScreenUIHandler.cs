using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BUFusionScreenUIHandler : MonoBehaviour
{
    [HideInInspector] public BUEquipmentSO equipmentSoData;
    [HideInInspector] public ActionItem UpgradeMaterialSoData;
    
    
    //UPDATE FIELDS
    [SerializeField] private GameObject EquipmentIcon;
    [SerializeField] private TMP_Text Rarity;
    [SerializeField] private TMP_Text AttributeText;
    [SerializeField] public GameObject UpgradeMaterialsIcon;
    [SerializeField] private TMP_Text NeededMaterialText;
    [SerializeField] private TMP_Text HaveMaterialText;
    

    void OnEnable()
    {
        EquipmentIcon.GetComponent<Image>().sprite = equipmentSoData.Icon;
        Rarity.SetText("Rarity: "+equipmentSoData.Rarity);
        AttributeText.SetText(equipmentSoData.AttributeDescription+" " +equipmentSoData.AttributeValue);
        UpgradeMaterialsIcon.GetComponent<Image>().sprite = UpgradeMaterialSoData.GetIcon();
        HaveMaterialText.SetText("Have: "+PlayerPrefs.GetInt(UpgradeMaterialSoData.GetItemID()));

        Enum.TryParse(equipmentSoData.RarityID, out Entity.Items.Rarity currentRarity);

        var needed = ((int)currentRarity + 1) +1;
        
        NeededMaterialText.SetText($"Need: {needed}");
    }
 
}
