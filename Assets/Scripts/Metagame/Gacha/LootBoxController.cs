using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Gacha;
using Newtonsoft.Json;
using UnityEngine;

public class LootBoxController : MonoBehaviour
{
   public LootBoxSO LootBoxSO;
   private int _numberOfItemToSpawn = 1;
   [SerializeField] private Animator _animator;

   [SerializeField] private GameObject _itemInfoUI;

   public Action<Item> OnUpdateItemUI;
   private Item item;
   private void Start()
   {
      _itemInfoUI.SetActive(false);
   }

   private void OnMouseDown()
   {
      Debug.Log("Opening loot box");
      OpenBox();
      _animator.SetBool("OpenLootBox", true);
      
   }

   
   public void OpenBox()
   {
         var itemSo = LootBoxSO.PickLootTable().PickItem();
        
         //TODO: Save items to player inventory: Save(itemSo.id, itemRaritySo.Id, GemSo.id) //All strings (designer friendly)
         Debug.Log($"Saving: {itemSo.ID},{itemSo.RaritySo.RarityID},{itemSo.GemSo.ID}");
         
         ////TEMP SAVE SYSTEM//////
         //TODO: Hook in save in save system
         InventoryItemSerialization saveItem = new InventoryItemSerialization();
         saveItem.itemID = itemSo.ID;
         saveItem.rarityID = itemSo.RaritySo.RarityID;
         saveItem.gemId = itemSo.GemSo.ID;
         //Inventoryslot1 is gained when pressing an item slot in inventory
         PlayerPrefs.SetString("Inventoryslot1", JsonConvert.SerializeObject(saveItem));
         
         
         //Hook in item generation to receive stats 
         //TODO: Get item with scaled stats here - these will be presented on the lootboxUI when opening item.
         item = ItemFactory.CreateItemFromInventory(itemSo, itemSo.RaritySo, itemSo.GemSo);
 
      

   }

   private void DisplayItem() // called by anim
   {
      _itemInfoUI.SetActive(true);
      OnUpdateItemUI?.Invoke(item);
   }
}
