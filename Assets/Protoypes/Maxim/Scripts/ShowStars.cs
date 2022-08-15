using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entity;


public class ShowStars : MonoBehaviour
{
    public Image[] Stars;
    public Health PlayerHealth;

    private void Start()
    {
        StarsGained();
    }

    private void StarsGained()
    {
        var healthLeft = PlayerHealth.CurrentHealth;

        if (healthLeft <= 2)
        {
            Stars[0].enabled = true;
        }
        else if (healthLeft <= 4)
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
