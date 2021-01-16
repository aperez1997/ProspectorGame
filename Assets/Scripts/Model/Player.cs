﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Record class for the player
/// </summary>
[Serializable]
public class Player
{
    public const int MAX_HEALTH = 4;
    public const int MAX_AP = 12;

    // Health
    [SerializeField] private int _health;
    public int Health
    {
        get { return _health; }
        private set {
            var delta = value - _health;
            _health = value;
            OnHealthChanged?.Invoke(this, new IntStatChangeEventArgs(delta, _health));
        }
    }
    public event EventHandler<IntStatChangeEventArgs> OnHealthChanged;
    
    // AP MAX
    [field:SerializeField] public int ActionPointsMax { get; private set; }

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

    public Player()
    {
        Health = MAX_HEALTH;
        ActionPointsMax = MAX_AP;
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

    public bool ReduceHealth(int amount = 1)
    {
        Health -= amount;
        return true;
    }

    public override string ToString()
    {
        return "Player[" 
            + Location.ToString() + ",AP(" + ActionPointsMax + "," + ActionPoints + ")"
            + "," + Inventory.ToString()
            + "]";
    }
}
