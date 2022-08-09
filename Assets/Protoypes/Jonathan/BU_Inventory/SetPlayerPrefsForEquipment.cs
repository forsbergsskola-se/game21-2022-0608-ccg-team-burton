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
                Debug.Log($"Found Entry for {PlayerPrefs.GetString(equipment.ID)}. Current rarity is {PlayerPrefs.GetString(equipment.RarityID)}. This affects {equipment.AttributeDescription} with a modifier of {PlayerPrefs.GetFloat(equipment.AttributeValueID)}.");
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
