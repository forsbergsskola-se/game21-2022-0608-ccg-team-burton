using System;
using UnityEngine;

public class Enemy_Eyes : MonoBehaviour
{
    public bool PlayerSeen { get; private set; }
    public bool GroundSeen { get; set; }
    public float Height { get; private set; }

    public bool turnBack;
    
    public Transform PlayerTrans { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerSeen = true;
            PlayerTrans = other.gameObject.transform;
        }
        
        if (other.gameObject.CompareTag("Grounded"))
        {
            Height = other.gameObject.transform.position.y;
            Debug.Log("Ground seen again");
            GroundSeen = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerSeen = false;
        }
        
        if (other.gameObject.CompareTag("Grounded"))
        {
            GroundSeen = false;
        }
    }
    
    
}
