using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Gacha;
using Unity.VisualScripting;
using UnityEngine;

public class LootBoxController : MonoBehaviour
{
   public LootBoxSO LootBoxSO;

   private int numberOfItemToSpawn = 100;
   public void OpenBox()
   {
      for (int i = 0; i < numberOfItemToSpawn; i++)
      {
         // Debug.Log(LootBoxSO);
         var table = LootBoxSO.PickLootTable();
         // Debug.Log(table);
      
         Debug.Log(table.PickItem());
      }

   }
}
