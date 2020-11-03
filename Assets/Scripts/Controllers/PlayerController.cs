using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Actions related stuffs
public class PlayerController : MonoBehaviour
{
    private Player player;
    private GameLogic gameLogic;

    public Text healthPointTxt;

    public Button CampBtn;
    public Button TownBtn;
    public Button ForageBtn;
    public TextMeshProUGUI ForageCost;
    public Button HuntBtn;
    public Button PanForGoldBtn;
    public TextMeshProUGUI PanForGoldCost;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameState.Instance.GameLogic;
        player = GameState.Instance.Player;
        player.OnHealthChanged += Player_OnHealthChanged;
        player.OnLocationChanged += Player_OnLocationOrAPChanged;
        player.OnActionPointsChanged += Player_OnLocationOrAPChanged;
        UpdateHealthUI();
        UpdateActionsUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealthUI()
    {
        healthPointTxt.text = player.Health.ToString() + "/" + Player.MAX_HEALTH.ToString();
    }

    void UpdateActionsUI()
    {
        DataTile dataTileAt = LoadPlayerDataTile();
        CampBtn.interactable = dataTileAt.CanCamp;

        // Town
        TownBtn.interactable = (dataTileAt.Type == BiomeType.Town);

        // Forage
        bool canForage = gameLogic.CanForage();
        ForageBtn.interactable = canForage;
        UpdateActionCostText(ForageCost, canForage, gameLogic.GetForageCost());

        // TODO: implement
        HuntBtn.interactable = false;

        // Pan for Gold
        bool canPan = gameLogic.CanPanForGold();
        PanForGoldBtn.interactable = canPan;
        UpdateActionCostText(PanForGoldCost, canPan, gameLogic.GetPanForGoldCost());
    }

    void UpdateActionCostText(TextMeshProUGUI costText, bool allowed, int cost)
    {
        costText.text = allowed ? cost.ToString() : String.Empty;
    }

    protected DataTile LoadPlayerDataTile()
    {
        return GameState.Instance.GetTileForPlayerLocation(player);
    }

    public void ActionCamp()
    {
        gameLogic.Camp();
    }

    public void ActionForage()
    {
        gameLogic.Forage();
    }

    public void ActionPanForGold()
    {
        gameLogic.PanForGold();
    }

    public void ActionVisitGeneralStore()
    {
        SceneManager.LoadScene("GeneralStore", LoadSceneMode.Additive);
    }

    private void Player_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthUI();
    }

    private void Player_OnLocationOrAPChanged(object sender, EventArgs e)
    {
        UpdateActionsUI();
    }
}
