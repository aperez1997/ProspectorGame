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
    public Button SkinningBtn;
    public TextMeshProUGUI SkinningCostText;

    public Button CookBtn;
    public TextMeshProUGUI CookCostText;

    public Button EatBtn;
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
        SetActionButton(canSkin, this.SkinningBtn, this.SkinningCostText);

        // cook button
        var canCook = this.gameLogic.CanCook(this.currentItem);
        SetActionButton(canCook, this.CookBtn, this.CookCostText);

        // eat button
        var canEat = this.gameLogic.CanEat(this.currentItem);
        SetActionButton(canEat, this.EatBtn, this.EatCostText);
    }

    public void ActionSkin()
    {
        this.gameLogic.Skin(this.currentItem);
        // close up because we probably got rid of the item we were skinning
        CloseInfoPanel();
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

    protected static void SetActionButton(ActionCheckItem check, Button button, TextMeshProUGUI text)
    {
        if (check.IsApplicableToItem) {
            GUIUtils.UpdateActionButtonCost(text, check.IsApplicableToItem, check.Cost.Sum);
            button.interactable = check.IsAble;
            button.gameObject.SetActive(true);
        } else {
            button.gameObject.SetActive(false);
        }
    }
}