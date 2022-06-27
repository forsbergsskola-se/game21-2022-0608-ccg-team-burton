using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    void LoadScene()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
