using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controller for General Store scene.
/// Is also a store controller sub-class, used with the general-store inventory from gamelogic
/// </summary>
public class GeneralStoreController : StoreInventoryController
{
    private Player player;


    protected override string GetItemContainerName() { return "Store Container"; }

    protected override void Start()
    {
        // for loading this scene from editor
        GameStateManager.DebugLoadState();

        Inventory = GameStateManager.LogicInstance.GetStoreInventory();

        // subscribe to money change event
        player = GameStateManager.LogicInstance.Player;
        player.Inventory.OnItemListChanged += Player_OnMoneyChanged;

        base.Start();
    }

    public void ExitStore()
    {
        SceneController.UnloadAdditiveScene();
    }

    protected override void SetPrefabDetails(GameObject gameObject, InventoryItem item)
    {
        SetInventoryStorePrefab_Static(gameObject, item);

        // hide item amount
        var itemAmountText = GetAmountText(gameObject);
        itemAmountText.enabled = false;

        // set price
        int price = item.Price;
        SetPriceText(gameObject, price);

        // setup button
        var button = GetActionButton(gameObject);
        button.interactable = gameLogic.CanAfford(price);
        button.onClick.AddListener(() => {
            gameLogic.BuyItem(item.id, price);
        });
        SetButtonText(gameObject, "Buy");
    }

    private void Player_OnMoneyChanged(object sender, EventArgs e)
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        player.Inventory.OnItemListChanged -= Player_OnMoneyChanged;
    }
}
