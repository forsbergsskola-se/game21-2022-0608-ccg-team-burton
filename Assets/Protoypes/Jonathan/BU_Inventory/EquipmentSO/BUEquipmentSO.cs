using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "EquipmentData", menuName = "BUInventory/EquipmentData")]
public class BUEquipmentSO : ScriptableObject
{
    public string ID;
    
    public string Name;
    
    public string AttributeDescription;
    public string AttributeValueID;
    public float AttributeValue;
    public Sprite Icon;
    public string RarityID;
    public Rarity Rarity;
    public Sprite UpgradeMaterialIcon;
    public int testInt;

    private void OnValidate()
    {
        //Update values here?
    }
}


 