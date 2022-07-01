using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be placed on anything that wishes to drop pickups into the world.
/// Tracks the drops for saving and restoring.
/// </summary>
public class ItemDropper : MonoBehaviour, I_Saveable
{

    private List<Pickup> droppedItems = new List<Pickup>();


    /// <summary>
    /// Create a pickup at the current position.
    /// </summary>
    /// <param name="item">The item type for the pickup.</param>
    public void DropItem(InventoryItem item, int number)
    {
        SpawnPickup(item, GetDropLocation(), number);
    }

    /// <summary>
    /// Override to set a custom method for locating a drop.
    /// </summary>
    /// <returns>The location the drop should be spawned.</returns>
    protected virtual Vector3 GetDropLocation()
    {
        return transform.position;
    }


    public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
    {
        var pickup = item.SpawnPickup(spawnLocation, number);
        droppedItems.Add(pickup);
    }

    [System.Serializable]
    private struct DropRecord
    {
        public string itemID;
        public SerializableVector position;
        public int number;
    }

    object I_Saveable.CaptureState()
    {
        RemoveDestroyedDrops();
        var droppedItemsList = new DropRecord[droppedItems.Count];
        for (int i = 0; i < droppedItemsList.Length; i++)
        {
            droppedItemsList[i].itemID = droppedItems[i].GetItem().GetItemID();
            droppedItemsList[i].position = new SerializableVector(droppedItems[i].transform.position);
            droppedItemsList[i].number = droppedItems[i].GetNumber();
        }
        return droppedItemsList;
    }

    void I_Saveable.RestoreState(object state)
    {
        var droppedItemsList = (DropRecord[])state;
        foreach (var item in droppedItemsList)
        {
            var pickupItem = InventoryItem.GetFromID(item.itemID);
            Vector3 position = item.position.ToVector();
            int number = item.number;
            SpawnPickup(pickupItem, position, 1);
        }
    }

    /// <summary>
    /// Remove any drops in the world that have subsequently been picked up.
    /// </summary>
    private void RemoveDestroyedDrops()
    {
        var newList = new List<Pickup>();
        foreach (var item in droppedItems)
        {
            if (item != null)
            {
                newList.Add(item);
            }
        }
        droppedItems = newList;
    }
}