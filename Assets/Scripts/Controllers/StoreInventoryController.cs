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
        gameLogic = GameStateManager.LogicInstance;

        base.Start();
    }

    public static void SetInventoryStorePrefab_Static(GameObject gameObject, InventoryItem item)
    {
        InventoryController.SetInventoryPrefab_Static(gameObject, item);
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

    public static void SetButtonText(GameObject gameObject, string buttonText)
    {
        GetButtonText(gameObject).text = buttonText;
    }
}
