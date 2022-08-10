using System;
using UnityEngine;

public class Initialization : MonoBehaviour
{

    public Action OnInitComplete;

    [SerializeField] private Libraries equipmentLibrary;


    private void Start()
    {
        NewGamePlayerStats();
        
        //Update UI after init
        OnInitComplete?.Invoke();
    }
//TODO: Should be called on like main map for first time setup. On second start (player prefs present) --> nothing will happen here
    private void NewGamePlayerStats()
    {
        if(!PlayerPrefs.HasKey("CurrentCoins"))
            PlayerPrefs.SetInt("CurrentCoins", 0);
        
        if(!PlayerPrefs.HasKey("CurrentButtons"))
            PlayerPrefs.SetInt("CurrentButtons", 0);
        
        foreach (var equipment in equipmentLibrary.EquipmentLibrarySo.EquipablesLibrary)
        {
            if (!PlayerPrefs.HasKey(equipment.ID))
            {
                Debug.Log($"No entry for {equipment.Name}. Creating new entry with {equipment.Rarity}-rarity. This equipment affects {equipment.AttributeDescription} with a modifier of {equipment.AttributeValue}.");
                PlayerPrefs.SetString(equipment.ID, equipment.Name);
                PlayerPrefs.SetString(equipment.RarityID, equipment.Rarity.ToString());
                PlayerPrefs.SetFloat(equipment.AttributeValueID, equipment.AttributeValue);

            }
        }
    }
}
