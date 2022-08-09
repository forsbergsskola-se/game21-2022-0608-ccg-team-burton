using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private CustomSceneManager _sceneManager;
    public void LoadScene()
    {
        //SceneManager.LoadScene(_sceneName);
        // Time.timeScale = 1;

        _sceneManager.DoCoroutine();
    }

    public void RestartSceen()
    {
        
    }
}
