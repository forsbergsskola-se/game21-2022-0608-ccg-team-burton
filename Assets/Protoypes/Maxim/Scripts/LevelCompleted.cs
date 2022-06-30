using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelCompleted : MonoBehaviour
{
    public GameObject WinScreen;
    public Image[] Stars;
    public Health PlayerHealth;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            WinScreen.SetActive(true);
            Time.timeScale = 0; 
            StarsAchieved();
        }
    }

    public void StarsAchieved()
    {
        int healthLeft = PlayerHealth.CurrentHealth;
        
       
        // float percentage = float.Parse(healthMax.ToString()) / float.Parse(healthLeft.ToString()) * 100f;

        if (healthLeft >= 2 && healthLeft < 4)
        {
            Stars[0].enabled = true;
        }
        else if (healthLeft >=4 && healthLeft < 6) 
        {
            Stars[0].enabled = true;
            Stars[1].enabled = true;
        }
        else 
        {
            Stars[0].enabled = true;
            Stars[1].enabled = true;
            Stars[2].enabled = true;
        }
        
    }
}
