using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// player movement controller. Handles updating button UI based on costs, moving player, and managing AP display
/// /// </summary>
public class Movement_Controller : MonoBehaviour
{
    // World
    public Tilemap tilemap;

    // UI
    public GameObject rightBtn;
    public GameObject downRightBtn;
    public GameObject downLeftBtn;
    public GameObject leftBtn;
    public GameObject upLeftBtn;
    public GameObject upRightBtn;
    public Camera Camera;

    private Player player;
    private MovementUIHelper helper;
    private GameLogic gameLogic;

    public SumDescription GetCost(HexDirection direction) { return helper.GetMovementCostDescription(direction); }

    public void SetCost(HexDirection direction, WorldTile tileTo, SumDescription costDesc)
    {
        helper.SetMovementCost(direction, tileTo, costDesc);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameStateManager.LogicInstance.Player;
        player.OnHealthChanged += Player_OnStatChanged;
        player.OnNourishmentChanged += Player_OnStatChanged;
        player.OnActionPointsChanged += Player_OnStatChanged;
        player.OnLocationChanged += Player_OnLocationChanged;

        gameLogic = GameStateManager.LogicInstance;

        //Debug.Log("Moving player to " + player.Location);
        UpdatePosition();
        var tileAt = LoadPlayerTile();
        helper = new MovementUIHelper(rightBtn, downRightBtn, downLeftBtn, leftBtn, upLeftBtn, upRightBtn, tileAt);
        UpdateMovementCosts(HexDirection.None);

        // add clicks
        rightBtn.GetComponent<Button>().onClick.AddListener(MoveEast);
        downRightBtn.GetComponent<Button>().onClick.AddListener(MoveSouthEast);
        downLeftBtn.GetComponent<Button>().onClick.AddListener(MoveSouthWest);
        leftBtn.GetComponent<Button>().onClick.AddListener(MoveWest);
        upLeftBtn.GetComponent<Button>().onClick.AddListener(MoveNorthWest);
        upRightBtn.GetComponent<Button>().onClick.AddListener(MoveNorthEast);
    }

    /** UI Functions go here */

    // These functions called by button onclick
    public void MoveWest() { HandleMovement(HexDirection.West); }
    public void MoveSouthWest() { HandleMovement(HexDirection.SouthWest); }
    public void MoveSouthEast() { HandleMovement(HexDirection.SouthEast); }
    public void MoveEast() { HandleMovement(HexDirection.East); }
    public void MoveNorthEast() { HandleMovement(HexDirection.NorthEast); }
    public void MoveNorthWest() { HandleMovement(HexDirection.NorthWest); }

    void UpdatePosition()
    {
        // move player transform to the world position that corresponds to their cell position
        Vector3Int cellPos = player.GetCellPosition();
        Vector3 worldPos = tilemap.CellToWorld(cellPos);
        transform.position = worldPos;
    }

    // Updates the movement button text and action point display
    void UpdateMovementUI()
    {
        foreach (MovementUIData data in helper.data) {
            UpdateButtonUI(data);
        }
    }

    // Updates a single movement button, also controlling enabled state
    void UpdateButtonUI(MovementUIData data)
    {
        var costDesc = data.CostDesc;
        var cost = costDesc.Sum;
        string costStr = cost.ToString();
        string toolTipStr = GetMovementCostDescriptionText(costDesc);
        bool enabled = true;
        if (cost < 0) {
            costStr = "X";
            toolTipStr = "Unpassable";
            enabled = false;            
        } else if (!player.HasEnoughActionPoints(cost)) {
            enabled = false;
        }
        var button = data.Button;
        button.GetComponentInChildren<Text>().text = costStr;
        var simpleTooltip = button.GetComponentInChildren<SimpleTooltip>();
        simpleTooltip.infoLeft = toolTipStr;
        simpleTooltip.infoRight = data.Hde.ToString();
        button.GetComponentInChildren<Button>().interactable = enabled;

        //Debug.Log("setting button with cost " + costStr + " for tile " + worldTile.ToString());

        // move button to tile location
        Vector3 worldPos = tilemap.CellToWorld(data.WorldTile.CellLoc);
        button.transform.position = Camera.WorldToScreenPoint(worldPos);
    }

    /// <summary>
    /// Get the full cost description for the given cost
    /// </summary>
    public string GetMovementCostDescriptionText(SumDescription costDesc)
    {
        return GUIUtils.GetSumDescriptionDisplayString(costDesc);
    }

    // Called to move in the given direction
    public bool HandleMovement(HexDirection direction)
    {
        //Debug.Log("Button direction "+ direction.ToString() +" was pressed! Cost is " + cost);
        var tileAt = LoadPlayerTile();

        var rv = gameLogic.MovePlayer(tileAt, direction);

        // update internal costs matrix and UI 
        UpdateMovementCosts(direction);
        return rv;
    }

    // Update movement costs for the buttons based on the current location and lastDirection moved
    public void UpdateMovementCosts(HexDirection lastDirection)
    {
        WorldTile tileAt = LoadPlayerTile();
        //Debug.Log("New position " + tileAt);

        // lookup each neighbor and it's cost
        foreach (KeyValuePair<HexDirection, WorldTile> neighborPair in tileAt.Neighbors)
        {
            HexDirection hdeNeighbor = neighborPair.Key;
            WorldTile tileNeighbor = neighborPair.Value;


            var costDesc = gameLogic.GetMovementCost(tileAt, tileNeighbor);
            //Debug.Log("Found neighbor " + hdeNeighbor + "=" + tileNeighbor + " cost = " + costDesc.Sum);
            SetCost(hdeNeighbor, tileNeighbor, costDesc);
        }

        UpdateMovementUI();
    }

    protected WorldTile LoadPlayerTile()
    {
        return GameStateManager.LogicInstance.GetTileForPlayerLocation();
    }

    private void Player_OnLocationChanged(object sender, EventArgs e)
    {
        UpdatePosition();
    }

    private void Player_OnStatChanged(object sender, EventArgs e)
    {
        UpdateMovementUI();
    }

    private void OnDestroy()
    {
        player.OnHealthChanged -= Player_OnStatChanged;
        player.OnNourishmentChanged -= Player_OnStatChanged;
        player.OnActionPointsChanged -= Player_OnStatChanged;
        player.OnLocationChanged -= Player_OnLocationChanged;
    }
}
