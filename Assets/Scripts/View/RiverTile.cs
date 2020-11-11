using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Tile that dynanically determines it's appearance based on surrounding tiles
/// Basically Rivers connect to one another
/// </summary>
public class RiverTile : Tile
{
    [SerializeField] protected Sprite[] m_Sprites;

    // This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        for (int yd = -1; yd <= 1; yd++)
        {
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasThisTile(tilemap, position))
                {
                    tilemap.RefreshTile(position);
                }
            }
        }
    }
    // This determines which sprite is used based on the RoadTiles that are adjacent to it and rotates it to fit the other tiles.
    // As the rotation is determined by the RoadTile, the TileFlags.OverrideTransform is set for the tile.
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int mask = 0;

        // check neighbors and add to mask for each neighbor that is also a river
        var neighbors = HexDirectionUtil.GetNeighborWorldVectors(location);
        foreach (KeyValuePair<HexDirection, Vector3Int> pair in neighbors)
        {
            if (HasThisTile(tilemap, pair.Value))
            {
                int maskValue = HexDirectionUtil.GetNeighborMaskValue(pair.Key);
                mask += maskValue;
            }
        }

        // determine sprite by mask
        int index = GetSpriteIndexByBitMask(mask);
        tileData.sprite = m_Sprites[index];
        tileData.color = Color.white;
        var m = tileData.transform;
         
        // determine rotation by mask
        var rot = GetRotationForBitMask(mask);
        m.SetTRS(Vector3.zero, rot, Vector3.one);

        // set tile data
        tileData.transform = m;
        tileData.flags = TileFlags.LockTransform;
        tileData.colliderType = ColliderType.None;
    }

    // This determines if the Tile at the position is the same Tile.
    // Basically, the same exact Tile object will be placed in multiple spots
    protected bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    /// <summary>Gets the ConnectedTileEdges value for the given bitmask</summary>
    public static int GetSpriteIndexByBitMask(int bitMask)
    {
        switch (bitMask)
        {
            default:
                Debug.LogError("Invalid bitMap value for edges: " + bitMask);
                return 12;
            case 0:
                // defaulting to all... but should probably have something special for this
                return 12;
            // 1 edge only
            case 1: // 1
            case 2: // 10 r1
            case 4: // 100 r2
            case 8: // 1000 r3
            case 16: // 10000 r4
            case 32: // 100000 r5
                return 0;
            // 2 edges
            case 3:  // 11
            case 6:  // 110 r1
            case 12: // 1100 r2
            case 24: // 11000 r3
            case 48: // 110000 r4
            case 33: // 100001 => 110000 r5
                return 1;
            case 5:  // 101
            case 10: // 1010 r1
            case 20: // 10100 r2
            case 40: // 101000 r3
            case 17: // 010001 => 101000 r4
            case 34: // 100010 => 101000 r5
                return 2;
            case 9:  // 1001
            case 18: // 10010 r1
            case 36: // 100100 r2
                // Note: Only 3 possibilities here
                return 3;
            // 3 edges
            case 7:  // 000111
            case 14: // 1110 r1
            case 28: // 11100 r2
            case 56: // 111000 r3
            case 49: // 110001 => 000111 r4
            case 35: // 100011 => 000111 r5
                return 4;
            case 11: // 001011
            case 22: // 010110 r1
            case 44: // 101100 r2
            case 25: // 011001 => 001011 r3
            case 50: // 110010 => 001011 r4
            case 37: // 100101 => 001011 r5
                return 5;
            case 19: // 010011
            case 38: // 100110 r1
            case 13: // 001101 => 010011 r2
            case 26: // 011010 => 010011 r3
            case 52: // 110100 => 010011 r4
            case 41: // 101001 => 010011 r5
                return 6;
            case 21: // 10101
            case 42: // 101010 r1
                // only 2 combos
                return 7;
            // 4 edges
            case 15: // 001111
            case 30: // 011110 r1
            case 60: // 111100 => 001111 r2
            case 57: // 111001 => 001111 r3
            case 51: // 110011 => 001111 r4
            case 39: // 100111 => 001111 r5
                return 8;
            case 23: // 010111
            case 46: // 101110 r1
            case 29: // 011101 => 010111 r2
            case 58: // 111010 => 010111 r3
            case 53: // 110101 => 010111 r4
            case 43: // 101011 => 010111 r5
                return 9;
            case 27: // 011011
            case 54: // 110110 => 011011 r-1
            case 45: // 101101 => 011011 r-2
                // Note: Only 3 combos
                return 10;
            // 5 edges
            case 31: // 011111
            case 62: // 111110 r-1
            case 61: // 111101 => 011111 r-2
            case 59: // 111011 => 011111 r-3
            case 55: // 110111 => 011111 r-4
            case 47: // 101111 => 011111 r-5
                return 11;
            // 6 edges
            case 63: // 111111
                return 12;
        }
    }

    /// <summary>gets the tile rotation for the given bitMask value</summary>
    public static Quaternion GetRotationForBitMask(int bitMask)
    {
        switch (bitMask)
        {
            default:
                Debug.LogError("Invalid bitMap value for rotation: " + bitMask);
                return Quaternion.identity;
            case 0:
                return Quaternion.identity;
            case 1: // 1
            case 3:  // 11
            case 7:  // 000111
            case 5:  // 101
            case 9:  // 1001
            case 11: // 001011
            case 15: // 001111
            case 19: // 010011
            case 21: // 10101
            case 23: // 010111
            case 27: // 011011
            case 31: // 011111
            case 63: // 111111
                // R0 (east)
                return Quaternion.identity;
            case 2:  // 10 r1
            case 6:  // 110 r1
            case 10: // 1010 r1
            case 14: // 1110 r1
            case 18: // 10010 r1
            case 22: // 010110 r1
            case 30: // 011110 r1
            case 38: // 100110 r1
            case 42: // 101010 r1
            case 46: // 101110 r1
            case 54: // 110110 => 011011 r-1
            case 62: // 111110 r-1
                // R1 (south east)
                return Quaternion.Euler(0f, 0f, -60f); // rot cw 1/6
            case 4: // 000100 r2
            case 12: // 1100 r2
            case 13: // 001101 => 010011 r2
            case 20: // 10100 r2
            case 28: // 11100 r2
            case 29: // 011101 => 010111 r2
            case 36: // 100100 r2
            case 44: // 101100 r2
            case 45: // 101101 => 011011 r-2
            case 60: // 111100 => 001111 r2
            case 61: // 111101 => 011111 r-2
                // R2 (south west)
                return Quaternion.Euler(0f, 0f, -120f);
                //return Quaternion.Euler(0f, 180f, -60f); // rot cw 1/6 flip horiz
            case 8:  // 001000 r3
            case 24: // 011000 r3
            case 25: // 011001 => 001011 r3
            case 26: // 011010 => 010011 r3
            case 40: // 101000 r3
            case 56: // 111000 r3
            case 57: // 111001 => 001111 r3
            case 58: // 111010 => 010111 r3
            case 59: // 111011 => 011111 r-3
                // R3 (west)
                return Quaternion.Euler(0f, 0f, -180f);
                //return Quaternion.Euler(180f, 180f, 0); // flipped both ways
            case 16: // 010000 r4
            case 17: // 010001 => 101000 r4
            case 48: // 110000 r4
            case 49: // 110001 => 000111 r4
            case 50: // 110010 => 001011 r4
            case 52: // 110100 => 010011 r4
            case 53: // 110101 => 010111 r4
            case 51: // 110011 => 001111 r4
            case 55: // 110111 => 011111 r-4
                // R4 (north west)
                return Quaternion.Euler(0f, 0f, -240f);
                //return Quaternion.Euler(180f, 180f, -60f); // flipped both rot cw 60
            case 32: // 100000 r5
            case 33: // 100001 => 110000 r5
            case 34: // 100010 => 101000 r5
            case 37: // 100101 => 001011 r5
            case 35: // 100011 => 000111 r5
            case 39: // 100111 => 001111 r5
            case 41: // 101001 => 010011 r5
            case 43: // 101011 => 010111 r5
            case 47: // 101111 => 011111 r-5
                // R5 (north east)
                return Quaternion.Euler(0f, 0f, -300f);
                //return Quaternion.Euler(0f, 0f, 60f); // rot ccw 1/6
        }
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/RiverTile")]
    public static void CreateRiverTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save River Tile", "New River Tile", "Asset", "Save River Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RiverTile>(), path);
    }
#endif
}
