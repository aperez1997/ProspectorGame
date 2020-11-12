using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemUIMetaData : MonoBehaviour
{
    public ItemId type;
    public int amount;

    public void SetFromInventoryItem(InventoryItem item)
    {
        type = item.id;
        amount = item.amount;
    }
}
