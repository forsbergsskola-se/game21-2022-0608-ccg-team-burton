using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class Bullet_ML : MonoBehaviour
{
    public Vector2 travelVector;
    [HideInInspector] public float lifetime = 20;
    private float timeAlive;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<IDamageable>().ModifyHealth(-3);
            gameObject.SetActive(false);
        }

        if (col.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
        
        gameObject.SetActive(false);
    }

    void Update()
    {
        gameObject.transform.position += (Vector3) travelVector * Time.deltaTime;
        timeAlive += Time.deltaTime * 1;

        if (timeAlive >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}
