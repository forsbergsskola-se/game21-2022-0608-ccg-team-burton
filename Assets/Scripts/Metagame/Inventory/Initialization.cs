using System;
using UnityEngine;

public class Initialization : MonoBehaviour
{

    [SerializeField] private Libraries equipmentLibrary;
    [SerializeField] private UICurrencyUpdater uICurrencyUpdater;

    private void Start()
    {
        if(!PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerDataCreated.ToString()))
            NewGamePlayerStats();
        
        //Update UI after init
        uICurrencyUpdater.OnCurrencyChanged?.Invoke();
    }
//TODO: Should be called on like main map for first time setup. On second start (player prefs present) --> nothing will happen here
    private void NewGamePlayerStats()
    {
        Debug.Log("Creating new data...");
        if(!PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentCoins.ToString()))
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), 0);
        
        if(!PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentButtons.ToString()))
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentButtons.ToString(), 0);
        
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
        
        PlayerPrefs.SetString(PlayerPrefsKeys.PlayerDataCreated.ToString(), "Data has been created");
    }
}
