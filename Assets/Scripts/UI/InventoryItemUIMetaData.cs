using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemUIMetaData : MonoBehaviour
{
    public ItemType type;
    public int amount;

    public void SetFromInventoryItem(InventoryItem item)
    {
        type = item.type;
        amount = item.amount;
    }
}
