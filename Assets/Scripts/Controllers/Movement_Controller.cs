using System;
using System.Collections.Generic;
using System.Linq;
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

    private MovementUIHelper helper;
    private Player player;

    int GetCost(HexDirection direction) { return helper.GetMovementCost(direction); }
    void SetCost(HexDirection direction, int cost) { helper.SetMovementCost(direction, cost); }

    // Start is called before the first frame update
    void Start()
    {
        player = GameState.Instance.Player;
        player.OnActionPointsChanged += Player_OnActionPointsChanged;
        player.OnLocationChanged += Player_OnLocationChanged;

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
    void Update()
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
        actionPointTxt.text = player.ActionPoints.ToString();

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
        } else if (cost > player.ActionPoints){
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
        player.UseActionPoints(cost);        

        // update internal costs matrix and UI 
        UpdateMovementCosts(direction);
    }

    // Update movement costs for the buttons based on the current location and lastDirection moved
    public void UpdateMovementCosts(HexDirection lastDirection)
    {
        Vector3Int posAt = player.GetCellPosition();

        DataTile dataTileAt = WorldMapLoader.Instance.GetDataTileAtLocation(posAt);
        if (dataTileAt == null){
            throw new System.Exception("Could not find dateTile for pos " + posAt.ToString());
        }

        // get neighbor vectors
        Dictionary<HexDirection, Vector3Int> neighbors = HexDirectionUtil.GetNeighborWorldVectors(posAt);

        // TODO Set all this up during map load as a property
        // lookup each neighbor and set it's cost
        foreach (KeyValuePair<HexDirection, Vector3Int> neighborPair in neighbors)
        {
            HexDirection hdeNeighbor = neighborPair.Key;
            Vector3Int posNeighbor = neighborPair.Value;
            DataTile dataTileNeighbor = WorldMapLoader.Instance.GetDataTileAtLocation(posNeighbor);
            if (dataTileNeighbor == null)
            {
                Debug.LogError("Missing datatile at going pos " + posNeighbor.ToString());
                continue;
            }
            // TODO: cost should probably come from a "cost engine". It should eventually be more than the raw terrain cost
            int costNeighbor = dataTileNeighbor.Cost; 
            SetCost(hdeNeighbor, costNeighbor);            
        }

        UpdateMovementUI();
    }

    private void Player_OnLocationChanged(object sender, EventArgs e)
    {
        UpdatePosition();
    }

    private void Player_OnActionPointsChanged(object sender, EventArgs e)
    {
        UpdateMovementUI();
    }
}
