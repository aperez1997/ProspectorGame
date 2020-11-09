using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Holds tile sprites for the various kinds of rivers, as well as the edge data
/// </summary>
[CreateAssetMenu(fileName = "RiverTile", menuName = "RiverTile")]
public class RiverTile : ScriptableObject
{
    [Tooltip("Number of edges that the river connects to")]
    public int edgeCount;

    [Tooltip("Bitmask of edge")]
    public ConnectedTileEdges connectedEdges;

    [Tooltip("tile sprite")]
    public Tile hexTile;
}

/// <summary>
/// Possible values of river edges.
/// Not all values in the bitmask are represented because we rotate. 
/// </summary>
public enum ConnectedTileEdges
{
    // 2 edges
    Two_adjacent = 3, // e + se
    Two_skip1 = 5, // e + sw
    Two_across = 9, // e + w
    // 3 edges
    Three_adjacent = 7, // e + se + sw
    Three_skip1a = 11, // e + se + w
    Three_skip1b = 19, // e + se + nw
    Three_split = 21, // E_SW_NW
    // 4 edges
    Four_adjacent = 15, // E_SE_SW_W
    Four_skip1 = 23, // E_SE_SW_NW
    Four_pairs = 27, // E_SE_W_NW
    // 5 edges
    Five_missing1 = 31, // all but ne
    // 6 edges
    All = 63
}

// Bitmask values
// e = 1, se = 2, sw = 4, w = 8, nw = 16, ne = 32

public class ConnectedEdgeTileLoader
{
    public static Tile GetRiverTile(int edgeBitMap)
    {
        return null;
    }

    public static ConnectedTileEdges GetEdgesByBitMap(int bitMap)
    {
        switch (bitMap)
        {
            // 1 edge only
            case 0:
            case 1:
            case 2:
            case 4:
            case 8:
            case 16:
            case 32:
            default:
                Debug.LogError("Invalid bitMap value" + bitMap);
                return ConnectedTileEdges.All;
            // 2 edges
            case 3:  // 11
            case 6:  // 110 r1
            case 12: // 1100 r2
            case 24: // 11000 r3
            case 48: // 110000 r4
            case 33: // 100001 => 110000 r5
                return ConnectedTileEdges.Two_adjacent;
            case 5:  // 101
            case 10: // 1010 r1
            case 20: // 10100 r2
            case 40: // 101000 r3
            case 17: // 010001 => 101000 r4
            case 34: // 100010 => 101000 r5
                return ConnectedTileEdges.Two_skip1;
            case 9:  // 1001
            case 18: // 10010 r1
            case 36: // 100100 r2
                // Note: Only 3 possibilities here
                return ConnectedTileEdges.Two_across;
            // 3 edges
            case 7:  // 000111
            case 14: // 1110 r1
            case 28: // 11100 r2
            case 56: // 111000 r3
            case 49: // 110001 => 000111 r4
            case 35: // 100011 => 000111 r5
                return ConnectedTileEdges.Three_adjacent;
            case 11: // 001011
            case 22: // 010110 r1
            case 44: // 101100 r2
            case 25: // 011001 => 001011 r3
            case 50: // 110010 => 001011 r4
            case 37: // 100101 => 001011 r5
                return ConnectedTileEdges.Three_skip1a;
            case 19: // 010011
            case 38: // 100110 r1
            case 13: // 001101 => 010011 r2
            case 26: // 011010 => 010011 r3
            case 52: // 110100 => 010011 r4
            case 41: // 101001 => 010011 r5
                return ConnectedTileEdges.Three_skip1b;
            case 21: // 10101
            case 42: // 101010 r1
                // only 2 combos
                return ConnectedTileEdges.Three_split;
            // 4 edges
            case 15: // 001111
            case 30: // 011110 r1
            case 60: // 111100 => 001111 r2
            case 57: // 111001 => 001111 r3
            case 51: // 110011 => 001111 r4
            case 39: // 100111 => 001111 r5
                return ConnectedTileEdges.Four_adjacent;
            case 23: // 010111
            case 46: // 101110 r1
            case 29: // 011101 => 010111 r2
            case 58: // 111010 => 010111 r3
            case 53: // 110101 => 010111 r4
            case 43: // 101011 => 010111 r5
                return ConnectedTileEdges.Four_skip1;
            case 27: // 011011
            case 54: // 110110 => 011011 r-1
            case 45: // 101101 => 011011 r-2
                // Note: Only 3 combos
                return ConnectedTileEdges.Four_pairs;
            // 5 edges
            case 31: // 011111
            case 62: // 111110 r-1
            case 61: // 111101 => 011111 r-2
            case 59: // 111011 => 011111 r-3
            case 55: // 110111 => 011111 r-4
            case 47: // 101111 => 011111 r-5
                return ConnectedTileEdges.Five_missing1;
            // 6 edges
            case 63: // 111111
                return ConnectedTileEdges.All;
        }
    }
}