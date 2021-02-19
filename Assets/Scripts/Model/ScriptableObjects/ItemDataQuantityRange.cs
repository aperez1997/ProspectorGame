using UnityEngine;

/// <summary>
/// ItemData and a range of quantities
/// </summary>
[System.Serializable]
public class ItemDataQuantityRange : IQuantityRange
{
    [Tooltip("The item")]
    public ItemData Item;
    [Tooltip("Required min qty. Can be zero if the max is set")]
    public int QuantityMin;
    [Tooltip("Optional max qty. If set and larger than min, a random value in between will be given")]
    public int QuantityMax;

    public int GetQuantityMin() { return QuantityMin; }
    public int GetQuantityMax() { return QuantityMax; }
}