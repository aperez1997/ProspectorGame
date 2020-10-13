using System;
using UnityEngine;

// Contains data about the terrain tiles
[Serializable]
public class DataTile
{
    public Vector3Int CellLoc;

    public TerrainType Type;

    public int Cost;

    public DataTile(Vector3Int cellLocationIn, TerrainType type, int cost)
    {
        CellLoc = cellLocationIn;
        Type = type;
        Cost = cost;
    }

    public override string ToString()
    {
        return "DT[loc" + CellLoc.ToString() + "t(" + Type.ToString() + ")c:" + Cost + "]";
    }
}
