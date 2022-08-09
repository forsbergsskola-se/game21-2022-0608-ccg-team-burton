using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BUEquipableSlot : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private BUEquipment equipment;
    [SerializeField] private GameObject UpgradeScreen;
    public void OnPointerDown(PointerEventData eventData)
    {
        UpgradeScreen.GetComponent<BUFusionScreenUIHandler>().EquipmentData = equipment;
        UpgradeScreen.SetActive(true);
    }
}
