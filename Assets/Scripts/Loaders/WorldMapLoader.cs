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
            Vector3Int cellLoc = worldTile.CellLoc;
            tileMap.SetTile(cellLoc, tile);

            // load features
            foreach (var feature in worldTile.Features)
            {
                var z = feature.zIndex;
                var loc = new Vector3Int(cellLoc.x, cellLoc.y, z);
                tileMap.SetTile(loc, feature.hexTile);
            }
        }
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

                // grass around the center
                if (Math.Abs(x) <= 1 && Math.Abs(y) <= 1){ tt = BiomeType.Grass; }

                WorldTile dTile = new WorldTile(loc, tt);
                dTileList.Add(dTile);

                // add features
                if (x == 0 && y == 0) {
                    // always start in town
                    dTile.AddFeature(TileFeatureType.Town);
                }
                if (RoadLocs.Contains((x,y))){
                    // Road locations
                    dTile.AddFeature(TileFeatureType.Road);
                }

                if (RiverLocs.Contains((x, y)))
                {
                    // Road locations
                    dTile.AddFeature(TileFeatureType.River);
                }
            }
        }
        return dTileList;
    }

    private static List<(int, int)> RoadLocs = new List<(int, int)>(new (int,int)[]{
        (-1,-1), (0,0), (1,0), (1,1), (1,2), (1,-1)
    });

    private static List<(int, int)> RiverLocs = new List<(int, int)>(new (int, int)[]{
        (-4,-4), (-4,-3), (-4,-2), (-4,-1), (-3,0), (-3,1)
    });

    // Update is called once per frame
    void Update()
    {
        
    }
}