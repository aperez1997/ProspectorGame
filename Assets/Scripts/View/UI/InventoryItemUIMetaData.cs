using UnityEngine;

/// <summary>
/// This is set into the UI prefab for inventory items.
/// It allows us to identify what item it represents when there's an event later
/// </summary>
public class InventoryItemUIMetaData : MonoBehaviour
{
    public string itemId;
    public int amount;

    public void SetFromInventoryItem(InventoryItem item)
    {
        itemId = item.Id;
        amount = item.Amount;
    }
}
