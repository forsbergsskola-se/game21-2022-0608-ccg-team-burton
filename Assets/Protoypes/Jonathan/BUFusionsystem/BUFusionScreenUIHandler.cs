using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BUFusionScreenUIHandler : MonoBehaviour
{
    public BUEquipment EquipmentData;
    
    //UPDATE FIELDS
    [SerializeField] private GameObject EquipmentIcon;
    [SerializeField] private TMP_Text Rarity;
    [SerializeField] private TMP_Text AttributeText;
    
    

    void OnEnable()
    {
        EquipmentIcon.GetComponent<Image>().sprite = EquipmentData.Icon;
        Rarity.SetText("Rarity: "+EquipmentData.Rarity.ToString());
        AttributeText.SetText(EquipmentData.AttributeDescription+" " +EquipmentData.AttributeValue.ToString() );

    }
 
}
