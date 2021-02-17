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
    public TextMeshProUGUI SkinningCost;

    // private
    private InventoryItem currentItem;
    private GameLogic gameLogic;

    protected override void Start()
    {
        // get these from singleton. Is this the right way?
        gameLogic = GameStateManager.LogicInstance;
        Inventory = gameLogic.Inventory;

        base.Start();

        InfoPanel.SetActive(false);
    }

    protected override void SetPrefabDetails(GameObject gameObject, InventoryItem item)
    {
        base.SetPrefabDetails(gameObject, item);

        // disable tooltip helper since we're using the info panel instead
        var helper = gameObject.GetComponent<ToolTipUIHelper>();
        helper.enabled = false;

        // toggle infoPanel on click
        var button = gameObject.AddComponent<Button>();
        void onclick()
        {
            ToggleInfoPanel(item);
        };
        button.onClick.AddListener(onclick);
    }

    protected void ToggleInfoPanel(InventoryItem item)
    {
        if (currentItem == item) {
            CloseInfoPanel();
        } else {
            OpenInfoPanel(item);
        }
    }

    protected void OpenInfoPanel(InventoryItem item)
    {
        currentItem = item;
        InfoPanel.SetActive(true);
        UpdateInfoPanelUI();
    }

    public void CloseInfoPanel()
    {
        currentItem = null;
        InfoPanel.SetActive(false);
    }

    public void UpdateInfoPanelUI()
    { 
        InfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = currentItem.Name
            + "\n" + currentItem.Description;

        var canSkin = gameLogic.CanSkin(currentItem, out bool isApplicable);
        if (isApplicable) {
            GUIUtils.UpdateActionButtonCost(SkinningCost, isApplicable, gameLogic.GetSkinningCost());
            SkinningBtn.interactable = canSkin;
            SkinningBtn.gameObject.SetActive(true);
        } else {
            SkinningBtn.gameObject.SetActive(false);
        }
    }

    public void ActionSkin()
    {
        gameLogic.Skin(currentItem);
        // close up because we probably got rid of the item we were skinning
        CloseInfoPanel();
    }
}