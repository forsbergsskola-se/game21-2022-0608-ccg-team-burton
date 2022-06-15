using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool PlayerSpotted = false;
    
    private void Start()
    {
   
    }

    private void OnDisable()
    {
      
    }

    private void PlayerDies()
    {
        PlayerSpotted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerSpotted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerSpotted = false;
        }
    }

}
