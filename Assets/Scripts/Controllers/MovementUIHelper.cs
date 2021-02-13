using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Holds UI and data for for each direction the player can go.
/// Maintains a dictionary of direction -> MovementUIData, which is a mix of direction, cost, and UI
//  consider breaking out the direction->cost aspect into another class? For pathfinding multiple moves 
/// </summary>
public class MovementUIHelper
{
    public MovementUIData[] data {
        get { return dataDict.Values.ToArray(); }
    }

    private Dictionary<HexDirection, MovementUIData> dataDict = new Dictionary<HexDirection, MovementUIData>();

    public MovementUIHelper(Button east, Button southEast, Button southWest, Button west, Button northWest, Button northEast, WorldTile worldTile)
    {
        MovementUIData uIData;

        uIData = new MovementUIData(HexDirection.East, SumDescription.Empty, east, worldTile.GetNeighborInDirection(HexDirection.East));
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.SouthEast, SumDescription.Empty, southEast, worldTile.GetNeighborInDirection(HexDirection.SouthEast));
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.SouthWest, SumDescription.Empty, southWest, worldTile.GetNeighborInDirection(HexDirection.SouthWest));
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.West, SumDescription.Empty, west, worldTile.GetNeighborInDirection(HexDirection.West));
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.NorthWest, SumDescription.Empty, northWest, worldTile.GetNeighborInDirection(HexDirection.NorthWest));
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.NorthEast, SumDescription.Empty, northEast, worldTile.GetNeighborInDirection(HexDirection.NorthEast));
        dataDict.Add(uIData.Hde, uIData);
    }

    public SumDescription GetMovementCostDescription(HexDirection hde)
    {
        bool rv = dataDict.TryGetValue(hde, out MovementUIData data);
        if (rv) {
            return data.CostDesc;
        }
        return null;
    }

    public void SetMovementCost(HexDirection hde, WorldTile tileTo, SumDescription costDesc)
    {
        dataDict.TryGetValue(hde, out MovementUIData data);
        data.Cost = costDesc.Sum;
        data.CostDesc = costDesc;
        data.WorldTile = tileTo;
    }

    public Button GetButton(HexDirection hde)
    {
        dataDict.TryGetValue(hde, out MovementUIData data);
        return data.Button;
    }
}

// TODO: should probably be it's own file
public class MovementUIData
{
    public HexDirection Hde;
    public int Cost;
    public SumDescription CostDesc;     
    public Button Button;
    public WorldTile WorldTile;

    public MovementUIData(HexDirection hde, SumDescription costDesc, Button button, WorldTile tile)
    {
        this.Hde = hde;
        this.Cost = costDesc.Sum;
        this.CostDesc = costDesc;
        this.Button = button;
        this.WorldTile = tile;
    }
}
