using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelCompleted : MonoBehaviour
{
    public GameObject LevelComplete;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LevelComplete.SetActive(true);
            Time.timeScale = 0; 
        }
    }
}
