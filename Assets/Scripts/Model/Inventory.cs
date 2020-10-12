using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Inventory
{
    // This is how we emit that the Items have changed. UI will catch this and update
    public event EventHandler OnItemListChanged;

    public List<Item> ItemList { get; private set; }

    public Inventory()
    {
        ItemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        ItemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
} 
