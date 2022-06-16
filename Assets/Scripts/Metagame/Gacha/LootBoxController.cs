using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Gacha;
using Unity.VisualScripting;
using UnityEngine;

public class LootBoxController : MonoBehaviour
{
   public LootBoxSO LootBoxSO;
   private int _numberOfItemToSpawn = 1;

   public void OpenBox()
   {
      for (var i = 0; i < _numberOfItemToSpawn; i++)
      {
         
         //Play animation
         
         
         var item = LootBoxSO.PickLootTable().PickItem();

         //Save items to player inventory         
         
         Debug.Log("--------------------------------------------");
         Debug.Log(item.name);
         Debug.Log($"Rarity: {item.Rarity}");
         Debug.Log($"Base Attack Damage: {item.BaseAttackDamage}");
         Debug.Log($"AttackModifier: x{item.AttackDamageMultiplier}");
         Debug.Log($"Total Damage: : {item.TotalAttackDamage}");
         Debug.Log("--------------------------------------------");

      }

   }
}
