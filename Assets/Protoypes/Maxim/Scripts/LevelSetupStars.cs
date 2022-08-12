using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSetupStars : MonoBehaviour
{
   public Image[] FullStars;
   private int achivedStars = 0;
   public int levelIndex;

   private void Update()
   {
      UpdateStars();
   }

   

   public void UpdateStars()
   {
      achivedStars = PlayerPrefs.GetInt("Lv" + levelIndex);

      if (achivedStars == 1)
      {
         FullStars[0].enabled = true;
      }

      else if (achivedStars == 2)
      {
         FullStars[0].enabled = true;
         FullStars[1].enabled = true;
      }

      else if (achivedStars == 3)
      {
         FullStars[0].enabled = true;
         FullStars[1].enabled = true;
         FullStars[2].enabled = true;
      }
   }
}
