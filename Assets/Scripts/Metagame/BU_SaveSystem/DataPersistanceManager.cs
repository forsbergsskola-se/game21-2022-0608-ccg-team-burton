using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using System.Linq;


//TODO: Singleton - fix?
public class DataPersistanceManager : MonoBehaviour
{
   public static DataPersistanceManager Instance { get; private set; }

   private List<IDataPersistance> dataPersistanceObjects;

   private GameData gameData;

   private void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("More than 1 DataPersistenceManager in scene");
      }

      Instance = this;
   }

   private void Start()
   {
      this.dataPersistanceObjects = FindAllDataPersistanceObjects();
      LoadGame();
   }

   private List<IDataPersistance> FindAllDataPersistanceObjects()
   {
      IEnumerable<IDataPersistance> dataPersistances = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

      return new List<IDataPersistance>(dataPersistances);

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
      foreach (var dataPersistanceObject in dataPersistanceObjects)
      {
         dataPersistanceObject.LoadData(gameData);
      }
   }

   public void SaveGame()
   {
      //TODO: Pass data to scripts to update them
      foreach (var dataPersistanceObject in dataPersistanceObjects)
      {
         dataPersistanceObject.SaveData(gameData);
      }
      //Todo: Save data to file via data handler
   }

   private void OnApplicationQuit()
   {
      SaveGame();
   }
}
