using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveClear : MonoBehaviour
{
   public Libraries Library;
   
   public void ClearInventoryFromItems()
   {
      foreach (var item in Library.MatlerialsLibrarySo.InventoryLibrary)
      {
         PlayerPrefs.DeleteKey(item.GetItemID());
         Debug.Log(item.GetItemID() +" was deleted!");
      }
   }


   public void AddCoins()
   {
      PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString())+250);
      Debug.Log($"Current Coins: {PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString())}");
   }
   
   public void RemoveCoins()
   {
      PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString())-250);
      Debug.Log($"Current Coins: {PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString())}");

   }
   
   public void AddButtons()
   {
      PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentButtons.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentButtons.ToString())+25);
      Debug.Log($"Current Buttons: {PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentButtons.ToString())}");
   }
   
   public void RemoveButtons()
   {
      PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentButtons.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentButtons.ToString())-25);
      Debug.Log($"Current Buttons: {PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentButtons.ToString())}");

   }

   public void AddMetal()
   {
      PlayerPrefs.SetInt("upgradematerials.metal", PlayerPrefs.GetInt("upgradematerials.metal")+1);
      Debug.Log($"Current Metal: {PlayerPrefs.GetInt("upgradematerials.metal")}");
   }
   
   public void RemoveMetal()
   {
      PlayerPrefs.SetInt("upgradematerials.metal", PlayerPrefs.GetInt("upgradematerials.metal")-1);
      Debug.Log($"Current Metal: {PlayerPrefs.GetInt("upgradematerials.metal")}");
   }
   
   public void AddPorcelain()
   {
      PlayerPrefs.SetInt("upgradematerials.porcelain", PlayerPrefs.GetInt("upgradematerials.porcelain")+1);
      Debug.Log($"Current Porcelain: {PlayerPrefs.GetInt("upgradematerials.porcelain")}");
   }
   
   public void RemovePorcelain()
   {
      PlayerPrefs.SetInt("upgradematerials.porcelain", PlayerPrefs.GetInt("upgradematerials.porcelain")-1);
      Debug.Log($"Current Porcelain: {PlayerPrefs.GetInt("upgradematerials.porcelain")}");
   }

   public void AddGems()
   {
      foreach (var materialItem in Library.MatlerialsLibrarySo.InventoryLibrary)
      {
         if (materialItem.GetItemID().Contains("red") || materialItem.GetItemID().Contains("blue") ||
             materialItem.GetItemID().Contains("green"))
         {
            PlayerPrefs.SetInt(materialItem.GetItemID(), PlayerPrefs.GetInt(materialItem.GetItemID())+10);
            Debug.Log($"Current {materialItem.GetDisplayName()}: {PlayerPrefs.GetInt(materialItem.GetItemID())}"); 
         }
      }
   }
   
   public void ResetEquipment()
   {

      foreach (var equipmentSo in Library.EquipmentLibrarySo.EquipablesLibrary)
      {
         PlayerPrefs.DeleteKey(equipmentSo.ID);
         Debug.Log(equipmentSo.ID+" was deleted");
      }
      Debug.Log("Creating new data...");
      if(!PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentCoins.ToString()))
         PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), 0);
        
      if(!PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentButtons.ToString()))
         PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentButtons.ToString(), 0);
        
      foreach (var equipment in Library.EquipmentLibrarySo.EquipablesLibrary)
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

   public void SetupTestPlayer()
   {
      ClearInventoryFromItems();
      ResetEquipment();
      for (int i = 0; i < 100; i++)
      {
         AddPorcelain();
         AddMetal();
         AddCoins();
         AddButtons();
         AddGems();
      }
   }
}
