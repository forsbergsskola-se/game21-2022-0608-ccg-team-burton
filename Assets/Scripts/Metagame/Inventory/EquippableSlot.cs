using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippableSlot : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private EquipmentSO equipmentSo;
    [SerializeField] private ActionItem upgradeMaterialSO;
    [SerializeField] private GameObject UpgradeScreen;
    public void OnPointerDown(PointerEventData eventData)
    {
        //TODO: Cache
        UpgradeScreen.GetComponent<FusionScreenUIHandler>().equipmentSoData = equipmentSo;
        UpgradeScreen.GetComponent<FusionScreenUIHandler>().UpgradeMaterialSoData = upgradeMaterialSO;
        
        UpgradeScreen.SetActive(true);
    }
}
