using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Gacha;
using Mono.Cecil;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LootBoxController : MonoBehaviour
{
   
   
   public LootBoxSO LootBoxSO;

   [SerializeField] private Animator _animator;
   [SerializeField] private GameObject _itemInfoUI;

   public Action<GameObject,InventoryItem> OnUpdateItemUI;
 
   private Item item;
   public GameObject[] ItemUIGameobjects;
   private SoundMananger _sound;

   // Since we are looping, we can use a list
   private List<InventoryItem> gainedItems = new();

   [SerializeField] private PickupSpawner[] _pickup;
   
   
   void Awake(){
      _sound = FindObjectOfType<SoundMananger>();
   }

   private void Start()
   {
      foreach (var itemElement in ItemUIGameobjects)
      {
         itemElement.SetActive(false);
      }
      _itemInfoUI.SetActive(false);
   }

   

   private void OnMouseUp()
   {
      Debug.Log("Opening loot box");
      OpenBox();
      _animator.SetBool("OpenLootBox", true);
      
   }

   
   public void OpenBox()
   {
      for (int i = 0; i < LootBoxSO.NumberOfItemsToSpawn; i++)
      {
         var LootedItemSO = LootBoxSO.PickLootTable().PickItem(); //Scriptable object
         gainedItems.Add(LootedItemSO);
         
          //TODO: SAVE LOOTEDITEMSO TO INVENTORY
      }
   }

   public void CollectItems() // Call on button
   {

   
      
      
   }
   
   private void DisplayItem() // called by anim
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
