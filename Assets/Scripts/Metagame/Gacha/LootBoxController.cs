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

         
         //TODO: Save items to player inventory - What to save? ItemSo only as long as it contains gems, names, etc? ID?
         //TODO: Have SO with SO+ID --> search for SO with help of ID
         //Save(itemSo.id, itemSo.itemRaritySo.Id, GemId....) //All strings or ints? (om inte loot med gem --> gem = null)
         
         //This represents equipping item (temporary placed here for testing)
         ItemPrefab.SetUpItemPrefab(itemSo);
       
      }

   }
}
