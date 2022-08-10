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
}
