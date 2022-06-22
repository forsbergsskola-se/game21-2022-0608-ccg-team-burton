using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour{

    [SerializeField]public TextMeshProUGUI cointText;
    [SerializeField] int cointValue;
    [HideInInspector]public int _coinCounter = 0;

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Coin")){
            Destroy(col.gameObject);
            //TODO: Implement coin sound here
            _coinCounter +=  cointValue;
            UpdateCoinText(_coinCounter);
        }
    }

    public void UpdateCoinText(int value){
        cointText.text = "Coins: " + value ;
    }
}
