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
    [SerializeField] private TMP_Text RarityValue;
    
    
    // Start is called before the first frame update
    void OnEnable()
    {
        EquipmentIcon.GetComponent<Image>().sprite = EquipmentData.Icon;
        RarityValue.SetText(EquipmentData.Rarity.ToString());

    }
 
}
