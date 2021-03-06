
using System;
using UnityEngine;

/// <summary>
/// links players inventory to underlying scripted objects
/// </summary>
[Serializable]
public class InventoryItem : IHasGameEvents
{
    public string Id;

    public int Amount;

    public InventoryItem(string id, int amount)
    {
        this.Id = id;
        this.Amount = amount;
    }

    // use this to get any SO sub-class specific properties
    private ItemData _itemData;
    public ItemData ItemData {
        get {
            if (_itemData is null) {
                var data = ItemDataLoader.LoadItemById(Id);
                if (!(data is ItemData)) {
                    throw new Exception("No item data for id " + Id);
                }
                _itemData = data;
            }
            return _itemData;
        }
    }

    /// <summary>
    /// Get the modifier associated with the given action type
    /// Applies to tools only, anything else will return 0
    /// This shorthand is provided because tool lookups are common
    /// </summary>
    public int GetActionModifier(ActionType action)
    {
        if (ItemData is ItemDataTool itemDataTool) {
            if (itemDataTool.HasAbility(action, out int toolModifier)) {
                return toolModifier;
            }
        } else {
            Debug.LogWarning("Trying to get action modifier [" + action.ToString() + "] for a non-tool " + ToString());
        }
        return 0;
    }

    // cached from itemData
    public ItemCategory Category { get { return ItemData.category; } }
    public string Name { get { return ItemData.name; } }
    public string Description { get { return ItemData.description; } }
    public Sprite Sprite { get { return ItemData.sprite; } }
    public bool Stackable { get { return ItemData.stackable; } }
    public int Price { get { return ItemData.price; } }
    public GameEvent[] GameEvents { get { return ItemData.gameEvents; } }
    public GameEvent[] GetGameEvents() { return GameEvents; }

    public override string ToString()
    {
        return "I:"+Id+":"+Amount;
    }
}