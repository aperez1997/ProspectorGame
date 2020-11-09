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
    [field:SerializeField] public List<WorldTile> WorldTileList { get; private set; }

    private Dictionary<Vector3Int, WorldTile> WorldTileDict;

    public GameLogic GameLogic { get; private set; }

    public GameState(Player player, List<WorldTile> worldTileList)
    {
        this.Player = player;
        this.WorldTileList = worldTileList;
        Init();
    }

    private void Init()
    {
        // we need to create a new one of these after deserialize.
        WorldTileDict = new Dictionary<Vector3Int, WorldTile>(0);
        // Data Tile Dictionary
        foreach (WorldTile worldTile in WorldTileList)
        {
            //Debug.Log("Adding tile at " + worldTile.CellLoc.ToString());
            WorldTileDict.Add(worldTile.CellLoc, worldTile);
        }
        //Debug.Log("Dictionary has " + WorldTileDict.Count + " keys");

        // Set Neighbors for all worldTile
        foreach (WorldTile tile in WorldTileList){ 
            Dictionary<HexDirection, Vector3Int> neighborVectors = 
                HexDirectionUtil.GetNeighborWorldVectors(tile.CellLoc);

            // lookup each neighbor and set it in a dictionary
            Dictionary<HexDirection, WorldTile> neighborTiles = new Dictionary<HexDirection, WorldTile>();
            foreach (var item in neighborVectors)
            {
                HexDirection hdeNeighbor = item.Key;
                Vector3Int posNeighbor = item.Value;
                WorldTile worldTileNeighbor = GetTileAtLocation(posNeighbor);
                if (!(worldTileNeighbor is WorldTile)){ continue; }
                //Debug.Log("Found neighbor " + hdeNeighbor + "=" + worldTileNeighbor);
                neighborTiles.Add(hdeNeighbor, worldTileNeighbor);
            }
            tile.Neighbors = neighborTiles;
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

    public WorldTile GetTileAtLocation(Vector3Int location)
    {
        _ = WorldTileDict.TryGetValue(location, out WorldTile tile);
        return tile;
    }

    public WorldTile GetTileForPlayerLocation(Player player)
    {
        Vector3Int posAt = player.GetCellPosition();
        WorldTile tileAt = GetTileAtLocation(posAt);
        if (!(tileAt is WorldTile))
        {
            string msg = "Could not find tile for pos " + posAt.ToString();
            throw new System.Exception(msg);
        }

        return tileAt;
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize(){ Init(); }

    public override string ToString()
    {
        return "GameState[P:" + Player.ToString() +"]";
    }
}
