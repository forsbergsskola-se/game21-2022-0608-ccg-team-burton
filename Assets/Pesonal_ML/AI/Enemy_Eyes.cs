using System;
using UnityEngine;

public class Enemy_Eyes : MonoBehaviour
{
    public bool PlayerSeen { get; private set; }
    public bool GroundSeen { get; private set; }
    public Vector3 PlayerPos { get; private set; }
    private bool checkPlayerPos;
    public Transform PlayerTrans { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerSeen = true;
            checkPlayerPos = true;
            PlayerTrans = other.gameObject.transform;
        }
        
        if (other.gameObject.CompareTag("Grounded"))
        {
            PlayerSeen = true;
            checkPlayerPos = true;
            PlayerTrans = other.gameObject.transform;
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
            gameObject.GetComponentInParent<Enemy_AI>().GroundGone();
            GroundSeen = false;
        }
    }
    
    
}
