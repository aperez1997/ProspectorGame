
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem : ISerializationCallbackReceiver
{
    public ItemId id;

    public int amount;

    // cached from itemData
    public ItemCategory Category { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sprite Sprite { get; private set; }
    public bool Stackable { get; private set; }
    public int Price { get; private set; }

    public InventoryItem(ItemId type, int amount)
    {
        this.id = type;
        this.amount = amount;
        LoadItemData();
    }

    private void LoadItemData()
    {
        ItemData item = ItemDataLoader.LoadItemById(id);
        if (item is ItemData)
        {
            Category = item.category;
            Name = item.name;
            Description = item.description;
            Sprite = item.sprite;
            Stackable = item.stackable;
            Price = item.price;
        }
        else
        {
            //Debug.LogError("Could not find item " + id.ToString());
        }
    }


    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize(){ LoadItemData(); }

    public override string ToString()
    {
        return "I:"+id+":"+amount;
    }
}