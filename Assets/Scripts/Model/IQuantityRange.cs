/// <summary>
/// An interface for a thing that has a min and max quantity
/// Used to express a random range of things (min to max)
/// </summary>
public interface IQuantityRange
{
    /// <summary>
    /// Required min value. Can be zero if max is higher than 0
    /// </summary>
    int GetQuantityMin();

    /// <summary>
    /// Optional Max Quantity.
    /// If greater than min, then min->max will be used, otherwise min will be used
    /// </summary>
    int GetQuantityMax();
}