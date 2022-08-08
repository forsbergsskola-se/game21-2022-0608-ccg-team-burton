using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inspiration From; https://www.youtube.com/watch?v=aUi9aijvpgs

[System.Serializable]
public class GameData
{
   public SavedItem SavedItem;

   //Default values, e.g if save not found --> start new game.
   public GameData()
   {
      this.SavedItem = new SavedItem();
      
   }
}

[Serializable]
public class SavedItem
{
   //TODO: This will be used to search sprite library in inventory scene or smth
   public int SpriteID;
}
