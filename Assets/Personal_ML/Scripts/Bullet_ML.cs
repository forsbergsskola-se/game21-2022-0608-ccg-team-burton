using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet_ML : MonoBehaviour
{
    public Vector2 travelVector;
    public float timeAlive;
    public int damageAmount;
    public float moveSpeed;
    public float maxLifespan;

    private void Start()
    {
        timeAlive = 0;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var layer = col.transform.gameObject.layer;
   
        Debug.Log(col.collider.transform.name);
        if (layer == 8)
        {
            Debug.Log("player hit");
            col.gameObject.GetComponent<IDamageable>().ModifyHealth(-damageAmount);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("player hit");
        }
    }
    

     private void OnTriggerEnter2D(Collider2D col)
    {
        var layer = col.transform.gameObject.layer;
    
        Debug.Log("trigger something");
        if (layer == 8)
        {
            Debug.Log("player hit");
            col.gameObject.GetComponent<IDamageable>().ModifyHealth(-damageAmount);
            gameObject.SetActive(false);
        }
        else if(layer == 0)
        {
            Debug.Log("trigger something");
        }
    
        //else if(timeAlive < 1)
        //{
        //    //     Physics2D.IgnoreCollision(col, gameObject.GetComponent<Collider2D>());
        //}
    }

    void Update()
    {
        gameObject.transform.position += (Vector3) travelVector * (Time.deltaTime * moveSpeed);
        timeAlive += Time.deltaTime;

        if (timeAlive >= maxLifespan)
        {
            gameObject.SetActive(false);
        }
    }
}
