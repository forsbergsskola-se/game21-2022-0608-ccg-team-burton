using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;


//TODO: Singleton - fix?
public class DataPersistanceManager : MonoBehaviour
{
   public static DataPersistanceManager Instance { get; private set; }

   private GameData gameData;

   private void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("More than 1 DataPersistanceManager in scene");
      }

      Instance = this;
   }

   private void Start()
   {
      LoadGame();
   }

   public void NewGame()
   {
      this.gameData = new GameData();
   }

   public void LoadGame()
   {
      //TODO: Load any saved data
      //If no data --> new game
      if (gameData == null)
      {
         Debug.Log("No data found --> new game");
         NewGame();
      }
      
      //TODO: Push loaded data to game scripts
      
   }

   public void SaveGame()
   {
      //TODO: Pass data to scripts to update them
      
      //Todo: Save data to file via data handler
   }

   private void OnApplicationQuit()
   {
      SaveGame();
   }
}
