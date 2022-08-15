using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet_ML : MonoBehaviour
{
    public Vector2 travelVector;
    [HideInInspector] public float lifetime = 10;
    private float _timeAlive;
    public int damageAmount;
    public float moveSpeed;

    private void Start()
    {
        _timeAlive = 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var layer = col.transform.gameObject.layer;
        
        if (layer == 8)
        {
            col.gameObject.GetComponent<IDamageable>().ModifyHealth(-damageAmount);
            gameObject.SetActive(false);
        }
        else if( _timeAlive < 1)
        {
            Physics2D.IgnoreCollision(col, transform.GetComponent<Collider2D>());
        }
        else if(layer is 6 or 10)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        gameObject.transform.position += (Vector3) travelVector * (Time.deltaTime * moveSpeed);
        _timeAlive += Time.deltaTime;

        if (_timeAlive >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}
