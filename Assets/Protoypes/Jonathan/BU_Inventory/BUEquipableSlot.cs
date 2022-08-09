using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BUEquipableSlot : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private BUEquipmentSO equipmentSo;
    [SerializeField] private GameObject UpgradeScreen;
    public void OnPointerDown(PointerEventData eventData)
    {
        UpgradeScreen.GetComponent<BUFusionScreenUIHandler>().equipmentSoData = equipmentSo;
        UpgradeScreen.SetActive(true);
    }
}
