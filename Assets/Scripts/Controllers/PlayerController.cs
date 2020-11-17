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
    private GameStateMeta gameStateMeta;
    private GameLogic gameLogic;

    // TopBar UI
    public Text healthPointTxt;
    public TextMeshProUGUI DateText;

    // Actions UI
    public Button CampBtn;

    public Button TownBtn;

    public Button ForageBtn;
    public TextMeshProUGUI ForageCost;
    public TextMeshProUGUI ForageChance;

    public Button HuntBtn;

    public Button PanForGoldBtn;
    public TextMeshProUGUI PanForGoldCost;
    public TextMeshProUGUI PanForGoldChance;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameState.Instance.GameLogic;
        player = GameState.Instance.Player;

        player.OnHealthChanged += Player_OnHealthChanged;
        player.OnLocationChanged += Player_OnLocationOrAPChanged;
        player.OnActionPointsChanged += Player_OnLocationOrAPChanged;

        gameStateMeta = GameState.Instance.GameStateMeta;
        gameStateMeta.OnGameDateChanged += GameStateMeta_OnDateChanged;

        UpdateHealthUI();
        UpdateActionsUI();
        UpdateDateUI();
    }

    void UpdateHealthUI()
    {
        healthPointTxt.text = player.Health.ToString() + "/" + Player.MAX_HEALTH.ToString();
    }

    void UpdateDateUI()
    {
        var date = gameStateMeta.GameDate;
        var dayOfWeek = date.ToString("ddd");
        var month = date.ToString("MMM", CultureInfo.InvariantCulture);
        var dayOfMonth = date.Day.ToString();
        var yearStr = date.Year.ToString();

        DateText.text = string.Format("{0,3} {1} {2}, {3}", dayOfWeek, month, dayOfMonth, yearStr);
    }

    void UpdateActionsUI()
    {
        WorldTile tileAt = LoadPlayerTile();
        CampBtn.interactable = tileAt.CanCamp;

        // Town
        TownBtn.interactable = tileAt.HasFeature(TileFeatureType.Town);

        // Forage
        bool canForage = gameLogic.CanForage(out int forageChance);
        ForageBtn.interactable = canForage;
        UpdateActionChanceText(ForageChance, forageChance);
        UpdateActionCostText(ForageCost, canForage, gameLogic.GetForageCost());

        // TODO: implement
        HuntBtn.interactable = false;

        // Pan for Gold
        bool canPan = gameLogic.CanPanForGold(out int panChance);
        PanForGoldBtn.interactable = canPan;
        UpdateActionChanceText(PanForGoldChance, panChance);
        UpdateActionCostText(PanForGoldCost, canPan, gameLogic.GetPanForGoldCost());
    }

    void UpdateActionChanceText(TextMeshProUGUI chanceText, int chance)
    {
        if (chance > 0) {
            chanceText.gameObject.SetActive(true);
            chanceText.text = chance.ToString() + "%";
            var color = Color.red;
            if (chance > 66) {
                color = Color.green;
            } else if (chance > 33) {
                color = Color.yellow;
            }
            chanceText.color = color;
        } else {
            chanceText.gameObject.SetActive(false);
        }
    }

    void UpdateActionCostText(TextMeshProUGUI costText, bool allowed, int cost)
    {
        costText.text = allowed ? cost.ToString() + " AP" : String.Empty;
    }

    protected WorldTile LoadPlayerTile()
    {
        return GameState.Instance.GetTileForPlayerLocation(player);
    }

    public void ActionCamp()
    {
        gameLogic.Camp();
    }

    public void ActionForage()
    {
        var rv = gameLogic.Forage();
        PopUpTextDriverV1.CreateSuccessFailurePopUp(ForageBtn.transform, rv);
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
        PopUpTextDriverV1.CreateStatChangePopUp(healthPointTxt.transform, e.Delta);
    }

    private void Player_OnLocationOrAPChanged(object sender, EventArgs e)
    {
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
        player.OnLocationChanged -= Player_OnLocationOrAPChanged;
        player.OnActionPointsChanged -= Player_OnLocationOrAPChanged;
        gameStateMeta.OnGameDateChanged -= GameStateMeta_OnDateChanged;
    }
}
