using UnityEngine;
using UnityEngine.Tilemaps;

public class DataTile
{
    public Vector3Int WorldLocation { get; }

    public TileBase TileBase { get; }

    public TerrainType Type { get; }

    public int Cost { get; }

    public DataTile(Vector3Int worldLocation, TileBase tileBase, TerrainType type, int cost)
    {
        WorldLocation = worldLocation;
        TileBase = tileBase;
        Type = type;
        Cost = cost;
    }

    public override string ToString()
    {
        return "DataTile[loc" + WorldLocation.ToString() + "t(" + Type.ToString() + ")c:" + Cost + "]";
    }
}
