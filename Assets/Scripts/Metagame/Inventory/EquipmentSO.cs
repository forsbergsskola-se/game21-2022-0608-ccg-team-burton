using UnityEngine;


[CreateAssetMenu(fileName = "EquipmentData", menuName = "BUInventory/EquipmentData")]
public class EquipmentSO : ScriptableObject
{
    public string ID;
    
    public string Name;
    
    public string AttributeDescription;
    public string AttributeValueID;
    public float AttributeValue;
    public Sprite Icon;
    public string RarityID;
    public Rarity Rarity;
    public int BaseUpgradeCost;
    public int NeededUpgradeMaterial;

    private void OnValidate()
    {
        //Update values here?
    }
}


 