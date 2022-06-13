using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;


//TODO: To implement on enemy attack
public class TriggerDamageJJMT : MonoBehaviour
{
    private int _damage = 1;
    private void OnTriggerEnter2D(Collider2D col)
    {
            Debug.Log("Deal damage");
            col.GetComponent<IDamageableJJMT>().ModifyHealth(-_damage);
    }
}
