using UnityEngine;

public class ButtonCloseWindow : MonoBehaviour
{
    public void CloseWindowOnClick()
    {
        transform.root.gameObject.SetActive(false);
    }
}
