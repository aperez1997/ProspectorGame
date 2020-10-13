using System;
using UnityEngine;

// Contains data about the terrain tiles
[Serializable]
public class DataTile
{
    public Vector3Int CellLocation;

    public TerrainType Type;

    public int Cost;

    public DataTile(Vector3Int cellLocationIn, TerrainType type, int cost)
    {
        CellLocation = cellLocationIn;
        Type = type;
        Cost = cost;
    }

    public override string ToString()
    {
        return "DT[loc" + CellLocation.ToString() + "t(" + Type.ToString() + ")c:" + Cost + "]";
    }
}
