using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for displaying the Player's inventory. Displays items
/// </summary>
public class PlayerInventoryController : InventoryController
{
    // UI - has the info panel which is needed for item clicks
    public ItemDetailsController ItemDetailsController;

    // private
    private GameLogic gameLogic;

    protected override void Start()
    {
        // get these from singleton. Is this the right way?
        this.gameLogic = GameStateManager.LogicInstance;
        this.Inventory = gameLogic.Inventory;

        base.Start();
    }

    protected override void SetPrefabDetails(GameObject goItem, InventoryItem item)
    {
        base.SetPrefabDetails(goItem, item);

        // disable tooltip helper since we're using the info panel instead
        var helper = goItem.GetComponent<ToolTipUIHelper>();
        helper.enabled = false;

        // toggle infoPanel on click
        var button = goItem.AddComponent<Button>();
        void onclick()
        {
            this.ItemDetailsController.ToggleInfoPanel(item);
        };
        button.onClick.AddListener(onclick);
    }
}