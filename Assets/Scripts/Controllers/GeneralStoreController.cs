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

        this.Inventory = GameStateManager.LogicInstance.GetStoreInventory();

        // subscribe to money change event
        this.player = GameStateManager.LogicInstance.Player;
        this.player.Inventory.OnItemListChanged += Player_OnMoneyChanged;

        base.Start();
    }

    public void ExitStore()
    {
        SceneController.UnloadAdditiveScene();
    }

    protected override void SetPrefabDetails(GameObject goItem, InventoryItem item)
    {
        base.SetPrefabDetails(goItem, item);

        // hide item amount
        var itemAmountText = GetAmountText(goItem);
        itemAmountText.enabled = false;

        // set price
        int price = item.Price;
        SetPriceText(goItem, price);

        // setup button
        var button = GetActionButton(goItem);
        button.interactable = gameLogic.CanAfford(price);
        button.onClick.AddListener(() => {
            gameLogic.BuyItem(item.Id, price);
        });
        SetButtonText(goItem, "Buy");
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
