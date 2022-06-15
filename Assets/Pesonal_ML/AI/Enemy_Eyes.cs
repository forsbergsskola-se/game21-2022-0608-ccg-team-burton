using System;
using UnityEngine;

public class Enemy_Eyes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<Enemy_AI>().PlayerSpotted();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Grounded"))
        {
            gameObject.GetComponentInParent<Enemy_AI>().GroundGone();
        }
    }
}
