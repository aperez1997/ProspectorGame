using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Loads the world map into the Overworld's unity TileMap
/// Also handles creation of random world map
/// </summary>
public class WorldMapLoader : MonoBehaviour
{
    public static WorldMapLoader Instance { get; private set; }

    public Tilemap tileMap;

    [Tooltip("Used for tiles that have not been revealed")] 
    public Tile HiddenTile;

    private WorldMap WorldMap;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.DebugLoadState();

        WorldMap = GameStateManager.LogicInstance.WorldMap;
        LoadWorldTileListIntoTileMap(WorldMap.WorldTileList);

        WorldMap.OnWorldTilesChanged += WorldMap_OnWorldTilesChanged;
    }

    public void LoadWorldTileListIntoTileMap(List<WorldTile> worldTileList)
    {
        Debug.Log("Loading World Map with " + worldTileList.Count + " tiles");
        foreach (WorldTile worldTile in worldTileList)
        {
            LoadWorldTileIntoTileMap(worldTile);
        }
    }

    public void LoadWorldTileIntoTileMap(WorldTile worldTile)
    {
        Vector3Int cellLoc = worldTile.CellLoc;
        if (!worldTile.IsRevealed) {
            tileMap.SetTile(cellLoc, HiddenTile);
            return;
        }

        Tile tile = worldTile.Tile;
        tileMap.SetTile(cellLoc, tile);

        // load features
        foreach (var feature in worldTile.Features) {
            var z = feature.zIndex;
            var loc = new Vector3Int(cellLoc.x, cellLoc.y, z);
            tileMap.SetTile(loc, feature.hexTile);
        }
    }

    private void WorldMap_OnWorldTilesChanged(object sender, WorldTileChangeEventArgs e)
    {
        LoadWorldTileListIntoTileMap(e.WorldTileList);
    }

    private void OnDestroy()
    {
        WorldMap.OnWorldTilesChanged -= WorldMap_OnWorldTilesChanged;
    }
}