using System;

public class Player
{
    public Inventory Inventory { get; private set; }

    public event EventHandler OnActionPointsChanged;

    private int _actionPoints;
    public int ActionPoints { 
        get { return _actionPoints; }
        private set { _actionPoints = value; OnActionPointsChanged?.Invoke(this, EventArgs.Empty); }
    }

    private readonly int ActionPointsMax;

    public Player(int actionPointsMax)
    {
        Inventory = new Inventory();
        ActionPointsMax = actionPointsMax;
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
}
