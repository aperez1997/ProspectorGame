
using System;
using UnityEngine;

[Serializable]
public class InventoryItem : ISerializationCallbackReceiver
{
    public ItemType type;

    public int amount;

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sprite Sprite { get; private set; }
    public bool Stackable { get; private set; }

    public InventoryItem(ItemType type, int amount)
    {
        this.type = type;
        this.amount = amount;
        LoadItemData();
    }

    private void LoadItemData()
    {
        ItemData item = ItemDataLoader.LoadItemByType(type);
        if (item is ItemData)
        {
            Name = item.name;
            Description = item.description;
            Sprite = item.sprite;
            Stackable = item.stackable;
        }
        else
        {
            Debug.LogError("Could not find item " + type.ToString());
        }
    }


    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize(){ LoadItemData(); }

    public override string ToString()
    {
        return "I:"+type+":"+amount;
    }
}