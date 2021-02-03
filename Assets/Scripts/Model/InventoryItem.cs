
using System;
using UnityEngine;

/// <summary>
/// links players inventory to underlying scripted objects
/// </summary>
[Serializable]
public class InventoryItem : ISerializationCallbackReceiver
{
    public string Id;

    public int Amount;

    // cached from itemData
    public ItemCategory Category { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sprite Sprite { get; private set; }
    public bool Stackable { get; private set; }
    public int Price { get; private set; }
    public GameEvent[] GameEvents { get; private set; }

    // Weapon sub-type
    public ItemData AmmoItem { get; private set; }
    public int? HuntingModifier { get; private set; }

    public InventoryItem(string id, int amount)
    {
        this.Id = id;
        this.Amount = amount;
        LoadItemData();
    }

    // This pattern is required because of SO sub-class properties
    private void LoadItemData()
    {
        ItemData item = ItemDataLoader.LoadItemById(Id);
        if (!(item is ItemData)) {
            //Debug.LogError("Could not find item " + id.ToString());
            return;
        }

        Category = item.category;
        Name = item.name;
        Description = item.description;
        Sprite = item.sprite;
        Stackable = item.stackable;
        Price = item.price;
        GameEvents = item.gameEvents;

        if (item is ItemDataWeapon weaponItem) {
            AmmoItem = weaponItem.Ammo;
            HuntingModifier = weaponItem.HuntingModifier;
        }
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize(){ LoadItemData(); }

    public override string ToString()
    {
        return "I:"+Id+":"+Amount;
    }
}