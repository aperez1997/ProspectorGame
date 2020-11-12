using System;

public class InventoryChangedEventArgs : EventArgs
{
    public ItemId Type { get; }

    public int Delta { get; }

    public int OldAmount { get { return NewAmount - Delta; } }

    public int NewAmount { get; }

    public InventoryChangedEventArgs(ItemId type, int delta, int newAmount)
    {
        Type = type;
        Delta = delta;
        NewAmount = newAmount;
    }
}
