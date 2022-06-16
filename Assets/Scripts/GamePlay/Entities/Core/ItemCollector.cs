using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour{

    [SerializeField] TextMeshProUGUI cointText;
    [SerializeField] int cointValue;
    int coins = 0;

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Coin")){
            Destroy(col.gameObject);
            coins +=  cointValue;
            cointText.text = "Coins: " + coins;
        }
    }
}
