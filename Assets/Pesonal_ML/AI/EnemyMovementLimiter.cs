using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementLimiter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Enemy_AI>() == default) return;

        col.gameObject.GetComponentInChildren<Enemy_Eyes>().turnBack = true;
        Debug.Log("turn back");
    }


    private void Update()
    {
        
    }
}
