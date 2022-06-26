using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEditor.SearchService.Scene;

public class MainMenu : MonoBehaviour
{
    public void NewGame();
    public static bool GameIsPaused = false;
    public GameObject ResumeButton;
    
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
        }
            
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Resume ()
    {
    }
    void Pause ()
    {
        pause
    }
}
