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
    public MovementUIData[] data
    {
        get { return dataDict.Values.ToArray(); }
    }

    private Dictionary<HexDirection, MovementUIData> dataDict = new Dictionary<HexDirection, MovementUIData>();

    public MovementUIHelper(Button east, Button southEast, Button southWest, Button west, Button northWest, Button northEast)
    {
        MovementUIData uIData;

        uIData = new MovementUIData(HexDirection.East, SumDescription.Empty, east);
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.SouthEast, SumDescription.Empty, southEast);
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.SouthWest, SumDescription.Empty, southWest);
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.West, SumDescription.Empty, west);
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.NorthWest, SumDescription.Empty, northWest);
        dataDict.Add(uIData.Hde, uIData);

        uIData = new MovementUIData(HexDirection.NorthEast, SumDescription.Empty, northEast);
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

    public void SetMovementCost(HexDirection hde, SumDescription costDesc)
    {
        dataDict.TryGetValue(hde, out MovementUIData data);
        data.Cost = costDesc.Sum;
        data.CostDesc = costDesc;
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

    public MovementUIData(HexDirection hde, SumDescription costDesc, Button button)
    {
        this.Hde = hde;
        this.Cost = costDesc.Sum;
        this.CostDesc = costDesc;
        this.Button = button;
    }
}
