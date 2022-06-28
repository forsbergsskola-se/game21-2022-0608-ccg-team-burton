using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Equipable Item"))]
public class EquipedItem : InventoryItem
{
    [Tooltip("Where allowed to put this item.")]
    [SerializeField] EquipedLocation allowedEquipLocation = EquipedLocation.Weapon;

    public EquipedLocation GetAllowedEquipLocation()
    {
        return allowedEquipLocation;
    }
}