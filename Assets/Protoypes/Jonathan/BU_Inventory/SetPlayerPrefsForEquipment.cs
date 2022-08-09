using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPrefsForEquipment : MonoBehaviour
{

    [SerializeField] private Libraries equipmentLibrary;
    // Start is called before the first frame update
    void Start()
    {
        SetupOrUpdatePLayerStats();
    }

    public void SetupOrUpdatePLayerStats()
    {
        foreach (var equipment in equipmentLibrary.EquipmentLibrarySo.EquipablesLibrary)
        {
            if (PlayerPrefs.HasKey(equipment.ID))
            {
                Debug.Log($"Found Entry for {equipment.Name}. Current rarity is {equipment.Rarity}. This affects {equipment.AttributeDescription} with a modifier of {equipment.AttributeValue}.");
            }
            else
            {
                Debug.Log($"No entry for {equipment.Name}. Creating new entry with {equipment.Rarity}-rarity. This equipment affects {equipment.AttributeDescription} with a modifier of {equipment.AttributeValue}.");
                PlayerPrefs.SetString(equipment.ID, equipment.Name);
                PlayerPrefs.SetString(equipment.RarityID, equipment.Rarity.ToString());
                PlayerPrefs.SetFloat(equipment.AttributeValueID, equipment.AttributeValue);
                
            }
        }
    }
    
}
