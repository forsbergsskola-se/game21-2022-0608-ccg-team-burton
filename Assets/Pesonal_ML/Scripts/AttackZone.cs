using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private BoxCollider2D owningCollider;
    public bool playerInZone { get; private set; }

    private void Awake()
    {
        owningCollider = GetComponent<BoxCollider2D>();
       // owningCollider.bounds = new Bounds()
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}
