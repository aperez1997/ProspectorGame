using System;
using System.Collections.Generic;
using UnityEngine;

// Not strictly necessary to enumerate these, but the UI also has them.
public enum HexDirection { None = 0, East = 1, SouthEast = 2, SouthWest = 3, West = 4, NorthWest = 5, NorthEast = 6 };

public class HexDirectionUtil
{
    public static HexDirection East = HexDirection.East;
    public static HexDirection SouthEast = HexDirection.SouthEast;
    public static HexDirection SouthWest = HexDirection.SouthWest;
    public static HexDirection West = HexDirection.West;
    public static HexDirection NorthWest = HexDirection.NorthWest;
    public static HexDirection NorthEast = HexDirection.NorthEast;

    // TODO: do we really need objects? I think the enum is probably enough
    public HexDirection hde;
    public HexDirectionUtil(HexDirection hde)
    {
        this.hde = hde;
    }

    // Turns a direction enum into a move vector for the player.
    // TODO: this probably only works for the scale I'm currently using. Should have a float scale param
    public static Vector3 HexDirectionEnumToWorldVector3(HexDirection hde)
    {
        float x = 0, y = 0;
        // check north/south for y = += .75
        if (hde == HexDirection.SouthEast || hde == HexDirection.SouthWest)
        {
            y = -.75f;
        }
        else if (hde == HexDirection.NorthEast || hde == HexDirection.NorthWest)
        {
            y = .75f;
        }

        // check north/south east/west for x = +-0.5
        if (hde == HexDirection.SouthEast || hde == HexDirection.NorthEast)
        {
            x = 0.5f;
        }
        else if (hde == HexDirection.SouthWest || hde == HexDirection.NorthWest)
        {
            x = -0.5f;
        }
        // or due east/west for x = +- 1
        else if (hde == HexDirection.East)
        {
            x = 1f;
        }
        else if (hde == HexDirection.West)
        {
            x = -1f;
        }

        Vector3 vector = new Vector3(x, y, 0);
        return vector;
    }

    // create a new vector3Int (cellPos) that is the given cellPos moved in the corresponding direction
    public static Vector3Int TranslateVector3Int(Vector3Int currentCellPos, HexDirection hde)
    {
        // determine which hash to use
        bool isEvenRow = currentCellPos.y % 2 == 0;
        Dictionary<HexDirection, (int, int)> neighborSet = isEvenRow ? NeighborHashEven : NeighborHashOdd;
        // lookup direction in our hash, value is x/y offsets
        neighborSet.TryGetValue(hde, out (int, int) offset);
        int xOffset = offset.Item1, yOffset = offset.Item2;
        // create new vector that is current vector + offsets
        return new Vector3Int(currentCellPos.x + xOffset, currentCellPos.y + yOffset, currentCellPos.z);
    }

    // TODO: Use the hashes instead of the lists
    // Get all neighbors of currentPos as WorldLocation Vectors, hashed by the corresponding direction
    public static Dictionary<HexDirection, Vector3Int> GetNeighborWorldVectors(Vector3Int currentPos)
    {
        Dictionary<HexDirection, Vector3Int> dict = new Dictionary<HexDirection, Vector3Int>();

        // even and odd rows(y) use different sets of neighbors.
        bool isEvenRow = currentPos.y % 2 == 0;
        ValueTuple<HexDirection, int, int>[] neighborSet = isEvenRow ? NeighborTuplesEven : NeighborTuplesOdd;

        // turn neighbor tuples into direction+vectors
        foreach (ValueTuple<HexDirection, int, int> tuple in neighborSet)
        {
            (HexDirection hexDirection, int xOffset, int yOffset) = tuple;

            Vector3Int posNeighbor = new Vector3Int(currentPos.x + xOffset, currentPos.y + yOffset, 0);
            dict.Add(hexDirection, posNeighbor);
        }
        return dict;
    }

    private static readonly Dictionary<HexDirection, (int, int)> NeighborHashEven = new Dictionary<HexDirection, (int, int)>
        {{HexDirection.East,(1,0)},{HexDirection.West,(-1,0)},{HexDirection.NorthEast,(0,1)}, {HexDirection.NorthWest,(-1,1)}, {HexDirection.SouthEast,(0,-1)}, {HexDirection.SouthWest,(-1,-1)}};

    private static readonly Dictionary<HexDirection, ValueTuple<int, int>> NeighborHashOdd = new Dictionary<HexDirection, (int, int)>
        {{HexDirection.East,(1,0)}, {HexDirection.West,(-1,0)}, {HexDirection.NorthEast,(1,1)}, {HexDirection.NorthWest,(0,1)}, {HexDirection.SouthEast,(1,-1)}, {HexDirection.SouthWest,(0,-1)} };

    /**
     * Unity uses odd-r convention. On Even rows (y) we want x=-1 and x=0 for the above/below spots, on Odd rows we want x=0 and x=1
     * https://www.redblobgames.com/grids/hexagons/
     */
    private static readonly ValueTuple<HexDirection, int, int>[] NeighborTuplesEven = 
        {(HexDirection.East,1,0), (HexDirection.West,-1,0), (HexDirection.NorthEast,0,1), (HexDirection.NorthWest,-1,1), (HexDirection.SouthEast,0,-1), (HexDirection.SouthWest,-1,-1)};

    private static readonly ValueTuple<HexDirection, int, int>[] NeighborTuplesOdd =
        {(HexDirection.East,1,0), (HexDirection.West,-1,0), (HexDirection.NorthEast,1,1), (HexDirection.NorthWest,0,1), (HexDirection.SouthEast,1,-1), (HexDirection.SouthWest,0,-1)};
}