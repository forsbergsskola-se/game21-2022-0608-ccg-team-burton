using System;
using System.Collections.Generic;
using Meta.Gacha;
using UnityEngine;


public class LootBoxController : MonoBehaviour
{
   public LootBoxSO LootBoxSO;

   [SerializeField] private Animator _animator;
   [SerializeField] private GameObject _itemInfoUI;
   
   public Action<GameObject,InventoryItem> OnUpdateItemUI;
 
   public GameObject[] ItemUIGameobjects;

   // Since we need to update all elements, we can use a list
   private List<InventoryItem> gainedItems = new();

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

         //Saving item
         PlayerPrefs.SetInt(itemID, PlayerPrefs.GetInt(itemID)+1);

      }
      Handheld.Vibrate();
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
}
