using UnityEngine;
using UnityEngine.EventSystems;

public class EquippableSlot : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private EquipmentSO equipmentSo;
    [SerializeField] private ActionItem upgradeMaterialSO;
    [SerializeField] private GameObject UpgradeScreen;
    public void OnPointerDown(PointerEventData eventData)
    {
        var FusionScreenUI = UpgradeScreen.GetComponent<FusionScreenUIHandler>();
        FusionScreenUI.EquipmentSoData = equipmentSo;
        FusionScreenUI.UpgradeMaterialSoData = upgradeMaterialSO;
        
        UpgradeScreen.SetActive(true);
    }
}
