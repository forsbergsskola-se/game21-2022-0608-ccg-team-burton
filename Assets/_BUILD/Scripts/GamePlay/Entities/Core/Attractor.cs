using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Attractor : MonoBehaviour{
    [SerializeField] float attractorSpeed;

    Pickup pickup;

    private void Start()
    {
        pickup = GetComponent<Pickup>();
    }
    void OnTriggerStay2D(Collider2D other){
        if (other.CompareTag("Player")){
            transform.position = Vector3.MoveTowards(transform.position,other.transform.position, attractorSpeed * Time.deltaTime);
            pickup.PickupItem();
        }
    }

    void Update(){
        if (transform.childCount < 1){
            Destroy(gameObject);
        }
    }
}
