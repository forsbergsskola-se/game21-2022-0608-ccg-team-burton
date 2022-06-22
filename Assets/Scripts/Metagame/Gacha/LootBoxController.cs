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
   
   public GameObject ItemPrefabGameObject;
   private ItemPrefab itemPrefab;

   private void Start()
   {
      itemPrefab = ItemPrefabGameObject.GetComponent<ItemPrefab>();
   }

   public void OpenBox()
   {
      for (var i = 0; i < _numberOfItemToSpawn; i++)
      {
         
         //Play animation
         
         
         var itemSo = LootBoxSO.PickLootTable().PickItem();
         
         
        
         //TODO: Save items to player inventory: Save(itemSo.id, itemRaritySo.Id, GemSo.id) //All strings (designer friendly)
         // Debug.Log($"Saving: {itemSo.ID},{itemSo.RaritySo.RarityID},{itemSo.GemSo.ID}");
         
         
         
         
         //Not supposed to be here, but example on item factory building an item

         itemPrefab.ItemSo = itemSo;
         itemPrefab.RaritySo = itemSo.RaritySo;
         itemPrefab.GemSo = itemSo.GemSo;
         Instantiate(ItemPrefabGameObject, Vector3.zero, Quaternion.identity);


      }

   }
}
