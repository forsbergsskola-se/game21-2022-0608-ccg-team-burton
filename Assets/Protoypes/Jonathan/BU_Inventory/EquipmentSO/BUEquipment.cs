using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "EquipmentData", menuName = "BUInventory/EquipmentData")]
public class BUEquipment : ScriptableObject
{
    public string ID;
    
    public string Name;
    public string AttributeDescription;
    public float AttributeValue;
    public Sprite Icon;

    public Rarity Rarity;
    
    public int testInt;

}


 