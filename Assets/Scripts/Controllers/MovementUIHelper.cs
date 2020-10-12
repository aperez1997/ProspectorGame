using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


// Holds UI and data for for each direction the player can go
// consider breaking out the direction->cost aspect into another class? For pathfinding multiple moves
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

        uIData = new MovementUIData(HexDirection.East, 0, east);
        dataDict.Add(uIData.hde, uIData);

        uIData = new MovementUIData(HexDirection.SouthEast, 0, southEast);
        dataDict.Add(uIData.hde, uIData);

        uIData = new MovementUIData(HexDirection.SouthWest, 0, southWest);
        dataDict.Add(uIData.hde, uIData);

        uIData = new MovementUIData(HexDirection.West, 0, west);
        dataDict.Add(uIData.hde, uIData);

        uIData = new MovementUIData(HexDirection.NorthWest, 0, northWest);
        dataDict.Add(uIData.hde, uIData);

        uIData = new MovementUIData(HexDirection.NorthEast, 0, northEast);
        dataDict.Add(uIData.hde, uIData);
    }

    public int GetMovementCost(HexDirection hde)
    {
        bool rv = dataDict.TryGetValue(hde, out MovementUIData data);
        if (rv){
            return data.cost;
        }
        return -1;
    }

    public void SetMovementCost(HexDirection hde, int cost)
    {
        dataDict.TryGetValue(hde, out MovementUIData data);
        data.cost = cost;       
    }

    public Button GetButtom(HexDirection hde)
    {
        dataDict.TryGetValue(hde, out MovementUIData data);
        return data.button;
    }
}

// tODO: should probably be it's own file
public class MovementUIData
{
    public HexDirection hde;
    public int cost;
    public Button button;

    public MovementUIData(HexDirection hde, int cost, Button button)
    {
        this.hde = hde;
        this.cost = cost;
        this.button = button;
    }
}
