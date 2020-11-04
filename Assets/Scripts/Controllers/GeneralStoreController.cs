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
            GameObject goItem = Instantiate(ItemTemplate, ItemContainer);
            InventoryController.SetInventoryPrefab(goItem, item);

            // hide item amount
            var itemAmountText = InventoryController.GetAmountText(goItem);
            itemAmountText.enabled = false;

            // set price
            int price = item.Price;
            var priceText = Utils.FindInChildren(goItem, "Price").GetComponent<TextMeshProUGUI>();
            priceText.text = "$" + price.ToString();

            // setup button
            var button = goItem.GetComponentInChildren<Button>();
            button.interactable = gameLogic.CanAfford(price);
            button.onClick.AddListener(() => {
                gameLogic.BuyItem(price, item.type);
            });
        }
    }

    private void Player_OnMoneyChanged(object sender, EventArgs e)
    {
        UpdateStoreInventoryUI();
    }
}
