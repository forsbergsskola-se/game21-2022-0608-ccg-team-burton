using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{

    [SerializeField] private Libraries _library;
    [SerializeField] private UICurrencyUpdater uICurrencyUpdater;

    private void Start()
    {
        if(!PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerDataCreated.ToString()))
            NewGamePlayerStats();


        ResetGemModifiers();
        //Update UI after init
        uICurrencyUpdater.OnCurrencyChanged?.Invoke();
    }

    private void ResetGemModifiers()
    {
        foreach (var gem in _library.MatlerialsLibrarySo.Materials)
        {
            if (gem.LevelBonusID.Contains("upgradematerials"))
                continue;
            
            PlayerPrefs.SetFloat(gem.LevelBonusID, 0);
            Debug.Log($"GEM {gem.GetDisplayName()} with ID {gem.GetItemID()} and bonus id {gem.LevelBonusID} was reset to {PlayerPrefs.GetFloat(gem.LevelBonusID)}");
        }
        
        
    }

    //TODO: Should be called on like main map for first time setup. On second start (player prefs present) --> nothing will happen here
    private void NewGamePlayerStats()
    {
        Debug.Log("Creating new data...");
        if(!PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentCoins.ToString()))
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), 0);
        
        if(!PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentButtons.ToString()))
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentButtons.ToString(), 0);
        
        foreach (var equipment in _library.EquipmentLibrarySo.EquipablesLibrary)
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
