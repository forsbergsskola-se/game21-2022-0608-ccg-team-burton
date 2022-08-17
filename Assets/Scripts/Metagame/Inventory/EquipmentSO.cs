using FMOD.Studio;
using FMODUnity;
using UnityEngine;


[CreateAssetMenu(fileName = "EquipmentData", menuName = "BUInventory/EquipmentData")]
public class EquipmentSO : ScriptableObject
{
    [Header("GENERAL")]
    public string ID;
    public string Name;
    public Sprite[] Icon;
    [Header("ATTRIBUTE")]
    public string AttributeValueID;
    public string AttributeDescription;
    public float AttributeValue;
    [Header("RARITY")]
    public string RarityID;
    public Rarity Rarity;
    [Header("UPGRADE VARIABLES")]
    public int BaseUpgradeCost;
    public EventReference SoundFile;
    [Tooltip("The number increment/rarity level")]
    public int AttributeUpgradeStepSize;
}


 