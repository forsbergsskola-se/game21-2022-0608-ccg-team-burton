using UnityEngine;

public class InventoryItem : ScriptableObject
{
    // CONFIG DATA
    [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
    [SerializeField] string itemID = null;
    [Tooltip("Item name to be displayed in UI.")]
    [SerializeField] string displayName = null;
    [Tooltip("Item description to be displayed in UI.")]
    [SerializeField][TextArea] string description = null;
    [Tooltip("The UI icon to represent this item in the inventory.")]
    [SerializeField] Sprite icon = null;

    public Sprite GetIcon()
    {
        return icon;
    }
    public string GetItemID()
    {
        return itemID;
    }
    public string GetDisplayName()
    {
        return displayName;
    }
}