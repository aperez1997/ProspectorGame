using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controller for the item detail popup. Displays action buttons
/// </summary>
public class ItemDetailsController : ContainerDisplayController<ItemActionButton>
{
    // UI
    public GameObject PopUp;

    // state
    protected GameLogic gameLogic;
    protected InventoryItem currentItem;

    protected override void Start()
    {
        // get these from singleton. Is this the right way?
        this.gameLogic = GameStateManager.LogicInstance;
        base.Start();

        this.PopUp.SetActive(false);
    }

    protected override string GetItemContainerName()
    {
        return "Grid-Actions";
    }

    protected override ReadOnlyCollection<ItemActionButton> GetItemList()
    {
        return this.GetActionsForItem();
    }

    protected override void SetPrefabDetails(GameObject goItem, ItemActionButton item)
    {
        var button = goItem.GetComponentInChildren<TonyButton>();
        var actionText = button.GetComponentsInChildren<TextMeshProUGUI>()[0];
        actionText.text = item.Name;

        // OnClick has to be a UnityAction that takes no arguments, but the Action methods need
        // to have the gameObject passed to them
        UnityAction actionWrapper = () => { item.Action(goItem); };
        button.onClick.AddListener(actionWrapper);

        var costText = button.GetComponentsInChildren<TextMeshProUGUI>()[1];
        // have to do this so the disabled state can take effect
        goItem.SetActive(true);
        GUIUtils.UpdateItemActionButtonState(button, costText, item.Check);
    }

    /****** panel stuff ******/

    public void ToggleInfoPanel(InventoryItem item)
    {
        if (this.currentItem == item) {
            CloseInfoPanel();
        } else {
            OpenInfoPanel(item);
        }
    }

    protected void OpenInfoPanel(InventoryItem item)
    {
        this.currentItem = item;
        this.PopUp.SetActive(true);
        UpdateInfoPanelUI();
    }

    public void CloseInfoPanel()
    {
        this.currentItem = null;
        this.PopUp.SetActive(false);
    }


    public void UpdateInfoPanelUI()
    {
        this.PopUp.GetComponentInChildren<TextMeshProUGUI>().text = this.currentItem.Name
            + "\n" + this.currentItem.Description;

        // action stuff
        UpdateUI();
    }

    public override void UpdateUI()
    {
        // only update if we have an inventoryItem set
        if (this.currentItem is InventoryItem) {
            base.UpdateUI();
        }
    }

    /****************************************
     * Actions
     ****************************************/

    /// <summary>
    /// Get the list of applicable action buttons for the given Item
    /// </summary>
    public ReadOnlyCollection<ItemActionButton> GetActionsForItem()
    {
        var item = this.currentItem;
        var buttons = new List<ItemActionButton>();

        // TODO: we can move the bulk of this into GameLogic if it returns an array of checks
        // as long as the check has the actionType and the button name, we should be able to set
        // everything else conditionally (i.e. using a switch statement)

        // skin
        var skinCheck = gameLogic.CanSkin(item);
        if (skinCheck.IsApplicableToItem) {
            buttons.Add(new ItemActionButton("Skin", skinCheck, ActionSkin));
        }

        // cook
        var cookCheck = gameLogic.CanCook(item);
        if (cookCheck.IsApplicableToItem) {
            buttons.Add(new ItemActionButton("Cook", cookCheck, ActionCook));
        }

        // eat
        var eatCheck = gameLogic.CanEat(item);
        if (eatCheck.IsApplicableToItem) {
            buttons.Add(new ItemActionButton("Eat", eatCheck, ActionEat));
        }

        return buttons.AsReadOnly();
    }

    public void ActionSkin(GameObject goButton)
    {
        var result = this.gameLogic.Skin(this.currentItem);
        if (result.IsSuccess) {
            CloseInfoPanel();
        } else {
            this.HandleActionFailure(goButton, result);
        }
    }

    public void ActionCook(GameObject goButton)
    {
        var result = this.gameLogic.Cook(this.currentItem);
        if (result.IsSuccess) {
            CloseInfoPanel();
        } else {
            this.HandleActionFailure(goButton, result);
        }
    }

    public void ActionEat(GameObject goButton)
    {
        var result = this.gameLogic.EatFood(this.currentItem);
        if (result.IsSuccess) {
            CloseInfoPanel();
        } else {
            this.HandleActionFailure(goButton, result);
        }
    }

    private void HandleActionFailure(GameObject goButton, ItemActionResult result)
    {
        // TODO: move this behavior into GUI Utils once everything is settled
        PopUpTextDriverV1.CreateFailurePopUp(goButton.transform, result.Reason);
    }
}
