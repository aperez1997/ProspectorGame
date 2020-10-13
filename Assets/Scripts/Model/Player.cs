using System;
using UnityEngine;

[Serializable]
public class Player
{
    [SerializeField] private Inventory _inventory;
    public Inventory Inventory { get => _inventory; private set => _inventory = value; }

    [SerializeField] private CellPositionStruct _location;
    public CellPositionStruct Location { get => _location; private set => _location = value; }
    public event EventHandler OnLocationChanged;

    [SerializeField] private int _actionPoints;
    public int ActionPoints { 
        get { return _actionPoints; }
        private set { _actionPoints = value; OnActionPointsChanged?.Invoke(this, EventArgs.Empty); }
    }
    public event EventHandler OnActionPointsChanged;

    [SerializeField] private int ActionPointsMax;

    public Player(int actionPointsMax)
    {
        Inventory = new Inventory();
        ActionPointsMax = actionPointsMax;
        Location = new CellPositionStruct(0, 0, 0, HexDirection.None);
    }

    public void ResetActionPoints()
    {
        ActionPoints = this.ActionPointsMax;
    }

    public int UseActionPoints(int used)
    {
        ActionPoints -= used;
        return ActionPoints;
    }
    
    public void SetLocation(Vector3Int vector3int, HexDirection direction)
    {
        Location.FromVector3IntAndPosition(vector3int, direction);
        OnLocationChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector3Int GetCellPosition()
    {
        return Location.ToVector3Int();
    }

    public override string ToString()
    {
        return "Player[" 
            + Location.ToString() + ",AP(" + ActionPointsMax + "," + ActionPoints + ")"
            + "," + Inventory.ToString()
            + "]";
       
    }
}
