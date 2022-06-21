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

   public ItemPrefab ItemPrefab;
   
   public void OpenBox()
   {
      for (var i = 0; i < _numberOfItemToSpawn; i++)
      {
         
         //Play animation
         
         
         var itemSo = LootBoxSO.PickLootTable().PickItem();

         
         
         //TODO: Save items to player inventory 
         
         
         //This represents equipping item
         ItemPrefab.SetItemPrefab(itemSo);
         
         
         
         
         
         
         
         // var createdItem = ItemFactory.CreateItemFromInventory(itemSo);
         //
         // if (createdItem is Weapon weapon)
         // {
         //    Debug.Log(weapon.ItemName);
         //    Debug.Log(weapon.Rarity);
         //    Debug.Log(weapon.WeaponDamage);
         //    return;
         // }
         //
         // if (createdItem is Armor armor)
         // {
         //    Debug.Log(armor.ItemName);
         //    Debug.Log(armor.Rarity);
         //    Debug.Log(armor.EffectValue);
         //    return;
         // }

         // Debug.Log($"Not weapon: {createdItem.ItemName}");
         // Debug.Log("--------------------------------------------");
         // Debug.Log(item.name);
         // Debug.Log($"Rarity: {item.Rarity}");
         // Debug.Log($"Base Attack Damage: {item.BaseAttackDamage}");
         // Debug.Log($"AttackModifier: x{item.AttackDamageMultiplier}");
         // Debug.Log($"Total Damage: : {item.TotalAttackDamage}");
         // Debug.Log("--------------------------------------------");

      }

   }
}
