using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BUEquipableSlot : MonoBehaviour, IPointerDownHandler
{

    public GameObject UpgradeScreen;
    public void OnPointerDown(PointerEventData eventData)
    {
        UpgradeScreen.SetActive(true);
    }
}
