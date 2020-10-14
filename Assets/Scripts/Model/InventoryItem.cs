
using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemType type;

    public int amount;

    public Sprite Sprite { get
        {
            Item item = ItemLoader.LoadItemByType(type);
            if (item is Item){ return item.sprite; }
            return null;
        }       
    }

    public InventoryItem(ItemType type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }

    public override string ToString()
    {
        return "I:"+type+":"+amount;
    }
}