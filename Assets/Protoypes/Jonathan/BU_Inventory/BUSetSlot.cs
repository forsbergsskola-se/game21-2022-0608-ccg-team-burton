using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BUSetSlot : MonoBehaviour
{

    private Image spriteRenderer;
    [SerializeField] private TMP_Text amount;



    private void Awake()
    {
        spriteRenderer = GetComponent<Image>();
    }

    public void SetSlot(ActionItem item)
    {
        spriteRenderer.sprite = item.GetIcon();
        amount.SetText(PlayerPrefs.GetInt(item.GetItemID()).ToString());
    }




    private void OnMouseDown()
    {
        Debug.Log("MOUSE DOWN TRIGGERED!");
    }
}

