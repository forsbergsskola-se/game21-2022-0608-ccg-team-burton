using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftButtonOG : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    bool IsPressed = false;
    public GameObject player;

    private void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
    }
}
