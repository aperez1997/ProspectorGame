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
        return inventory.ItemList;
    }

    protected override void SetPrefabDetails(GameObject goItem, InventoryItem item)
    {
        SetInventoryPrefab_Static(goItem, item);
    }

    public static void SetInventoryPrefab_Static(GameObject gameObject, InventoryItem item)
    {
        // MetaData
        var metaData = gameObject.GetComponent<InventoryItemUIMetaData>();
        metaData.SetFromInventoryItem(item);

        // find sprit
        Image image = gameObject.GetComponentInChildren<Image>();
        image.sprite = item.Sprite;
        image.enabled = true;

        var nameText = Utils.FindInChildren(gameObject, "Text Name").GetComponent<TextMeshProUGUI>();
        nameText.text = item.Name;

        // find amount text and set
        var itemAmountText = GetAmountText(gameObject);
        if (item.Stackable) {
            itemAmountText.text = item.Amount.ToString();
            itemAmountText.enabled = true;
        } else {
            itemAmountText.enabled = false;
        }

        // for tooltips
        var helper = gameObject.GetComponent<ToolTipUIHelper>();
        helper.text = item.Description;

        gameObject.SetActive(true);
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
        foreach (Transform child in ItemContainer)
        {
            // skip the template
            if (child == ItemTemplate.transform) { continue; }
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
        inventory.OnItemListChanged -= Inventory_OnItemListChanged;
    }
}