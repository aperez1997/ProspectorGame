using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Record class for the player
/// </summary>
[Serializable]
public class Player
{
    // Health
    [field: SerializeField] public int HealthMax { get; private set; }

    [SerializeField] private int _health;
    public int Health
    {
        get { return _health; }
        set {
            var delta = value - _health;
            _health = value;
            OnHealthChanged?.Invoke(this, new IntStatChangeEventArgs(delta, _health));
        }
    }
    public event EventHandler<IntStatChangeEventArgs> OnHealthChanged;

    /// <summary>
    /// The percent of current health vs max
    /// </summary>
    public double HealthPercent {
        get { return (double)Health / (double)HealthMax; }
    }

    // nourishment
    [field: SerializeField] public int NourishmentMax { get; private set; }

    [SerializeField] private int _nourishment;
    public int Nourishment {
        get { return _nourishment; }
        set {
            var delta = value - _nourishment;
            _nourishment = value;
            OnNourishmentChanged?.Invoke(this, new IntStatChangeEventArgs(delta, _nourishment));
        }
    }
    public event EventHandler<IntStatChangeEventArgs> OnNourishmentChanged;

    /// <summary>
    /// The percent of current nourishment vs max
    /// </summary>
    public double NourishmentPercent {
        get { return (double) Nourishment / (double) NourishmentMax; }
    }


    // AP MAX
    [field: SerializeField] public int ActionPointsMax { get; private set; }

    // AP
    [SerializeField] private int _actionPoints;
    public int ActionPoints { 
        get { return _actionPoints; }
        set {
            var delta = value - _actionPoints;
            _actionPoints = value;
            OnActionPointsChanged?.Invoke(this, new IntStatChangeEventArgs(delta, _actionPoints));
        }
    }
    public event EventHandler<IntStatChangeEventArgs> OnActionPointsChanged;

    // Inventory
    [field:SerializeField] public Inventory Inventory { get; private set; }

    // Location
    [SerializeField] private CellPositionStruct _location;
    public CellPositionStruct Location { get => _location; private set => _location = value; }
    public event EventHandler OnLocationChanged;

    // Effects
    [field: SerializeField] public PlayerStatus Status { get; private set; }

    public Player(int maxHealth, int nourishmentMax, int maxActionPoints)
    {
        HealthMax = maxHealth;
        Health = maxHealth;
        NourishmentMax = nourishmentMax;
        Nourishment = nourishmentMax;
        ActionPointsMax = maxActionPoints;
        Inventory = new Inventory();                 
        Location = new CellPositionStruct(0, 0, 0, HexDirection.None);
        Status = new PlayerStatus();
    }

    public bool HasEnoughActionPoints(int desired)
    {
        return ActionPoints >= desired;
    }

    public bool SpendActionPoints(int cost)
    {
        if (!HasEnoughActionPoints(cost)) { return false; }
        ActionPoints -= cost;
        return true;
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
