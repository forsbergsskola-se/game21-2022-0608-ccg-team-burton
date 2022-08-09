using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindow : MonoBehaviour
{
    public void CloseWindowOnClick()
    {
        transform.root.gameObject.SetActive(false);
    }
}
