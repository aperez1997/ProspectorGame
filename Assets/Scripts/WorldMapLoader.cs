﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMapLoader : MonoBehaviour
{
    public static WorldMapLoader Instance { get; private set; }

    public Tilemap tileMap;

    private Dictionary<Vector3Int, DataTile> DataTileDict = new Dictionary<Vector3Int, DataTile>(0);

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
            GameStateManager.LoadGame();
        }

        List<DataTile> dataTileList = GameState.Instance.DataTileList;
        foreach (DataTile dataTile in dataTileList)
        {
            Tile tile = GetTileFromTerrainType(dataTile.Type);
            tileMap.SetTile(dataTile.CellLoc, tile);
            DataTileDict.Add(dataTile.CellLoc, dataTile);
        }
    }

    public DataTile GetDataTileAtLocation(Vector3Int location)
    {
        DataTile dataTile;
        bool rv = DataTileDict.TryGetValue(location, out dataTile);
        return dataTile;
    }

    public static List<DataTile> CreateRandomWorldMap()
    {
        List<DataTile> dTileList = new List<DataTile>();

        int maxX = 5;
        int maxY = 5;

        // generates a random map for now
        for (int x = -1 * maxX; x <= maxX; x++)
        {
            for (int y = -1 * maxY; y <= maxY; y++)
            {
                Vector3Int loc = new Vector3Int(x, y, 0);

                TerrainType tt = TerrainTypeUtil.GetRandomTypeForRandoMap();
                // surround the edges of the map with water
                if (Math.Abs(x) == maxX || Math.Abs(y) == maxY) { tt = TerrainType.Water; }
                // always start in town
                else if (x == 0 && y == 0) { tt = TerrainType.Town; }

                int cost = GetCostFromTerrainType(tt);

                DataTile dTile = new DataTile(loc, tt, cost);
                dTileList.Add(dTile);               
            }
        }
        return dTileList;
    }

    public static Tile GetTileFromTerrainType(TerrainType tt)
    {
        switch (tt)
        {
            case TerrainType.Grass:
                return TileAssets.Instance.grassTile;
            case TerrainType.Forest:
                return TileAssets.Instance.forestTile;
            case TerrainType.Hills:
                return TileAssets.Instance.hillsTile;
            case TerrainType.Mountains:
                return TileAssets.Instance.mountainTile;
            case TerrainType.Water:
                return TileAssets.Instance.waterTile;
            case TerrainType.Town:
                return TileAssets.Instance.townTile;
        }
        Debug.LogError("Unmapped terrain type " + tt.ToString());
        return null;
    }

    public static int GetCostFromTerrainType(TerrainType tt)
    {
        switch (tt)
        {
            case TerrainType.Grass:
                return 1;
            case TerrainType.Forest:
                return 2;
            case TerrainType.Hills:
                return 3;
            case TerrainType.Mountains:
                return 4;
            case TerrainType.Water:
                return -1;
            case TerrainType.Town:
                return 2;
        }
        Debug.LogError("Unmapped terrain type " + tt.ToString());
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

struct WorldBuilderTile
{
    public DataTile DataTile;
    public TileBase TileBase;

    public WorldBuilderTile(DataTile dataTile, TileBase tileBase)
    {
        DataTile = dataTile;
        TileBase = tileBase;
    }

    public Vector3Int CellLocation { get => DataTile.CellLoc; }
}
