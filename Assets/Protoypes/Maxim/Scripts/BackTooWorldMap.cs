using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTooWorldMap : MonoBehaviour
{
    public void LoadWorldMap()
    {
        SceneManager.LoadScene("UI_WorldMap_Prototype");
    }
}
