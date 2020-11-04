﻿using System;
using UnityEngine;

[Serializable]
public class Player
{
    public const int MAX_HEALTH = 4;

    // Health
    [SerializeField] private int _health;
    public int Health
    {
        get { return _health; }
        private set { _health = value; OnHealthChanged?.Invoke(this, EventArgs.Empty); }
    }
    public event EventHandler OnHealthChanged;
    
    // AP MAX
    [field:SerializeField] public int ActionPointsMax { get; private set; }

    // AP
    [SerializeField] private int _actionPoints;
    public int ActionPoints { 
        get { return _actionPoints; }
        private set { _actionPoints = value; OnActionPointsChanged?.Invoke(this, EventArgs.Empty); }
    }
    public event EventHandler OnActionPointsChanged;

    // Inventory
    [field:SerializeField] public Inventory Inventory { get; private set; }

    // Money
    public event EventHandler OnMoneyChanged;

    // Location
    [SerializeField] private CellPositionStruct _location;
    public CellPositionStruct Location { get => _location; private set => _location = value; }
    public event EventHandler OnLocationChanged;

    public Player(int actionPointsMax)
    {
        Health = MAX_HEALTH;
        ActionPointsMax = actionPointsMax;
        Inventory = new Inventory();
        Location = new CellPositionStruct(0, 0, 0, HexDirection.None);
        ResetActionPoints();
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

    public void ResetActionPoints()
    {
        // AP reduced by health loss.
        int healthLossAPRedux = (MAX_HEALTH - Health);
        ActionPoints = this.ActionPointsMax - healthLossAPRedux;
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

    public bool HasEnoughMoney(int cost)
    {
        return Inventory.HasItem(ItemType.Money, cost);
    }

    public bool SpendMoney(int cost)
    {
        if (!HasEnoughMoney(cost)){ return false; }
        Inventory.RemoveItem(ItemType.Money, cost);
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public bool ReceiveMoney(int amount)
    {
        Inventory.AddItem(ItemType.Money, amount);
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
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
