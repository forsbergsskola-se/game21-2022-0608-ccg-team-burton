using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour{

    [SerializeField] TextMeshProUGUI cointText;
    
    int coins = 0;
    
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Coin")){
            Destroy(col.gameObject);
            coins++;
            cointText.text = "Coins: " + coins;
        }
    }
}
