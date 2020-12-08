using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the world map in the game
/// </summary>
[Serializable]
public class WorldMap : ISerializationCallbackReceiver
{
    /// <summary>
    /// The world map tiles, core element of the world map
    /// </summary>
    [field: SerializeField] public List<WorldTile> WorldTileList { get; private set; }

    public event EventHandler<WorldTileChangeEventArgs> OnWorldTilesChanged;

    // Derived from world tiles, Location => WorldTile mapping. Used for lookups
    private Dictionary<Vector3Int, WorldTile> WorldTileDict;

    public WorldMap(List<WorldTile> worldTileList)
    {
        WorldTileList = worldTileList;
        Init();
    }

    private void Init()
    {
        // we need to create a new one of these after deserialize.
        WorldTileDict = new Dictionary<Vector3Int, WorldTile>(0);
        // Data Tile Dictionary
        foreach (WorldTile worldTile in WorldTileList) {
            //Debug.Log("Adding tile at " + worldTile.CellLoc.ToString());
            WorldTileDict.Add(worldTile.CellLoc, worldTile);
        }
        //Debug.Log("Dictionary has " + WorldTileDict.Count + " keys");

        // Set Neighbors for all worldTile
        foreach (WorldTile tile in WorldTileList) {
            Dictionary<HexDirection, Vector3Int> neighborVectors =
                HexDirectionUtil.GetNeighborWorldVectors(tile.CellLoc);

            // lookup each neighbor and set it in a dictionary
            Dictionary<HexDirection, WorldTile> neighborTiles = new Dictionary<HexDirection, WorldTile>();
            foreach (var item in neighborVectors) {
                HexDirection hdeNeighbor = item.Key;
                Vector3Int posNeighbor = item.Value;
                WorldTile worldTileNeighbor = GetTileAtLocation(posNeighbor);
                if (!(worldTileNeighbor is WorldTile)) { continue; }
                //Debug.Log("Found neighbor " + hdeNeighbor + "=" + worldTileNeighbor);
                neighborTiles.Add(hdeNeighbor, worldTileNeighbor);
            }
            tile.Neighbors = neighborTiles;
        }
    }
    public WorldTile GetTileAtLocation(Vector3Int location)
    {
        _ = WorldTileDict.TryGetValue(location, out WorldTile tile);
        return tile;
    }

    public WorldTile GetTileForPlayer(Player player)
    {
        Vector3Int posAt = player.GetCellPosition();
        WorldTile tileAt = GetTileAtLocation(posAt);
        if (!(tileAt is WorldTile)) {
            string msg = "Could not find tile for pos " + posAt.ToString();
            throw new System.Exception(msg);
        }

        return tileAt;
    }

    public void RevealTile(WorldTile tile, int radius = 1)
    {
        var tilesChanged = tile.Reveal(radius);

        var args = new WorldTileChangeEventArgs(tilesChanged);
        OnWorldTilesChanged?.Invoke(this, args);
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize() { Init(); }

    public override string ToString()
    {
        return "WorldMap[Tiles:" + WorldTileList.ToString() + "]";
    }
}
