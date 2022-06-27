using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenShop : MonoBehaviour
{
    public void LoadShop()
    {
        SceneManager.LoadScene("UI_Shop_Prototype");
    }
}
