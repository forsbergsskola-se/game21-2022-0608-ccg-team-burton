using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Gacha;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

public class LootBoxController : MonoBehaviour
{
   public LootBoxSO LootBoxSO;
   private int _numberOfItemToSpawn = 1;
   [SerializeField] private Animator _animator;
   public GameObject ItemPrefabGameObject;
   private ItemPrefab itemPrefab;

   [SerializeField] private GameObject _itemInfoUI;

   public Action<Item> OnUpdateItemUI;
   private Item item;
   private void Start()
   {
      itemPrefab = ItemPrefabGameObject.GetComponent<ItemPrefab>();
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
      for (var i = 0; i < _numberOfItemToSpawn; i++)
      {
         
         
         //TODO: Get Item From lootbox, Contains RaritySO and GeemSO
         var itemSo = LootBoxSO.PickLootTable().PickItem();

        
         //TODO: Save items to player inventory: Save(itemSo.id, itemRaritySo.Id, GemSo.id) //All strings (designer friendly)
         Debug.Log($"Saving: {itemSo.ID},{itemSo.RaritySo.RarityID},{itemSo.GemSo.ID}");
         InventoryItemSerialization saveItem = new InventoryItemSerialization();
         saveItem.itemID = itemSo.ID;
         saveItem.rarityID = itemSo.RaritySo.RarityID;
         saveItem.gemId = itemSo.GemSo.ID;
         //Inventoryslot1 is gained when pressing an item slot in inventory
         PlayerPrefs.SetString("Inventoryslot1", JsonConvert.SerializeObject(saveItem));
         
         //Hok in 
         item = ItemFactory.CreateItemFromInventory(itemSo, itemSo.RaritySo, itemSo.GemSo);
         // var LoadSlot = PlayerPrefs.GetString("Inventoryslot1");
         //
         // Debug.Log(LoadSlot);




         //
         //
         // //TODO: Below is how an item would be constructed after loading SO:s via ID
         // //Not supposed to be here, but example on item factory building an item
         // Debug.Log($"Loading Entry (InventorySlot): {itemSo.ID},{itemSo.RaritySo.RarityID},{itemSo.GemSo.ID}" );
         // Debug.Log("Creating Item with ItemFactory"); 
         // itemPrefab.ItemSo = itemSo;
         // itemPrefab.RaritySo = itemSo.RaritySo;
         // itemPrefab.GemSo = itemSo.GemSo;
         // Instantiate(ItemPrefabGameObject, Vector3.zero, Quaternion.identity);


      }

   }

   private void DisplayItem()
   {
      _itemInfoUI.SetActive(true);
      OnUpdateItemUI?.Invoke(item);
   }
}
