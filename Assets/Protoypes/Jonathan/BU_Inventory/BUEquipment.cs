using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "EquipmentData", menuName = "BUInventory/EquipmentData")]
public class BUEquipment : ScriptableObject
{
    public string ID;
    
    public string Name;
    
    public Sprite Icon;

    public string Rarity ="Common";

    public int testInt;

}
