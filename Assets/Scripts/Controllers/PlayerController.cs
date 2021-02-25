using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Actions related stuffs
public class PlayerController : MonoBehaviour
{
    // game data
    private Player player;
    private Inventory inventory;
    private GameStateMeta gameStateMeta;
    private GameLogic gameLogic;

    // TopBar UI
    public TextMeshProUGUI HealthPointTxt;
    public TextMeshProUGUI NourishmentTxt;
    public TextMeshProUGUI ActionPointTxt;
    public SimpleTooltip ActionPointHelper;
    public TextMeshProUGUI DateText;

    // Actions UI
    public TonyButton SleepBtn;

    public TonyButton TownBtn;

    public TonyButton ForageBtn;
    public TextMeshProUGUI ForageCost;
    public TextMeshProUGUI ForageChance;

    public TonyButton HuntBtn;
    public TextMeshProUGUI HuntCost;
    public TextMeshProUGUI HuntChance;

    public TonyButton PanForGoldBtn;
    public TextMeshProUGUI PanForGoldCost;
    public TextMeshProUGUI PanForGoldChance;

    // Start is called before the first frame update
    void Start()
    {
        this.gameLogic = GameStateManager.LogicInstance;
        this.player = GameStateManager.LogicInstance.Player;
        this.inventory = this.player.Inventory;

        this.player.OnHealthChanged += Player_OnHealthChanged;
        this.player.OnNourishmentChanged += Player_OnNourishmentChanged;
        this.player.OnActionPointsChanged += Player_OnAPChanged;
        this.player.OnLocationChanged += Player_OnLocationChanged;
        this.inventory.OnItemListChanged += Inventory_OnItemListChanged;

        this.gameStateMeta = GameStateManager.LogicInstance.GameStateMeta;
        this.gameStateMeta.OnGameDateChanged += GameStateMeta_OnDateChanged;

        UpdateHealthUI();
        UpdateNourishmentUI();
        UpdateAPUI();
        UpdateActionsUI();
        UpdateDateUI();

        // add clicks
        SleepBtn.onClick.AddListener(ActionSleep);
        TownBtn.onClick.AddListener(ActionVisitGeneralStore);
        ForageBtn.onClick.AddListener(ActionForage);
        HuntBtn.onClick.AddListener(ActionHunt);
        PanForGoldBtn.onClick.AddListener(ActionPanForGold);
    }

    void UpdateHealthUI()
    {
        HealthPointTxt.text = this.player.Health.ToString()
            + "/" + this.player.HealthMax.ToString();
    }

    void UpdateNourishmentUI()
    {
        NourishmentTxt.text = this.player.Nourishment.ToString()
            + "/" + this.player.NourishmentMax.ToString();
    }

    void UpdateAPUI()
    {
        var sumDesc = this.gameLogic.GetPlayerMaxActionPointsSum();
        this.ActionPointTxt.text = this.player.ActionPoints.ToString()
            + "/" + sumDesc.Sum.ToString();
        this.ActionPointHelper.infoLeft = GUIUtils.GetSumDescriptionDisplayString(sumDesc);
    }

    void UpdateDateUI()
    {
        var date = gameStateMeta.GameDate;
        var dayOfWeek = date.ToString("ddd");
        var month = date.ToString("MMM", CultureInfo.InvariantCulture);
        var dayOfMonth = date.Day.ToString();
        var yearStr = date.Year.ToString();

        this.DateText.text = string.Format("{0,3} {1} {2}, {3}", dayOfWeek, month, dayOfMonth, yearStr);
    }

    void UpdateActionsUI()
    {
        WorldTile tileAt = LoadPlayerTile();
        SleepBtn.interactable = tileAt.CanCamp;

        // Town
        TownBtn.interactable = tileAt.HasFeature(TileFeatureType.Town);

        // Forage
        bool canForage = gameLogic.CanForage(out int forageChance);
        ForageBtn.interactable = canForage;
        UpdateActionChanceText(ForageChance, forageChance);
        UpdateActionCostText(ForageCost, canForage, gameLogic.GetForageCost());

        // Hunt
        bool canHunt= gameLogic.CanHunt(out int huntChance);
        HuntBtn.interactable = canHunt;
        UpdateActionChanceText(HuntChance, huntChance);
        UpdateActionCostText(HuntCost, canHunt, gameLogic.GetHuntCost());      

        // Pan for Gold
        bool canPan = gameLogic.CanPanForGold(out int panChance);
        PanForGoldBtn.interactable = canPan;
        UpdateActionChanceText(PanForGoldChance, panChance);
        UpdateActionCostText(PanForGoldCost, canPan, gameLogic.GetPanForGoldCost());
    }

    void UpdateActionChanceText(TextMeshProUGUI chanceText, int chance)
    {
        GUIUtils.UpdateActionButtonChance(chanceText, chance);
    }

    void UpdateActionCostText(TextMeshProUGUI costText, bool allowed, int cost)
    {
        GUIUtils.UpdateActionButtonCost(costText, allowed, cost);
    }

    protected WorldTile LoadPlayerTile()
    {
        return GameStateManager.LogicInstance.GetTileForPlayerLocation();
    }

    public void ActionSleep()
    {
        gameLogic.Sleep();
    }

    public void ActionForage()
    {
        var rv = gameLogic.Forage();
        PopUpTextDriverV1.CreateSuccessFailurePopUp(ForageBtn.transform, rv);
    }

    public void ActionHunt()
    {
        var rv = gameLogic.Hunt();
        PopUpTextDriverV1.CreateSuccessFailurePopUp(HuntBtn.transform, rv);
    }

    public void ActionPanForGold()
    {
        var rv = gameLogic.PanForGold();
        PopUpTextDriverV1.CreateSuccessFailurePopUp(PanForGoldBtn.transform, rv);
    }

    public void ActionVisitGeneralStore()
    {
        SceneController.LoadGeneralStore_Static();
    }

    private void Player_OnHealthChanged(object sender, IntStatChangeEventArgs e)
    {
        UpdateHealthUI();
        PopUpTextDriverV1.CreateStatChangePopUp(HealthPointTxt.transform, e.Delta);
    }

    private void Player_OnNourishmentChanged(object sender, IntStatChangeEventArgs e)
    {
        UpdateNourishmentUI();
        PopUpTextDriverV1.CreateStatChangePopUp(NourishmentTxt.transform, e.Delta);
    }

    private void Player_OnAPChanged(object sender, IntStatChangeEventArgs e)
    {
        UpdateAPUI();
        PopUpTextDriverV1.CreateStatChangePopUp(ActionPointTxt.transform, e.Delta);

        UpdateActionsUI();
    }

    private void Player_OnLocationChanged(object sender, EventArgs e)
    {
        UpdateActionsUI();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        // primarily needed for ammo changes, which may happen after all other subscribed changes
        UpdateActionsUI();
    }

    private void GameStateMeta_OnDateChanged(object sender, EventArgs e)
    {
        UpdateDateUI();
    }

    private void OnDestroy()
    {
        // cleanup or bad things can happen
        player.OnHealthChanged -= Player_OnHealthChanged;
        player.OnNourishmentChanged -= Player_OnNourishmentChanged;
        player.OnActionPointsChanged -= Player_OnAPChanged;
        player.OnLocationChanged -= Player_OnLocationChanged;
        inventory.OnItemListChanged -= Inventory_OnItemListChanged;

        gameStateMeta.OnGameDateChanged -= GameStateMeta_OnDateChanged;
    }
}
