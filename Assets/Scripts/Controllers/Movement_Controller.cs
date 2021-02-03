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
    public Text actionPointTxt;
    public Button rightBtn;
    public Button downRightBtn;
    public Button downLeftBtn;
    public Button leftBtn;
    public Button upLeftBtn;
    public Button upRightBtn;

    private Player player;
    private MovementUIHelper helper;
    private GameLogic gameLogic;

    public CostDescription GetCost(HexDirection direction) { return helper.GetMovementCostDescription(direction); }
    public void SetCost(HexDirection direction, CostDescription costDesc) { helper.SetMovementCost(direction, costDesc); }

    // Start is called before the first frame update
    void Start()
    {
        player = GameStateManager.LogicInstance.Player;
        player.OnActionPointsChanged += Player_OnActionPointsChanged;
        player.OnLocationChanged += Player_OnLocationChanged;

        gameLogic = GameStateManager.LogicInstance;

        //Debug.Log("Moving player to " + player.Location);
        UpdatePosition();       
        helper = new MovementUIHelper(rightBtn, downRightBtn, downLeftBtn, leftBtn, upLeftBtn, upRightBtn);
        UpdateMovementCosts(HexDirection.None);
    }

    /** UI Functions go here */

    // These functions called by button onclick
    public void MoveWest() { HandleMovement(HexDirection.West); }
    public void MoveSouthWest() { HandleMovement(HexDirection.SouthWest); }
    public void MoveSouthEast() { HandleMovement(HexDirection.SouthEast); }
    public void MoveEast() { HandleMovement(HexDirection.East); }
    public void MoveNorthEast() { HandleMovement(HexDirection.NorthEast); }
    public void MoveNorthWest() { HandleMovement(HexDirection.NorthWest); }

    // Update is called once per frame
    void Awake()
    {

    }

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
        actionPointTxt.text = player.ActionPoints.ToString() + "/" + player.ActionPointsMax.ToString();

        foreach (MovementUIData data in helper.data)
        {
            UpdateButtonUI(data.Button, data.CostDesc);
        }
    }

    // Updates a single movement button, also controlling enabled state
    void UpdateButtonUI(Button button, CostDescription costDesc)
    {
        var cost = costDesc.TotalCost;
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
        button.GetComponentInChildren<Text>().text = costStr;
        button.GetComponentInChildren<ToolTipUIHelper>().text = toolTipStr;
        button.interactable = enabled;
    }

    /// <summary>
    /// Get the full cost description for the given cost
    /// </summary>
    public string GetMovementCostDescriptionText(CostDescription costDesc)
    {
        var strings = costDesc.CostItems.Select(item => item.Name + " " + item.Value.ToString());
        return string.Join("\n", strings);
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
            //Debug.Log("Found neighbor " + hdeNeighbor + "=" + tileNeighbor);

            var costDesc = gameLogic.GetMovementCost(tileAt, tileNeighbor);
            SetCost(hdeNeighbor, costDesc);
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

    private void Player_OnActionPointsChanged(object sender, IntStatChangeEventArgs e)
    {
        UpdateMovementUI();
        PopUpTextDriverV1.CreateStatChangePopUp(actionPointTxt.transform, e.Delta);
    }

    private void OnDestroy()
    {
        player.OnActionPointsChanged -= Player_OnActionPointsChanged;
        player.OnLocationChanged -= Player_OnLocationChanged;
    }
}
