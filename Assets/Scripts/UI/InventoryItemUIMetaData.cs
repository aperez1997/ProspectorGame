using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
