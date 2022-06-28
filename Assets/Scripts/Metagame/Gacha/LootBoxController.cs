using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Gacha;
using Mono.Cecil;
using Newtonsoft.Json;
using UnityEngine;

public class LootBoxController : MonoBehaviour, ISaveable
{
   public LootBoxSO LootBoxSO;
   private int _numberOfItemToSpawn = 1;
   [SerializeField] private Animator _animator;

   [SerializeField] private GameObject _itemInfoUI;

   public Action<Item> OnUpdateItemUI;
   public FMODUnity.EventReference musicTrack2;
   
   private Item item;
   SoundMananger _sound;

   void Awake(){
      _sound = FindObjectOfType<SoundMananger>();
   }

   private void Start()
   {
      _itemInfoUI.SetActive(false);
   }

   private void OnMouseDown()
   {
      Debug.Log("Opening loot box");
      OpenBox();
      _animator.SetBool("OpenLootBox", true);
      _sound.StartSound(musicTrack2);
   }

   
   public void OpenBox()
   {
         var itemSo = LootBoxSO.PickLootTable().PickItem();
        
         Debug.Log($"Saving: {itemSo.ID},{itemSo.RaritySo.ID},{itemSo.GemSo.ID}");
         
            
            
         
         //Hook in item generation to receive stats 
         //TODO: Get item with scaled stats here - these will be presented on the lootboxUI when opening item.
         item = ItemFactory.CreateItemFromInventory(itemSo, itemSo.RaritySo, itemSo.GemSo);
 
      

   }

   private void DisplayItem() // called by anim
   {
      _itemInfoUI.SetActive(true);
      OnUpdateItemUI?.Invoke(item);
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
