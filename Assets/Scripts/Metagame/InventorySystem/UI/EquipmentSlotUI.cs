using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An slot for the players equipment.
/// </summary>
public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
{
    // CONFIG DATA

    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] EquipedLocation equipLocation = EquipedLocation.Weapon;

    // CACHE
    Equipment playerEquipment;

    // LIFECYCLE METHODS

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerEquipment = player.GetComponent<Equipment>();
        playerEquipment.equipmentUpdated += RedrawUI;
    }

    private void Start()
    {
        RedrawUI();
    }

    // PUBLIC

    public int MaxAcceptable(InventoryItem item)
    {
        EquipedItem equipableItem = item as EquipedItem;
        if (equipableItem == null) return 0;
        if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;
        if (GetItem() != null) return 0;

        return 1;
    }

    public void AddItems(InventoryItem item, int number)
    {
        playerEquipment.AddItem(equipLocation, (EquipedItem)item);
    }

    public InventoryItem GetItem()
    {
        return playerEquipment.GetItemInSlot(equipLocation);
    }

    public int GetNumber()
    {
        if (GetItem() != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void RemoveItems(int number)
    {
        playerEquipment.RemoveItem(equipLocation);
    }

    // PRIVATE

    void RedrawUI()
    {
        icon.SetItem(playerEquipment.GetItemInSlot(equipLocation));
    }
}