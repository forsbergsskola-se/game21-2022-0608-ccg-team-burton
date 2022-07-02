using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupOnSpawn : MonoBehaviour
{
     

    // Start is called before the first frame update
    void Start()
    {
     GetComponentInChildren<Pickup>().PickupItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
