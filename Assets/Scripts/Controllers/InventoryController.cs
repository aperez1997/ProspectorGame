﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public Transform ItemContainer;
    public GameObject ItemTemplate;

    private Inventory inventory;

    private void Awake()
    {
        ItemContainer = transform.Find("Panel-Inventory");
    }

    private void Start()
    {
        // get inventory from singleton. Is this the right way?
        SetInventory(GameState.Instance.Inventory);
        UpdateInventoryUI();
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        // subscribe to changes.
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    public void UpdateInventoryUI()
    {
        // remove old display
        foreach (Transform child in ItemContainer)
        {
            Destroy(child.gameObject);
        }

        // display items   
        foreach (InventoryItem item in inventory.ItemList)
        {
            GameObject goItem = Instantiate(ItemTemplate, ItemContainer);
            goItem.SetActive(true);

            // find sprit
            Image image = goItem.GetComponentInChildren<Image>();
            image.sprite = item.Sprite;

            // find amount text and adjust
            Text itemAmountText = goItem.GetComponentInChildren<Text>();
            if (item.Stackable)
            {
                itemAmountText.text = item.amount.ToString();
                itemAmountText.enabled = true;
            } else
            {
                itemAmountText.enabled = false;
            }


            // for tooltips
            ToolTipUIHelper helper = goItem.GetComponent<ToolTipUIHelper>();
            helper.text = item.Name + "\n" + item.Description;
        }
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        UpdateInventoryUI();
    }
}
