using System;
using UnityEngine;



/// <summary>
/// This component should be placed on the GameObject tagged "Player".
/// </summary>
public class Inventory : MonoBehaviour, I_Saveable
{

    [Tooltip("Allowed size")]
    [SerializeField] int inventorySize = 16;

    InventorySlot[] slots;

    public struct InventorySlot
    {
        public int number;
        public InventoryItem item;
    }



    /// <summary>
    /// shows when the items in the slots are added/removed.
    /// </summary>
    public event Action inventoryUpdated;

    /// <summary>
    /// Convenience for getting the player's inventory.
    /// </summary>
    public static Inventory GetPlayerInventory()
    {
        var player = GameObject.FindWithTag("Player");
        return player.GetComponent<Inventory>();
    }

    /// <summary>
    /// Could this item fit anywhere 
    /// </summary>
    public bool HasSpaceFor(InventoryItem item)
    {
        return FindSlot(item) >= 0;
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetSize()
    {
        return slots.Length;
    }

    /// <summary>
    /// Attempt to add the items to the first available slot.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>Whether or not the item could be added.</returns>
    public bool AddToFirstEmptySlot(InventoryItem item, int number)
    {
        int i = FindSlot(item);

        if (i < 0)
        {
            return false;
        }

        slots[i].item = item;
        slots[i].number += number;
        print(slots[i].number);
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    /// <summary>
    /// Is there an instance of the item in the inventory?
    /// </summary>
    public bool HasItem(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i], item))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Return the item type in the given slot.
    /// </summary>
    public InventoryItem GetItemInSlot(int slot)
    {
        return slots[slot].item;
    }

    /// <summary>
    /// Remove the item from the given slot.
    /// </summary>
    public void RemoveFromSlot(int slot, int number)
    {
        slots[slot].item = null;
        if (slots[slot].number >= 0)
        {
            slots[slot].number = 0;
            slots[slot].item = null;
        }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }

    /// <summary>
    /// Will add an item to the given slot if possible. If there is already
    /// a stack of this type, it will add to the existing stack. Otherwise,
    /// it will be added to the first empty slot.
    /// </summary>
    /// <param name="slot">The slot to attempt to add to.</param>
    /// <param name="item">The item type to add.</param>
    /// <returns>True if the item was added anywhere in the inventory.</returns>
    public bool AddItemToSlot(int slot, InventoryItem item, int number)
    {
        if (slots[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number); ;
        }

        var i = FindStack(item);
        if (i >= 0)
        {
            slot = i;
        }
        slots[slot].item = item;
        slots[slot].number += number;

        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }


    private void Awake()
    {
        slots = new InventorySlot[inventorySize];
    }

    /// <summary>
    /// Find a slot that can accomodate the given item.
    /// </summary>
    /// <returns>-1 if no slot is found.</returns>
    private int FindSlot(InventoryItem item)
    {
        int i = FindStack(item);
        if (i < 0)
        {
            i = FindEmptySlot();
        }
        return i;
    }

    private int FindStack(InventoryItem item)
    {
        if (!item.IsStackable())
        {
            return -1;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Find an empty slot.
    /// </summary>
    /// <returns>-1 if all slots are full.</returns>
    private int FindEmptySlot()
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }

    public int GetNumberInSlot(int index)
    {
        return slots[index].number;
    }
    [System.Serializable]
    private struct InventorySlotRecord
    {
        public string itemID;
        public int number;
    }

    object I_Saveable.CaptureState()
    {
        var slotRecords = new InventorySlotRecord[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (slots[i].item != null)
            {
                slotRecords[i].itemID = slots[i].item.GetItemID();
                slotRecords[i].number = slots[i].number;
            }
        }
        return slotRecords;
    }

    void I_Saveable.RestoreState(object state)
    {
        var slotRecords = (InventorySlotRecord[])state;
        for (int i = 0; i < inventorySize; i++)
        {
            slots[i].item = InventoryItem.GetFromID(slotRecords[i].itemID);
            slots[i].number = slotRecords[i].number;
        }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }
}