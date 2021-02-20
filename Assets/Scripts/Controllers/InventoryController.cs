using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller that displays the contents of an Inventory
/// Use with the pfInvetoryItem prefab
/// </summary>
abstract public class InventoryController : ContainerDisplayController<InventoryItem>
{
    private Inventory inventory;
    protected Inventory Inventory {
        get { return inventory; }
        set {
            if (inventory is Inventory) {
                inventory.OnItemListChanged -= Inventory_OnItemListChanged;
            }
            inventory = value;
            inventory.OnItemListChanged += Inventory_OnItemListChanged;
        }
    }

    protected override ReadOnlyCollection<InventoryItem> GetItemList()
    {
        return this.inventory.ItemList;
    }

    protected override void SetPrefabDetails(GameObject goItem, InventoryItem item)
    {
        // MetaData
        var metaData = goItem.GetComponent<InventoryItemUIMetaData>();
        metaData.SetFromInventoryItem(item);

        // find sprit
        Image image = goItem.GetComponentInChildren<Image>();
        image.sprite = item.Sprite;
        image.enabled = true;

        var nameText = Utils.FindInChildren(goItem, "Text Name").GetComponent<TextMeshProUGUI>();
        nameText.text = item.Name;

        // find amount text and set
        var itemAmountText = GetAmountText(goItem);
        if (item.Stackable) {
            itemAmountText.text = GetItemAmountString(item);
            itemAmountText.enabled = true;
        } else {
            itemAmountText.enabled = false;
        }

        // for tooltips
        var helper = goItem.GetComponent<ToolTipUIHelper>();
        helper.text = item.Description;

        goItem.SetActive(true);
    }

    protected static string GetItemAmountString(InventoryItem item)
    {
        var style = QuantityStyle.None;

        string output = item.Amount.ToString();
        if (item.ItemData is ItemDataResource itemDataResource) {
            style = itemDataResource.quantityStyle;
        }

        switch (style) {
            case QuantityStyle.Money:
                output = GetMoneyString(item.Amount);
                break;
        }

        return output;
    }

    protected static string GetMoneyString(int amount)
    {
        int dollars = amount / 100;
        int cents = amount % 100;
        string output = dollars.ToString();
        if (cents > 0) { output += "." + cents; }
        return "$" + output;
    }

    public static TextMeshProUGUI GetAmountText(GameObject goItem)
    {
        return Utils.FindInChildren(goItem, "Text Quantity").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Finds the transform for the first inventory item of the given type, or null
    /// </summary>
    protected Transform FindInventoryItemTransform(string itemId)
    {
        foreach (Transform child in this.ItemContainer)
        {
            // skip the template
            if (child == this.ItemTemplate.transform) { continue; }
            var metaData = child.GetComponent<InventoryItemUIMetaData>();
            if (metaData.itemId == itemId)
            {
                return child;
            }
        }
        return null;
    }

    protected void Inventory_OnItemListChanged(object sender, InventoryChangedEventArgs e)
    {
        // find item that changed and add popUp
        var child = FindInventoryItemTransform(e.ItemId);
        if (child is Transform)
        {
            PopUpTextDriverV1.CreateInventoryChangePopUp(child, e.Delta, e.Name);
        }

        OnItemListChanged(sender, e);
    }

    private void OnDestroy()
    {
        // if we don't do this, it will still get called and cause errors
        // because the gameObjects will be gone
        this.inventory.OnItemListChanged -= Inventory_OnItemListChanged;
    }
}