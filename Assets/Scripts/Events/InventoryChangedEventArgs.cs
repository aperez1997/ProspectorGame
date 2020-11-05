using System;

public class InventoryChangedEventArgs : EventArgs
{
    public ItemType Type { get; }

    public int Delta { get; }

    public int OldAmount { get { return NewAmount - Delta; } }

    public int NewAmount { get; }

    public InventoryChangedEventArgs(ItemType type, int delta, int newAmount)
    {
        Type = type;
        Delta = delta;
        NewAmount = newAmount;
    }
}
