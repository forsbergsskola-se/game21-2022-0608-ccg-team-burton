using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFailed : MonoBehaviour
{
    public GameObject DeathScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DeathScreen.SetActive(true);
            Time.timeScale = 0; 
        }
    }
}
