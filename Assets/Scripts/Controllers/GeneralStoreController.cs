using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controller for general store scene
public class GeneralStoreController : MonoBehaviour
{
    public GameObject ItemTemplate;

    private Transform ItemContainer;

    private GameLogic gameLogic;

    private void Awake()
    {
        ItemContainer = Utils.FindInChildren(gameObject, "Store Container").transform;
        ItemTemplate.SetActive(false);
    }

    private void Start()
    {
        GameStateManager.DebugLoadState();
        gameLogic = GameState.Instance.GameLogic;

        // subscribe to money change event
        GameState.Instance.Player.OnMoneyChanged += Player_OnMoneyChanged;

        UpdateStoreInventoryUI();
    }

    public void ExitStore()
    {
        SceneManager.UnloadSceneAsync("GeneralStore");
    }

    public void UpdateStoreInventoryUI()
    {
        // remove old display
        foreach (Transform child in ItemContainer)
        {
            // don't destroy the Template or weird things happen
            if (child == ItemTemplate.transform) { continue; }
            Destroy(child.gameObject);
        }

        // display items
        var inventory = gameLogic.GetStoreInventory();
        foreach (InventoryItem item in inventory.ItemList)
        {
            GameObject gameObject = Instantiate(ItemTemplate, ItemContainer);
            PlayerStoreInventoryController.SetInventoryStorePrefab_Static(gameObject, item);

            // hide item amount
            var itemAmountText = InventoryController.GetAmountText(gameObject);
            itemAmountText.enabled = false;

            // set price
            int price = item.Price;
            PlayerStoreInventoryController.SetPriceText(gameObject, price);

            // setup button
            var button = PlayerStoreInventoryController.GetActionButton(gameObject);
            button.interactable = gameLogic.CanAfford(price);
            button.onClick.AddListener(() => {
                gameLogic.BuyItem(item.id, price);
            });
            var btnText = PlayerStoreInventoryController.GetButtonText(gameObject);
            btnText.text = "Buy";
        }
    }

    private void Player_OnMoneyChanged(object sender, EventArgs e)
    {
        UpdateStoreInventoryUI();
    }
}
