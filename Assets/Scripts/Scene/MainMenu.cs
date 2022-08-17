using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}