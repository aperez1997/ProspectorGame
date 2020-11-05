using System;

/**
 * <summary>Represents an integer stat change</summary>
 */
public class IntStatChangeEventArgs : EventArgs
{
    /// <summary>
    /// Amount the stat changed by
    /// </summary>
    public int Delta { get; }

    /// <summary>
    /// The new value
    /// </summary>
    public int NewValue { get; }

    /// <summary>
    /// The original Value
    /// </summary>
    public int OldValue { get{ return NewValue - Delta; } }

    public IntStatChangeEventArgs(int delta, int newValue)
    {
        this.Delta = delta;
        this.NewValue = newValue;
    }
}
