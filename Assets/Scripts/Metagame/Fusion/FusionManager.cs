using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{


    public Action OnUiUpdate;
    public void InitiateUpgrade(EquipmentSO equipmentData, ActionItem upgradeMaterial)
    {
        if (equipmentData.NeededUpgradeMaterial > PlayerPrefs.GetInt(upgradeMaterial.GetItemID()))
        {
            Debug.Log("Not enough materials!");
        }
        else
        {
            Debug.Log("Upgrading!");
            var newBalance =PlayerPrefs.GetInt(upgradeMaterial.GetItemID()) - equipmentData.NeededUpgradeMaterial;
            PlayerPrefs.SetInt(upgradeMaterial.GetItemID(), newBalance);
            //EVEnt to update UI on fusionUI?
        }
        
        
    }
}
