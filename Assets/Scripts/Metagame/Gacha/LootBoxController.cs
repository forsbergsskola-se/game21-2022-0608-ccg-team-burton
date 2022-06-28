using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Gacha;
using Mono.Cecil;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LootBoxController : MonoBehaviour, ISaveable
{
   public LootBoxSO LootBoxSO;

   private int _numberOfItemToSpawn;
   [SerializeField] private Animator _animator;
   [SerializeField] private GameObject _itemInfoUI;

   public Action<Item> OnUpdateItemUI;
 
   private Item item;
   public GameObject[] ItemElements;
   SoundMananger _sound;

   // Since we are looping, we can use a list
   private List<Item> gainedItems = new List<Item>();
   
   
   void Awake(){
      _sound = FindObjectOfType<SoundMananger>();
   }

   private void Start()
   {
      foreach (var o in ItemElements)
      {
         o.SetActive(false);
      }
      _itemInfoUI.SetActive(false);


      // Debug.Log(_numberOfItemToSpawn);
   }

   

   private void OnMouseDown()
   {
      Debug.Log("Opening loot box");
      OpenBox();
      _animator.SetBool("OpenLootBox", true);
      
   }

   
   public void OpenBox()
   {
      for (int i = 0; i < LootBoxSO.NumberOfItemsToSpawn; i++)
      {
         var itemSo = LootBoxSO.PickLootTable().PickItem();
        
         Debug.Log($"Saving: {itemSo.ID},{itemSo.RaritySo.ID},{itemSo.GemSo.ID}");
          
         //Hook in item generation to receive stats 
         //TODO: Get item with scaled stats here - these will be presented on the lootboxUI when opening item.
         item = ItemFactory.CreateItemFromInventory(itemSo, itemSo.RaritySo, itemSo.GemSo);
         gainedItems.Add(item);
         
      }
       
 
      

   }

   
   private void DisplayItem() // called by anim
   {
      int i = 0;
      _itemInfoUI.SetActive(true);

      foreach (var item in gainedItems)
      {
         ItemElements[i].SetActive(true);
         i++;
      }
      // OnUpdateItemUI?.Invoke(item);
   }


   public object CaptureState()
   {
      throw new NotImplementedException();
   }

   public void RestoreState(object state)
   {
      throw new NotImplementedException();
   }
}
