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

    public void AddItem(InventoryItem item)
    {
        ItemType type = item.type;
        bool haveItem = itemDict.TryGetValue(type, out InventoryItem foundItem);
        if (foundItem is InventoryItem){
            foundItem.amount += item.amount;
        } else {
            ItemList.Add(item);
            itemDict.Add(type, item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
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
