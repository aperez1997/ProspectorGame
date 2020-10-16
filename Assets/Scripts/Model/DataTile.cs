using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// Contains data about the terrain tiles
[Serializable]
public class DataTile : ISerializationCallbackReceiver
{
    public Vector3Int CellLoc;

    public BiomeType Type;
    
    // Derived field that come from TerrainData
    public int BaseMoveCost { get; private set; }

    public Tile Tile { get; private set; }

    public DataTile(Vector3Int cellLocationIn, BiomeType type)
    {
        CellLoc = cellLocationIn;
        Type = type;
        LoadBiomeData();
    }

    public override string ToString()
    {
        return "DT[loc" + CellLoc.ToString() + "t(" + Type.ToString() + "]";
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        LoadBiomeData();
    }

    private void LoadBiomeData()
    {
        BaseMoveCost = -1; Tile = null;
        BiomeData data = BiomeDataLoader.LoadBiomeDataByType(Type);
        if (data is BiomeData)
        {
            BaseMoveCost = data.baseMoveCost;
            Tile = data.hexTile;
        }
    }
}
