﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// player movement controller. Handles updating button UI based on costs, moving player, and managing remaining APs
// APs should probably move into some sort of "player state" object
// Cost resolution should come from some world map class
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

    int GetCost(HexDirection direction) { return helper.GetMovementCost(direction); }
    void SetCost(HexDirection direction, int cost) { helper.SetMovementCost(direction, cost); }

    // Start is called before the first frame update
    void Start()
    {
        player = GameState.Instance.Player;
        player.OnActionPointsChanged += Player_OnActionPointsChanged;
        player.OnLocationChanged += Player_OnLocationChanged;

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
            UpdateButtonUI(data.button, data.cost);
        }
    }

    // Updates a single movement button, also controlling enabled state
    void UpdateButtonUI(Button button, int cost)
    {
        string costStr = cost.ToString();
        bool enabled = true;
        if (cost < 0){
            costStr = "X";
            enabled = false;
        } else if (!player.HasEnoughActionPoints(cost)){
            enabled = false;
        }
        button.GetComponentInChildren<Text>().text = costStr;
        button.interactable = enabled;
    }

    // Called to move in the given direction
    public void HandleMovement(HexDirection direction)
    {
        // get cost
        int cost = GetCost(direction);
        Debug.Log("Button direction "+ direction.ToString() +" was pressed! Cost is " + cost);
        if (!player.HasEnoughActionPoints(cost))
        {
            Debug.LogWarning("Cannot move because not enough AP");
            return;
        }

        // change the player's position. This will cause an event to update UI
        Vector3Int newCellPos = HexDirectionUtil.TranslateVector3Int(player.GetCellPosition(), direction);
        player.SetLocation(newCellPos, direction);

        // Bookkeeping
        player.SpendActionPoints(cost);

        // update internal costs matrix and UI 
        UpdateMovementCosts(direction);
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

            // TODO: cost should probably come from game logic. It should eventually be more than the raw terrain cost
            int costNeighbor = tileNeighbor.BaseMoveCost;
            SetCost(hdeNeighbor, costNeighbor);
        }

        UpdateMovementUI();
    }

    protected WorldTile LoadPlayerTile()
    {
        return GameState.Instance.GetTileForPlayerLocation(player);
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
}
