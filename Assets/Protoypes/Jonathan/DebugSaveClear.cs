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
}
