using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState : ISerializationCallbackReceiver
{
    public static GameState Instance { get; set; }

    public Player Player;

    public Inventory Inventory { get { return Player.Inventory; } }

    // The world
    [field:SerializeField] public List<DataTile> DataTileList { get; private set; }

    private Dictionary<Vector3Int, DataTile> DataTileDict;

    public GameLogic GameLogic { get; private set; }

    public GameState(Player player, List<DataTile> dataTileList)
    {
        this.Player = player;
        this.DataTileList = dataTileList;
        Init();
    }

    private void Init()
    {
        // we need to create a new one of these after deserialize.
        DataTileDict = new Dictionary<Vector3Int, DataTile>(0);
        // Data Tile Dictionary
        foreach (DataTile dataTile in DataTileList)
        {
            //Debug.Log("Adding tile at " + dataTile.CellLoc.ToString());
            DataTileDict.Add(dataTile.CellLoc, dataTile);
        }
        //Debug.Log("Dictionary has " + DataTileDict.Count + " keys");

        // Set Neighbors for all dataTiles
        foreach (DataTile dataTile in DataTileList){ 
            Dictionary<HexDirection, Vector3Int> neighborVectors = 
                HexDirectionUtil.GetNeighborWorldVectors(dataTile.CellLoc);

            // lookup each neighbor and set it in a dictionary
            Dictionary<HexDirection, DataTile> neighborTiles = new Dictionary<HexDirection, DataTile>();
            foreach (var item in neighborVectors)
            {
                HexDirection hdeNeighbor = item.Key;
                Vector3Int posNeighbor = item.Value;
                DataTile dataTileNeighbor = GetDataTileAtLocation(posNeighbor);
                if (!(dataTileNeighbor is DataTile))
                {
                    Debug.LogError("Missing datatile at pos " + posNeighbor.ToString());
                    continue;
                }
                //Debug.Log("Found neighbor " + hdeNeighbor + "=" + dataTileNeighbor);
                neighborTiles.Add(hdeNeighbor, dataTileNeighbor);
            }
            dataTile.Neighbors = neighborTiles;
        }

        /*
         * At some point, this could be injectable. The idea is that this class could be interchanged
         * depending on some conditions, like the type of game being played or add-ons
         * However, knowing how to build this thing should probably be in some other class,
         * like the gameStateManager. In which case, maybe it needs an external setter
         * Either that, or GameState that we use in-game gets created from a serialized data-struct 
         * which would avoid the DI issues
         */
        GameLogic = new GameLogic(this);
    }

    public DataTile GetDataTileAtLocation(Vector3Int location)
    {
        _ = DataTileDict.TryGetValue(location, out DataTile dataTile);
        return dataTile;
    }

    public DataTile GetTileForPlayerLocation(Player player)
    {
        Vector3Int posAt = player.GetCellPosition();
        DataTile dataTileAt = GetDataTileAtLocation(posAt);
        if (!(dataTileAt is DataTile))
        {
            string msg = "Could not find dateTile for pos " + posAt.ToString();
            throw new System.Exception(msg);
        }

        return dataTileAt;
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize(){ Init(); }

    public override string ToString()
    {
        return "GameState[P:" + Player.ToString() +"]";
    }
}
