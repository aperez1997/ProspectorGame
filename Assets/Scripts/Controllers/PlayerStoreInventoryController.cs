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
        this.Inventory = GameStateManager.LogicInstance.Inventory;

        base.Start();
    }

    protected override void SetPrefabDetails(GameObject goItem, InventoryItem item)
    {
        base.SetPrefabDetails(goItem, item);

        var button = GetActionButton(goItem);

        bool canSell = this.gameLogic.CanSell(item.Id);
        if (canSell) {
            // set price
            int price = this.gameLogic.GetSellPrice(item.Id);
            SetPriceText(goItem, price);

            // setup button
            button.onClick.AddListener(() => {
                this.gameLogic.SellItem(item.Id, price);
            });
            SetButtonText(goItem, "Sell");
        } else {
            // hide price
            GetPriceText(goItem).enabled = false;
            // hide button
            button.gameObject.SetActive(false);
        }
    }
}
