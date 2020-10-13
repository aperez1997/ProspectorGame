using System;
using UnityEngine;

// Represents a cell location
[Serializable]
public class CellPositionStruct
{
    public int xPos;
    public int yPos;
    public int zPos;

    public HexDirection lastDirection;

    public CellPositionStruct(int xPosIn, int yPosIn, int zPosIn, HexDirection lastDirectionIn)
    {
        xPos = xPosIn;
        yPos = yPosIn;
        zPos = zPosIn;
        lastDirection = lastDirectionIn;
    }

    public CellPositionStruct(Vector3Int vector3Int, HexDirection lastDirection)
    {       
        FromVector3IntAndPosition(vector3Int, lastDirection);
    }

    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(xPos, yPos, zPos);
    }

    public void FromVector3IntAndPosition(Vector3Int vector3Int, HexDirection lastDirectionIn)
    {
        xPos = vector3Int.x;
        yPos = vector3Int.y;
        zPos = vector3Int.z;
        lastDirection = lastDirectionIn;
    }

    public override string ToString()
    {
        return "WLS[" + xPos +"," +yPos +"," +zPos + ":" + lastDirection + "]";
    }
}

