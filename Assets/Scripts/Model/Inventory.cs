using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    // This is how we emit that the Items have changed. UI will catch this and update
    public event EventHandler OnItemListChanged;

    [SerializeField] private List<Item> _itemList;
    public List<Item> ItemList { get => _itemList; private set => _itemList = value; }

    public Inventory()
    {
        ItemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        ItemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public override string ToString()
    {
        bool first = true;
        string itemStrings = "";
        foreach (Item item in ItemList)
        {
            string prefix = first ? "" : ",";
            first = false;
            itemStrings += prefix + item.ToString();
        }
        return "Inv{ " + itemStrings + "}";
    }
} 
