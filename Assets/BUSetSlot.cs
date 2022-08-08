using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BUSetSlot : MonoBehaviour
{

    private Image spriteRenderer;
    [SerializeField]
    private TMP_Text amount;

    
    
    private void Awake()
    {
        spriteRenderer = GetComponent<Image>();
        Debug.Log(spriteRenderer);
    }

    public void SetSlot(ActionItem item)
    {
        spriteRenderer.sprite = item.GetIcon();
        amount.SetText(PlayerPrefs.GetInt(item.GetItemID()).ToString()); 
    }
}
