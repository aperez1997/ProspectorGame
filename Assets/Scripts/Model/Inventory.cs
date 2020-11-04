﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    // This is how we emit that the Items have changed. UI will catch this and update
    public event EventHandler OnItemListChanged;

    [field:SerializeField] public List<InventoryItem> ItemList { get; private set; }

    public Inventory()
    {
        ItemList = new List<InventoryItem>();
    }

    /** 
     *  <summary>returns true if inventory has an item of the given type</summary>
     *  <param name="type">type of item to check for</param>
     *  <param name="count">number to check for, stackables only!</param>
     */
    public bool HasItem(ItemType type, int count = 1)
    {
        return HasItem(type, out _, count);
    }

    /** 
     *  <summary>returns true if inventory has an item of the given type</summary>
     *  <param name="type">type of item to check for</param>
     *  <param name="returnItem">If item exists, it will be returned here. For non-stackables, this will be the first item only</param>
     *  <param name="count">number to check for, stackables only!</param>
     */
    public bool HasItem(ItemType type, out InventoryItem returnItem, int count = 1)
    {
        returnItem = null;
        for (int i = 0; i < ItemList.Count; i++)
        {
            InventoryItem inventoryItem = ItemList[i];
            if (inventoryItem.type == type && inventoryItem.amount >= count)
            {
                returnItem = inventoryItem;
                return true;
            }
        }
        return false;
    }

    public void AddItem(InventoryItem item)
    {
        ItemType type = item.type;
        bool haveItem = HasItem(type, out InventoryItem foundItem);
        if (item.Stackable && haveItem){
            foundItem.amount += item.amount;
        } else {
            ItemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool RemoveItem(ItemType type, int count = 1)
    {
        bool have = HasItem(type, out InventoryItem foundItem, count);
        if (have) {
            foundItem.amount -= count;
            if (foundItem.amount == 0)
            {
                // remove empty items
                ItemList.Remove(foundItem);
            }
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return have;
        } else {
            if (foundItem is InventoryItem)
            {
                Debug.Log("tried to remove " + count + " of type " + type.ToString() + " but there are only " + foundItem.amount +"!");
            } else
            {
                Debug.Log("tried to remove " + count + " of type " + type.ToString() + " but there aren't any!");
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
