using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    // This is how we emit that the Items have changed. UI will catch this and update
    public event EventHandler<InventoryChangedEventArgs> OnItemListChanged;

    [field:SerializeField] public List<InventoryItem> ItemList { get; private set; }

    public Inventory()
    {
        ItemList = new List<InventoryItem>();
    }

    /** 
     *  <summary>returns true if inventory has an item of the given id</summary>
     *  <param name="id">item to check for</param>
     *  <param name="count">number to check for, stackables only!</param>
     */
    public bool HasItem(ItemId id, int count = 1)
    {
        return HasItem(id, out _, count);
    }

    /** 
     *  <summary>returns true if inventory has an item of the given id</summary>
     *  <param name="id">id of item to check for</param>
     *  <param name="returnItem">If item exists, it will be returned here. For non-stackables, this will be the first item only</param>
     *  <param name="count">number to check for, stackables only!</param>
     */
    public bool HasItem(ItemId id, out InventoryItem returnItem, int count = 1)
    {
        returnItem = null;
        for (int i = 0; i < ItemList.Count; i++)
        {
            InventoryItem inventoryItem = ItemList[i];
            if (inventoryItem.id == id && inventoryItem.amount >= count)
            {
                returnItem = inventoryItem;
                return true;
            }
        }
        return false;
    }

    public List<InventoryItem> GetItemsByCategory(ItemCategory category)
    {
        var foundItems = new List<InventoryItem>();
        for (int i = 0; i < ItemList.Count; i++) {
            InventoryItem inventoryItem = ItemList[i];
            if (inventoryItem.Category == category) {
                foundItems.Add(inventoryItem);
            }
        }
        return foundItems;
    }

    /// <summary>Shortcut to add item, if you only care about id+amount</summary>
    public void AddItem(ItemId id, int amount)
    {
        var item = new InventoryItem(id, amount);
        AddItem(item);
    }

    /// <summary>Proper way to add an item</summary>
    public void AddItem(InventoryItem item)
    {
        ItemId id = item.id;
        bool haveItem = HasItem(id, out InventoryItem foundItem);
        int newAmount = item.amount;
        if (item.Stackable && haveItem){
            foundItem.amount += item.amount;
            newAmount = foundItem.amount;
        } else {
            ItemList.Add(item);
        }

        var e = new InventoryChangedEventArgs(id, item.amount, newAmount);
        OnItemListChanged?.Invoke(this, e);
    }

    public bool RemoveItem(ItemId id, int count = 1)
    {
        bool have = HasItem(id, out InventoryItem foundItem, count);
        if (have) {
            foundItem.amount -= count;
            if (foundItem.amount == 0)
            {
                // remove empty items
                ItemList.Remove(foundItem);
            }
            var e = new InventoryChangedEventArgs(id, -1 * count, foundItem.amount);
            OnItemListChanged?.Invoke(this, e);
            return have;
        } else {
            if (foundItem is InventoryItem)
            {
                Debug.Log("tried to remove " + count + " of id " + id.ToString() + " but there are only " + foundItem.amount +"!");
            } else
            {
                Debug.Log("tried to remove " + count + " of id " + id.ToString() + " but there aren't any!");
            }

            return false;
        }
    }

    public override string ToString()
    {
        bool first = true;
        string itemStrings = "";
        foreach (InventoryItem item in ItemList)
        {
            string prefix = first ? "" : ",";
            first = false;
            itemStrings += prefix + item.ToString();
        }
        return "Inv{ " + itemStrings + "}";
    }
} 
