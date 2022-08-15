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
        Debug.Log($"start bullet: {timeAlive}");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var layer = col.transform.gameObject.layer;
        
        if (layer == 8)
        {
            col.gameObject.GetComponent<IDamageable>().ModifyHealth(-damageAmount);
            gameObject.SetActive(false);
        }
        else if( timeAlive < 1)
        {
            Physics2D.IgnoreCollision(col, transform.GetComponent<Collider2D>());
        }
    }

    void Update()
    {
        gameObject.transform.position += (Vector3) travelVector * (Time.deltaTime * moveSpeed);
        timeAlive += Time.deltaTime;

        if (timeAlive >= maxLifespan)
        {
            Debug.Log("remove bullet");
            gameObject.SetActive(false);
        }
    }
}
