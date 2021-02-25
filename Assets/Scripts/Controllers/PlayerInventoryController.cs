using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for displaying the Player's inventory
/// </summary>
public class PlayerInventoryController : InventoryController
{
    // UI
    public GameObject InfoPanel;

    // buttons
    public TonyButton SkinningBtn;
    public TextMeshProUGUI SkinningCostText;

    public TonyButton CookBtn;
    public TextMeshProUGUI CookCostText;

    public TonyButton EatBtn;
    public TextMeshProUGUI EatCostText;

    // private
    private InventoryItem currentItem;
    private GameLogic gameLogic;

    protected override void Start()
    {
        // get these from singleton. Is this the right way?
        this.gameLogic = GameStateManager.LogicInstance;
        this.Inventory = gameLogic.Inventory;

        base.Start();

        this.InfoPanel.SetActive(false);

        // set clicks
        SkinningBtn.onClick.AddListener(ActionSkin);
        CookBtn.onClick.AddListener(ActionCook);
        EatBtn.onClick.AddListener(ActionEat);
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
            ToggleInfoPanel(item);
        };
        button.onClick.AddListener(onclick);
    }

    protected void ToggleInfoPanel(InventoryItem item)
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
        this.InfoPanel.SetActive(true);
        UpdateInfoPanelUI();
    }

    public void CloseInfoPanel()
    {
        this.currentItem = null;
        this.InfoPanel.SetActive(false);
    }

    public void UpdateInfoPanelUI()
    {
        this.InfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = this.currentItem.Name
            + "\n" + this.currentItem.Description;

        // skin button
        var canSkin = this.gameLogic.CanSkin(this.currentItem);
        SetActionButtonState(canSkin, this.SkinningBtn, this.SkinningCostText);

        // cook button
        var canCook = this.gameLogic.CanCook(this.currentItem);
        SetActionButtonState(canCook, this.CookBtn, this.CookCostText);

        // eat button
        var canEat = this.gameLogic.CanEat(this.currentItem);
        SetActionButtonState(canEat, this.EatBtn, this.EatCostText);
    }

    public void ActionSkin()
    {
        // I want to switch to this pattern, but need a "actionResult" class instead of using check.
        // otherwise we have no way to communicate what happened (if that's necessary)
        var check = this.gameLogic.Skin(this.currentItem);
        if (check.IsAble) {
            // close up because we probably got rid of the item we were skinning
            CloseInfoPanel();
        } else {
            // TODO: move this behavior into GUI Utils once everything is settled
            PopUpTextDriverV1.CreateFailurePopUp(this.SkinningBtn.transform, check.Reason);
        }
    }

    public void ActionCook()
    {
        this.gameLogic.Cook(this.currentItem);
        // close up because we probably got rid of the item we cooked
        CloseInfoPanel();
    }

    public void ActionEat()
    {
        this.gameLogic.EatFood(this.currentItem);
        // close up because we probably got rid of the item we ate
        CloseInfoPanel();
    }

    /// <summary>
    /// Set action button state based on check
    /// </summary>
    protected static void SetActionButtonState(ActionCheckItem check, TonyButton button, TextMeshProUGUI text)
    {
        GUIUtils.UpdateItemActionButtonState(button, text, check);
    }
}