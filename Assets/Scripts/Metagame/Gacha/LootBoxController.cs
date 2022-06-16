using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Gacha;
using Unity.VisualScripting;
using UnityEngine;

public class LootBoxController : MonoBehaviour
{
   public LootBoxSO LootBoxSO;



   private int numberOfItemToSpawn = 1;

   public void OpenBox()
   {
      for (int i = 0; i < numberOfItemToSpawn; i++)
      {
         // Debug.Log(LootBoxSO);
         var table = LootBoxSO.PickLootTable();
         // Debug.Log(table);
         var item = table.PickItem();

         Debug.Log(item +" with rarity: " +item.Rarity);
         Debug.Log(item.name);
         Debug.Log($"Rarity: {item.Rarity}");
         Debug.Log($"Base Attack Damage: {item.BaseAttackDamage}");
         Debug.Log($"AttackModifier: x{item.AttackDamageMultiplier}");
         Debug.Log($"Total Damage: : {item.TotalAttackDamage}");
         
      }

   }
}
