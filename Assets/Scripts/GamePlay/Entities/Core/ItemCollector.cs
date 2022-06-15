using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour{

    int coins = 0;
    
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Coin")){
            Destroy(col.gameObject);
            coins++;
            Debug.Log("Coins: " + coins);
        }
    }
}
