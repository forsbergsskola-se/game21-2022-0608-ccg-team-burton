using System;
using System.Collections.Generic;
using Meta.Gacha;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class LootBoxController : MonoBehaviour, IPointerClickHandler
{
   //PUBLIC FIELDS
   public LootBoxSO LootBoxSO;
   public Action<GameObject,InventoryItem> OnUpdateItemUI;
   public GameObject[] ItemUIGameobjects;
   
   //PRIVATE FIELDS
   [SerializeField] private Animator _animator;
   [SerializeField] private GameObject _itemInfoUI;
   private List<InventoryItem> _gainedItems = new();
   private bool _boxOpenedCurrentSession;
   private string _itemID;

   private void Start()
   {
      _boxOpenedCurrentSession = false;
      foreach (var itemElement in ItemUIGameobjects)
      {
         itemElement.SetActive(false);
      }
      _itemInfoUI.SetActive(false);
   }

   public void OnPointerClick(PointerEventData eventData)
   {
      if (_boxOpenedCurrentSession) 
         return;
      
      OpenBox();
      _boxOpenedCurrentSession = true;
      _animator.SetBool("OpenLootBox", true);
   }
   
   public void OpenBox()
   {
      for (int i = 0; i < LootBoxSO.NumberOfItemsToSpawn; i++)
      {
         var LootedItemSO = LootBoxSO.PickLootTable().PickItem(); //Scriptable object
         _gainedItems.Add(LootedItemSO);
         _itemID = LootedItemSO.GetItemID();

         //Saving item
         PlayerPrefs.SetInt(_itemID, PlayerPrefs.GetInt(_itemID)+1);
      }
      Handheld.Vibrate();
   }

   
   private void DisplayItem() // called by anim event
   {
      var i = 0;
      _itemInfoUI.SetActive(true);

      foreach (var item in _gainedItems)
      {
         ItemUIGameobjects[i].SetActive(true);
         OnUpdateItemUI?.Invoke(ItemUIGameobjects[i],item);
         i++;
      }
      
      
   }
}
