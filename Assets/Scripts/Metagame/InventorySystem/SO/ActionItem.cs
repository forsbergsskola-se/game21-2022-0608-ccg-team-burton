using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = ("Team5/Inventory/Action Item"))]


public class ActionItem : InventoryItem
{
    [Tooltip("Does this item get consumed every time its used.")]
    [SerializeField] bool consumable = false;

    public virtual bool Use(GameObject user)
    {
        if (pickup.TryGetComponent(out IConsumable consumable))
        {
            return consumable.Consume(user);
        }

        Debug.Log("Item could not be consumed since no IConsumable interface was found.");
        return false;
    }

    public bool isConsumable()
    {
        return consumable;
    }
}