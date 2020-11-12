using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for a Player Inventory used in Store context (sell)
/// </summary>
public class PlayerStoreInventoryController : StoreInventoryController
{
    protected override void Start()
    {
        // get these from singleton. Is this the right way?
        Inventory = GameState.Instance.Inventory;

        base.Start();
    }

    protected override void SetInventoryPrefab(GameObject gameObject, InventoryItem item)
    {
        SetInventoryStorePrefab_Static(gameObject, item);

        var button = GetActionButton(gameObject);

        bool canSell = gameLogic.CanSell(item.id);
        if (canSell) {
            // set price
            int price = gameLogic.GetSellPrice(item.id);
            SetPriceText(gameObject, price);

            // setup button
            button.onClick.AddListener(() => {
                gameLogic.SellItem(item.id, price);
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
