using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMapLoader : MonoBehaviour
{
    public static WorldMapLoader Instance { get; private set; }

    public Tilemap tileMapBiome;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.DebugLoadState();

        LoadMapDataIntoTileMap(GameState.Instance.WorldTileList);
    }

    public void LoadMapDataIntoTileMap(List<WorldTile> worldTileList)
    {
        Debug.Log("Loading World Map with " + worldTileList.Count + " tiles");
        foreach (WorldTile worldTile in worldTileList)
        {
            Tile tile = worldTile.Tile;
            tileMapBiome.SetTile(worldTile.CellLoc, tile);
        }

        //tileMapBiome.SetTile(new Vector3Int(1, 1, 1), testTile);
    }

    // TODO: should probably be moved into gameLogic, once I figure out a way to do that
    public static List<WorldTile> CreateRandomWorldMap()
    {
        /**
         * consider using data-struct (this many mountains, this height of water)
         * that can be made into a hash, and hash always generates the same level
         */ 

        List<WorldTile> dTileList = new List<WorldTile>();

        int maxX = 5;
        int maxY = 5;

        // generates a random map for now
        for (int x = -1 * maxX; x <= maxX; x++)
        {
            for (int y = -1 * maxY; y <= maxY; y++)
            {
                Vector3Int loc = new Vector3Int(x, y, 0);

                BiomeType tt = BiomeData.GetRandomTypeForRandoMap();
                // surround the edges of the map with water
                if (Math.Abs(x) == maxX || Math.Abs(y) == maxY) { tt = BiomeType.Water; }
                // always start in town
                else if (x == 0 && y == 0) { tt = BiomeType.Town; }

                WorldTile dTile = new WorldTile(loc, tt);
                dTileList.Add(dTile);               
            }
        }
        return dTileList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}