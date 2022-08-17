using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadInventoryFromPlayerPrefs : MonoBehaviour
{
    //PRIVATE VARIABLES
    [SerializeField] private FusionManager _fusionManager;
    [SerializeField] private Libraries _libraries;
    [SerializeField] private GameObject _inventorySlot;
    [SerializeField] private GameObject[] _inventorySlots;
    [SerializeField] private GameObject _inventorySlotUI;
    private List<GameObject> _currentItems = new();
    [SerializeField] private FusionScreenUIHandler _fusionUI;
    [SerializeField] private Image[] _equipmentIcons;
    private void OnEnable()
    {
        _fusionUI.OnInventoryChange += UpdateInventory;
    }

    private void OnDisable()
    {
        _fusionUI.OnInventoryChange -= UpdateInventory;
    }

    private void Start()
    {
      UpdateInventory();
    }
    
    //Yes this is inefficient. But it is million times better than no system at all...
    private void UpdateInventory()
    {
        if (_currentItems.Count > 0)
        {
            DestroyCurrentItemsInInventory();
        }
        var index = 0;
        
        foreach (var item in _libraries.MatlerialsLibrarySo.Materials)
        {
            if (PlayerPrefs.GetInt(item.GetItemID()) <= 0) continue;
            var itemInSlot = Instantiate(_inventorySlot, _inventorySlots[index].transform.position , Quaternion.identity);
            _currentItems.Add(itemInSlot);
            itemInSlot.transform.parent = _inventorySlotUI.transform;
            itemInSlot.GetComponent<InventorySlot>().SetItemSlot(item);
            index++;
        }
        
        UpdateEquipmentIcons();
    }

    public void UpdateEquipmentIcons()
    {
        foreach (var equipmentIcon in _equipmentIcons)
        {
            foreach (var equipmentSo in _libraries.EquipmentLibrarySo.EquipablesLibrary)
            {
                if (equipmentIcon.transform.name.Contains(equipmentSo.Name))
                {       
                    equipmentIcon.sprite = equipmentSo.Icon[(int)_fusionManager.GetCurrentRarity(equipmentSo)];
                }
            }
        }
    }
    private void DestroyCurrentItemsInInventory()
    {
        foreach (var currentItem in _currentItems)
        {
            Destroy(currentItem);
        }
    }
}