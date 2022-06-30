using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelCompleted : MonoBehaviour
{
    public GameObject WinScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            WinScreen.SetActive(true);
            Time.timeScale = 0; 
        }
    }
}
