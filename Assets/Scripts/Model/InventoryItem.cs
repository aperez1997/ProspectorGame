
using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemType type;

    public int amount;

    public Sprite Sprite { get
        {
            ItemData item = ItemDataLoader.LoadItemByType(type);
            if (item is ItemData){ return item.sprite; }
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