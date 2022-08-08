using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] GameEvent onDeath;

    bool dead;
    void OnMouseDown()
    {
        if (dead == false)
        {
            Die();
        }
    }

    
    void Die()
    {
        onDeath?.Invoke();
        dead = true;
    }
}
