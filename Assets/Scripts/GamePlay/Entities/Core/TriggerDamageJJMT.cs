using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;


//TODO: To implement on enemy attack
public class TriggerDamageJJMT : MonoBehaviour
{
    private int _damage = 1;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Entity"))
        {
         
            Debug.Log($"Deal damage by {name}");
            col.gameObject.GetComponent<IDamageableJJMT>().ModifyHealth(-_damage); // This is hopw you dewal damage to an IDamageable

        }
        
    }
    
}
