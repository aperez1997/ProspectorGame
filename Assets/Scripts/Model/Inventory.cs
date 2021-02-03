using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Player's inventory. Basically a list of InventoryItem objects
/// </summary>
[Serializable]
public class Inventory
{
    // This is how we emit that the Items have changed. UI will catch this and update
    public event EventHandler<InventoryChangedEventArgs> OnItemListChanged;

    [SerializeField] private List<InventoryItem> _itemList;
    public ReadOnlyCollection<InventoryItem> ItemList { get { return _itemList.AsReadOnly(); } }

    public Inventory()
    {
        _itemList = new List<InventoryItem>();
    }

    /** 
     *  <summary>returns true if inventory has the given item</summary>
     *  <param name="itemData">item to check for</param>
     *  <param name="count">number to check for, stackables only!</param>
     */
    public bool HasItem(ItemData itemData, int count = 1)
    {
        return HasItem(itemData.id, out _, count);
    }

    /** 
     *  <summary>returns true if inventory has the given item</summary>
     *  <param name="itemId">item to check for</param>
     *  <param name="count">number to check for, stackables only!</param>
     */
    public bool HasItem(string itemId, int count = 1)
    {
        return HasItem(itemId, out _, count);
    }

    /** 
     *  <summary>returns true if inventory has an item of the given id</summary>
     *  <param name="itemId">id of item to check for</param>
     *  <param name="returnItem">If item exists, it will be returned here. For non-stackables, this will be the first item only</param>
     *  <param name="count">number to check for, stackables only!</param>
     */
    public bool HasItem(string itemId, out InventoryItem returnItem, int count = 1)
    {
        returnItem = null;
        for (int i = 0; i < _itemList.Count; i++)
        {
            InventoryItem inventoryItem = _itemList[i];
            if (inventoryItem.Id == itemId && inventoryItem.Amount >= count)
            {
                returnItem = inventoryItem;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns true if the inventory contains at least 1 item of the given type
    /// </summary>
    public bool HasItemOfCategory(ItemCategory category)
    {
        foreach (var inventoryItem in _itemList) {
            if (inventoryItem.Category == category) {
                return true;
            }
        }
        return false;
    }

    public List<InventoryItem> GetItemsByCategory(ItemCategory category)
    {
        var foundItems = new List<InventoryItem>();
        for (int i = 0; i < _itemList.Count; i++) {
            InventoryItem inventoryItem = _itemList[i];
            if (inventoryItem.Category == category) {
                foundItems.Add(inventoryItem);
            }
        }
        return foundItems;
    }

    public bool HasAmmoForWeapon(InventoryItem item)
    {
        if (item.Category != ItemCategory.Weapons) {
            Debug.LogWarning("Tried to look for ammo for a non-wweapon " + item.ToString());
            return false;
        } else if (!(item.AmmoItem is ItemData)) {
            Debug.LogWarning("Tried to look for ammo for a weapon without ammoId " + item.ToString());
            return false;
        }
        var ammoItem = item.AmmoItem;
        return HasItem(ammoItem);
    }

    /// <summary>Shortcut to add item by itemData</summary>
    public void AddItem(ItemData data, int amount)
    {
        AddItem(data.id, amount);
    }

    /// <summary>Shortcut to add item, if you only care about id+amount</summary>
    public void AddItem(string id, int amount)
    {
        var item = new InventoryItem(id, amount);
        AddItem(item);
    }

    /// <summary>Proper way to add an item</summary>
    public void AddItem(InventoryItem item)
    {
        string id = item.Id;
        bool haveItem = HasItem(id, out InventoryItem foundItem);
        int newAmount = item.Amount;
        if (item.Stackable && haveItem){
            foundItem.Amount += item.Amount;
            newAmount = foundItem.Amount;
        } else {
            _itemList.Add(item);
        }

        var e = new InventoryChangedEventArgs(id, item.Name, item.Amount, newAmount);
        OnItemListChanged?.Invoke(this, e);
    }

    public bool RemoveItem(ItemData itemData, int count = 1)
    {
        return RemoveItem(itemData.id, count);
    }

    public bool RemoveItem(string id, int count = 1)
    {
        bool have = HasItem(id, out InventoryItem foundItem, count);
        if (have) {
            foundItem.Amount -= count;
            if (foundItem.Amount == 0)
            {
                // remove empty items
                _itemList.Remove(foundItem);
            }
            var e = new InventoryChangedEventArgs(id, foundItem.Name, -1 * count, foundItem.Amount);
            OnItemListChanged?.Invoke(this, e);
            return have;
        } else {
            if (foundItem is InventoryItem)
            {
                Debug.Log("tried to remove " + count + " of id " + id.ToString() + " but there are only " + foundItem.Amount +"!");
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
        foreach (InventoryItem item in _itemList)
        {
            string prefix = first ? "" : ",";
            first = false;
            itemStrings += prefix + item.ToString();
        }
        return "Inv{ " + itemStrings + "}";
    }
} 
