using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : ISerializationCallbackReceiver
{
    // This is how we emit that the Items have changed. UI will catch this and update
    public event EventHandler OnItemListChanged;

    [field:SerializeField] public List<InventoryItem> ItemList { get; private set; }

    private readonly Dictionary<ItemType, InventoryItem> itemDict = new Dictionary<ItemType, InventoryItem>();

    public Inventory()
    {
        ItemList = new List<InventoryItem>();
        itemDict = new Dictionary<ItemType, InventoryItem>();
    }

    public bool HasItem(ItemType type, int count = 1)
    {
        return HasItem(type, out _, count);
    }

    public bool HasItem(ItemType type, out InventoryItem returnItem, int count = 1)
    {
        itemDict.TryGetValue(type, out returnItem);
        if (returnItem is InventoryItem)
        {
            return returnItem.amount >= count;
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
            itemDict.Add(type, item);
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
                itemDict.Remove(type);
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

    public void OnBeforeSerialize(){}

    public void OnAfterDeserialize()
    {
        itemDict.Clear();
        // repopulate the dictionary [it doesn't serialize]
        foreach (InventoryItem item in ItemList)
        {
            itemDict.Add(item.type, item);
        }
    }
} 
