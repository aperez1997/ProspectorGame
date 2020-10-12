using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    public List<Item> ItemList { get => itemList; }

    public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item { type = Item.ItemType.Ration, amount = 7 });
        AddItem(new Item { type = Item.ItemType.Ration, amount = 2 });
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty) ;
    }
} 
