using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class for a Player Inventory used in Store context (sell)
public class PlayerStoreInventoryController : InventoryController
{
    protected override void SetInventoryPrefab(GameObject gameObject, InventoryItem item)
    {
        SetInventoryStorePrefab_Static(gameObject, item);

        // set price
        bool canSell = gameLogic.CanSell(item.type);
        int price = gameLogic.GetSellPrice(item.type);
        SetPriceText(gameObject, price);

        // setup button
        var button = GetActionButton(gameObject);
        button.interactable = canSell;
        button.onClick.AddListener(() => {
            gameLogic.SellItem(item.type, price);
        });
        var btnText = PlayerStoreInventoryController.GetButtonText(gameObject);
        btnText.text = "Sell";
    }

    public static void SetInventoryStorePrefab_Static(GameObject gameObject, InventoryItem item)
    {
        SetInventoryPrefab_Static(gameObject, item);
    }

    public static TextMeshProUGUI GetPriceText(GameObject gameObject)
    {
        return Utils.FindInChildren(gameObject, "Price").GetComponent<TextMeshProUGUI>();
    }

    public static void SetPriceText(GameObject gameObject, int cost)
    {
        GetPriceText(gameObject).text = cost > 0 ? "$" + cost.ToString() : string.Empty;
    }

    public static Button GetActionButton(GameObject gameObject)
    {
        return gameObject.GetComponentInChildren<Button>();
    }

    public static TextMeshProUGUI GetButtonText(GameObject gameObject)
    {
        return GetActionButton(gameObject).GetComponentInChildren<TextMeshProUGUI>();
    }
}
