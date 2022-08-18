using System;
using Entity;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    
    public Health playerHealth;
    public Image[] hearts;

    void OnEnable() => playerHealth.OnHealthChanged += UpdateHealthUI;
    private void OnDisable() => playerHealth.OnHealthChanged -= UpdateHealthUI;

    private void UpdateHealthUI(int health)
    {
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