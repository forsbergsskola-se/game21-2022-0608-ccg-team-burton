using UnityEngine;

public class PlayerInitialization : MonoBehaviour
{

    [SerializeField] private Libraries equipmentLibrary;

    private void Start()
    {
        SetupOrUpdatePLayerStats();
    }
//TODO: Should be called on like main map for first time setup. On second start (player prefs present) --> nothing will happen here
    private void SetupOrUpdatePLayerStats()
    {
        foreach (var equipment in equipmentLibrary.EquipmentLibrarySo.EquipablesLibrary)
        {
            if (!PlayerPrefs.HasKey(equipment.ID))
            {
                Debug.Log($"No entry for {equipment.Name}. Creating new entry with {equipment.Rarity}-rarity. This equipment affects {equipment.AttributeDescription} with a modifier of {equipment.AttributeValue}.");
                PlayerPrefs.SetString(equipment.ID, equipment.Name);
                PlayerPrefs.SetString(equipment.RarityID, equipment.Rarity.ToString());
                PlayerPrefs.SetFloat(equipment.AttributeValueID, equipment.AttributeValue);

            }
            else
            {
                Debug.Log($"{equipment.Name} is present with id: {equipment.ID}");
            }
        }
        
        PlayerPrefs.SetInt("CurrentCoins", 0);
        PlayerPrefs.SetInt("CurrentButtons", 0);
    }
}
