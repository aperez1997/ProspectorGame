using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// For River Test scene, makes a few rivers tiles to check the dynamic sprites
/// </summary>
public class RiverTileTest : MonoBehaviour
{
    public Tilemap tileMap;

    public RiverTile riverTile;
    public RoadTile roadTile;

    private Tile grass;

    private void Awake()
    {
        transform.Translate(new Vector3(-4, -4, 0));

        // grass
        grass = BiomeDataLoader.LoadBiomeDataByType(BiomeType.Grass).hexTile;

        // 1 edge
        SetRiver(new Vector3Int(0, 0, 1));
        SetRiver(new Vector3Int(1, 0, 1));

        SetRiver(new Vector3Int(3, 0, 1));
        SetRiver(new Vector3Int(3, -1, 1));

        SetRiver(new Vector3Int(5, -1, 1));
        SetRiver(new Vector3Int(6, 0, 1));

        // 2 edges
        SetRiver(new Vector3Int(0, 2, 1));
        SetRiver(new Vector3Int(1, 2, 1));
        SetRiver(new Vector3Int(0, 3, 1));

        SetRiver(new Vector3Int(3, 2, 1));
        SetRiver(new Vector3Int(4, 2, 1));
        SetRiver(new Vector3Int(4, 3, 1));

        SetRiver(new Vector3Int(6, 2, 1));
        SetRiver(new Vector3Int(7, 2, 1));
        SetRiver(new Vector3Int(8, 2, 1));

        // 3 edges
        SetRiver(new Vector3Int(0, 5, 1));
        SetRiver(new Vector3Int(1, 5, 1));
        SetRiver(new Vector3Int(0, 6, 1));
        SetRiver(new Vector3Int(1, 6, 1));
    }

    void Update()
    {
        int speed = 10;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
    }

    private void SetGrass(Vector3Int loc)
    {
        loc.z = 0;
        tileMap.SetTile(loc, grass);
    }

    private void SetRiver(Vector3Int loc)
    {
        SetGrass(loc);
        tileMap.SetTile(loc, riverTile);
        loc.z = 2;
        tileMap.SetTile(loc, roadTile);
    }
}

public enum Bits
{
    east = 1,
    south_east = 2,
    south_west = 4,
    west = 8,
    north_west = 16,
    north_east = 32
}