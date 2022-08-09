using System;
using System.Collections;
using System.Collections.Generic;
using Protoypes.Harry;
using Unity.VisualScripting;
using UnityEngine;

public class disablePlayer : MonoBehaviour
{
    [SerializeField] NewMovement playerMovement;
    public float secondsToActivate;
    public GameObject player;
    
    private void Start()
    {
        playerMovement = player.GetComponent<NewMovement>();
        StartCoroutine(ActivationRoutine());
    }

    private IEnumerator ActivationRoutine()
    {
        //Wait for 14 secs.
        yield return new WaitForSeconds(secondsToActivate);
        playerMovement.enabled = true;
    }
}
