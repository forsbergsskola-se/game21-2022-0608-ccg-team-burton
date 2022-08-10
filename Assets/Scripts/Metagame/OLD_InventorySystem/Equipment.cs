using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour, I_Saveable
{
    Dictionary<EquipedLocation, EquipedItem> equippedItems = new Dictionary<EquipedLocation, EquipedItem>();
    public event Action equipmentUpdated;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public EquipedItem GetItemInSlot(EquipedLocation equipLocation)
    {
        if (!equippedItems.ContainsKey(equipLocation))
        {
            return null;
        }

        return equippedItems[equipLocation];
    }

    public void AddItem(EquipedLocation slot, EquipedItem item)
    {
        Debug.Assert(item.GetAllowedEquipLocation() == slot);

        equippedItems[slot] = item;

        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public void RemoveItem(EquipedLocation slot)
    {
        equippedItems.Remove(slot);
        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public object CaptureState()
    {
        var equippedItemsForSerialization = new Dictionary<EquipedLocation, string>();
        foreach (var pair in equippedItems)
        {
            equippedItemsForSerialization[pair.Key] = pair.Value.GetItemID();
        }
        return equippedItemsForSerialization;
    }

    public void RestoreState(object state)
    {
        equippedItems = new Dictionary<EquipedLocation, EquipedItem>();

        var equippedItemsForSerialization = (Dictionary<EquipedLocation, string>)state;

        foreach (var pair in equippedItemsForSerialization)
        {
            var item = (EquipedItem)InventoryItem.GetFromID(pair.Value);
            if (item != null)
            {
                equippedItems[pair.Key] = item;
            }
        }
    }
}
