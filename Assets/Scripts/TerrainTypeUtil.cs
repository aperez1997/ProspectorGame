using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// All the known terrain types (base). Types are spelled out because they will (eventually) go into a file
public enum TerrainType { Water = 1, Grass = 2, Forest = 3, Hills = 4, Mountains = 5, Town = 6 };

// Util package for terrain related stuff
public class TerrainTypeUtil
{
    public static TerrainType GetTerrainTypeFromInt(int type)
    {
        // TODO: bounds check
        return (TerrainType)type;
    }

    public static TerrainType GetRandomTypeForRandoMap()
    {
        System.Array values = System.Enum.GetValues(typeof(TerrainType));
        int index = Random.Range(0, values.Length - 1); // Don't return town tiles
        return (TerrainType) values.GetValue(index);
    }
}
