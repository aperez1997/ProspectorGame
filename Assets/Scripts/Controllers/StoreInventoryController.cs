using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// abstract controller class for a store inventory.
/// Includes things like a price text and sell button
/// Use with the "pfInventoryItem-Store Variant" prefab
/// </summary>
abstract public class StoreInventoryController : InventoryController
{
    protected GameLogic gameLogic;

    protected override void Start()
    {
        // get these from singleton. Is this the right way?
        this.gameLogic = GameStateManager.LogicInstance;

        base.Start();
    }

    public static TextMeshProUGUI GetPriceText(GameObject goItem)
    {
        return Utils.FindInChildren(goItem, "Price").GetComponent<TextMeshProUGUI>();
    }

    public static void SetPriceText(GameObject goItem, int cost)
    {
        GetPriceText(goItem).text = cost > 0 ? GetMoneyString(cost) : string.Empty;
    }

    public static Button GetActionButton(GameObject goItem)
    {
        return goItem.GetComponentInChildren<Button>();
    }

    public static TextMeshProUGUI GetButtonText(GameObject goItem)
    {
        return GetActionButton(goItem).GetComponentInChildren<TextMeshProUGUI>();
    }

    public static void SetButtonText(GameObject goItem, string buttonText)
    {
        GetButtonText(goItem).text = buttonText;
    }
}
