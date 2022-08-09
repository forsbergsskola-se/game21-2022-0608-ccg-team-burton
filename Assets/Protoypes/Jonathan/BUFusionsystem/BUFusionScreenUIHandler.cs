using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BUFusionScreenUIHandler : MonoBehaviour
{
    public BUEquipmentSO equipmentSoData;
    
    //UPDATE FIELDS
    [SerializeField] private GameObject EquipmentIcon;
    [SerializeField] private TMP_Text Rarity;
    [SerializeField] private TMP_Text AttributeText;
    [SerializeField] private GameObject UpgradeMaterialsIcon;
    [SerializeField] private TMP_Text NeededMaterialText;
    

    void OnEnable()
    {
        EquipmentIcon.GetComponent<Image>().sprite = equipmentSoData.Icon;
        Rarity.SetText("Rarity: "+equipmentSoData.Rarity.ToString());
        AttributeText.SetText(equipmentSoData.AttributeDescription+" " +equipmentSoData.AttributeValue.ToString() );
        UpgradeMaterialsIcon.GetComponent<Image>().sprite = equipmentSoData.UpgradeMaterialIcon;
        
    }
 
}
