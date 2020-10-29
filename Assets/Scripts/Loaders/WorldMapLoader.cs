using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        // Hack to auto-load game if we get here and there isn't something loaded yet
        // this should only be for the editor
        if (GameState.Instance == null)
        {
            Debug.Log("No Gamestate, loading game as an editor hack");
            if (!GameStateManager.LoadGame())
            {
                Debug.Log("No saved game. Creating new game as an editor hack");
                GameStateManager.CreateNewGame();
            }
        }

        LoadMapDataIntoTileMap(GameState.Instance.DataTileList);
    }

    public void LoadMapDataIntoTileMap(List<DataTile> dataTileList)
    {
        Debug.Log("Loading World Map with " + dataTileList.Count + " tiles");
        foreach (DataTile dataTile in dataTileList)
        {
            Tile tile = dataTile.Tile;
            tileMap.SetTile(dataTile.CellLoc, tile);
        }
    }

    // TODO: should probably be moved into gameLogic, once I figure out a way to do that
    public static List<DataTile> CreateRandomWorldMap()
    {
        /**
         * consider using data-struct (this many mountains, this height of water)
         * that can be made into a hash, and hash always generates the same level
         */ 

        List<DataTile> dTileList = new List<DataTile>();

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

                DataTile dTile = new DataTile(loc, tt);
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