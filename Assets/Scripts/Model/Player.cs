using System;
using UnityEngine;

[Serializable]
public class Player
{
    [field:SerializeField] public Inventory Inventory { get; private set; }

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

    public bool HasEnoughActionPoints(int desired)
    {
        return ActionPoints >= desired;
    }

    public int UseActionPoints(int used)
    {
        if (HasEnoughActionPoints(used))
        {
            ActionPoints -= used;            
        }
        return ActionPoints;
    }

    public void ResetActionPoints()
    {
        ActionPoints = this.ActionPointsMax;
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
