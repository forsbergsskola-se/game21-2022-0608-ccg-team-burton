using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    public int health;
    
    public Health playerHealth;
    
    public Image[] hearts;
    

    private void Update()
    {
        health = playerHealth.CurrentHealth;
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
