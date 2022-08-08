using System;
using System.Collections.Generic;
using Meta.Gacha;
using UnityEngine;


public class LootBoxController : MonoBehaviour, IDataPersistance
{
   
   
   public LootBoxSO LootBoxSO;

   [SerializeField] private Animator _animator;
   [SerializeField] private GameObject _itemInfoUI;
   
   public Action<GameObject,InventoryItem> OnUpdateItemUI;
 
   private Item item;
   public GameObject[] ItemUIGameobjects;

   // Since we need to update all elements, we can use a list
   private List<InventoryItem> gainedItems = new();

   // [SerializeField] private PickupSpawner[] _pickup;

   private bool _boxOpenedCurrentSession;
   
   public GameObject DroppedItem;

   private string itemID;

   private void Start()
   {
      _boxOpenedCurrentSession = false;
      foreach (var itemElement in ItemUIGameobjects)
      {
         itemElement.SetActive(false);
      }
      _itemInfoUI.SetActive(false);
   }

   

   private void OnMouseUp()
   {
      if (!_boxOpenedCurrentSession)
      {
         Debug.Log("Opening loot box");
         OpenBox();
         _boxOpenedCurrentSession = true;
         _animator.SetBool("OpenLootBox", true); 
      }
      
      
   }
   
   
   public void OpenBox()
   {
      for (int i = 0; i < LootBoxSO.NumberOfItemsToSpawn; i++)
      {
         var LootedItemSO = LootBoxSO.PickLootTable().PickItem(); //Scriptable object
         gainedItems.Add(LootedItemSO);


         itemID = LootedItemSO.GetItemID();
         Debug.Log($"Saving item ID: {itemID}, with name: {LootedItemSO.GetDisplayName()}");
         
         
          /////////////////////////////////
         SetUpItemSO(LootedItemSO);
         
          //TODO: SAVE LOOTEDITEMSO TO INVENTORY HERE <3
          var item = Instantiate(DroppedItem, Vector2.zero, Quaternion.identity); // When instantiated, it Autocollects to inventory
    ////////////////////////////////////////

      }
   }

   public void SetUpItemSO(InventoryItem item) // Call on collect button if we dont save above in open box
   {

      DroppedItem.GetComponent<PickupSpawner>().Item = item;
      Debug.Log(item.name);

   }
   
   private void DisplayItem() // called by anim event
   {
      int i = 0;
      _itemInfoUI.SetActive(true);

      foreach (var item in gainedItems)
      {
         ItemUIGameobjects[i].SetActive(true);
         OnUpdateItemUI?.Invoke(ItemUIGameobjects[i],item);
   
         i++;
      }
   }

   public void LoadData(GameData data)
   {
      throw new NotImplementedException();
   }

   public void SaveData(GameData data)
   {
      data.SavedItem.itemID = this.itemID;
   }
}
