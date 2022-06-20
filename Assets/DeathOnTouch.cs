using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var playerHealth =col.gameObject.GetComponent<IDamageable>(); 
            playerHealth.ModifyHealth(-playerHealth.CurrentHealth);
            
        }
    }
}
