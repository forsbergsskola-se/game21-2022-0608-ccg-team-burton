using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class LevelFailed : MonoBehaviour
{
    [SerializeField] GameEvent playerDies;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            var playerHealth =collision.gameObject.GetComponent<IDamageable>(); 
            playerHealth.ModifyHealth(-playerHealth.CurrentHealth);
            Destroy(collision.gameObject);
            
            playerDies.Invoke();
        }
    }
}
