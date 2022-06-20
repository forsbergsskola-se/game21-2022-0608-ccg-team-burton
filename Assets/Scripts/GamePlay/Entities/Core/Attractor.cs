using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Attractor : MonoBehaviour{
    [SerializeField] float attractorSpeed;

    void OnTriggerStay2D(Collider2D other){
        if (other.CompareTag("Player")){
            transform.position = Vector3.MoveTowards(transform.position,other.transform.position, attractorSpeed * Time.deltaTime);
        }
    }

    void Update(){
        if (transform.childCount < 1){
            Destroy(gameObject);
        }
    }
}
