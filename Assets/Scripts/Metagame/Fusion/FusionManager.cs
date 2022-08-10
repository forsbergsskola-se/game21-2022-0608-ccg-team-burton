using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{

    public void InitiateUpgrade(EquipmentSO equipmentData, ActionItem upgradeMaterial)
    {
        if (equipmentData.NeededUpgradeMaterial > PlayerPrefs.GetInt(upgradeMaterial.GetItemID()))
        {
            Debug.Log("Not enough materials!");
        }
        else
        {
            Debug.Log("Can upgrade!");
        }
    }
}
