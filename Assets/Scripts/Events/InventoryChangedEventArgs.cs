using System;

/// <summary>
/// When the inventory changes
/// </summary>
public class InventoryChangedEventArgs : EventArgs
{
    public string ItemId { get; }

    public string Name { get; }

    public int Delta { get; }

    public int OldAmount { get { return NewAmount - Delta; } }

    public int NewAmount { get; }

    public InventoryChangedEventArgs(string itemId, string name, int delta, int newAmount)
    {
        ItemId = itemId;
        Name = name;
        Delta = delta;
        NewAmount = newAmount;
    }
}
