using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BUFusionScreenUIHandler : MonoBehaviour
{
    public BUEquipment EquipmentData;
    
    //UPDATE FIELDS
    public GameObject EquipmentIcon;
    
    
    // Start is called before the first frame update
    void OnEnable()
    {
        EquipmentIcon.GetComponent<Image>().sprite = EquipmentData.Icon;
    }
 
}
