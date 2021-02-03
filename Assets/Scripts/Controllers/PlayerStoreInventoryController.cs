using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for displaying a Player Inventory used in Store context (sell)
/// </summary>
public class PlayerStoreInventoryController : StoreInventoryController
{
    protected override void Start()
    {
        // get these from singleton. Is this the right way?
        Inventory = GameStateManager.LogicInstance.Inventory;

        base.Start();
    }

    protected override void SetPrefabDetails(GameObject gameObject, InventoryItem item)
    {
        SetInventoryStorePrefab_Static(gameObject, item);

        var button = GetActionButton(gameObject);

        bool canSell = gameLogic.CanSell(item.Id);
        if (canSell) {
            // set price
            int price = gameLogic.GetSellPrice(item.Id);
            SetPriceText(gameObject, price);

            // setup button
            button.onClick.AddListener(() => {
                gameLogic.SellItem(item.Id, price);
            });
            SetButtonText(gameObject, "Sell");
        } else {
            // hide price
            GetPriceText(gameObject).enabled = false;
            // hide button
            button.gameObject.SetActive(false);
        }

    }
}
