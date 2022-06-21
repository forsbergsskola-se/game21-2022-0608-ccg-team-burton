using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLocked : MonoBehaviour
{
    [SerializeField] private bool _locked;
    public Image LockImage;
    public GameObject[] Stars;

    private void Update()
    {
        UpdateLevelImage();
    }


    private void UpdateLevelImage()
    {
        if (!_locked)
        {
            LockImage.gameObject.SetActive(true);
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].gameObject.SetActive(false);
            }
        }
        else
        {
            LockImage.gameObject.SetActive(false);
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].gameObject.SetActive(true);
            }
        }
    }
}
