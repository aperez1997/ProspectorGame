using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

/// <summary>
/// Simple class that represents a sum of numbers, for display to the user
/// Each item has a description and int value
/// </summary>
public class SumDescription
{
    private List<SumItem> _sumItems;
    public ReadOnlyCollection<SumItem> SumItems { get { return _sumItems.AsReadOnly(); } }

    private int _sum;
    public int Sum { get { return _sum; } }

    public SumDescription()
    {
        init(new SumItem[0]);
    }

    public SumDescription(SumItem[] items)
    {
        init(items);
    }

    /// <summary>
    /// Create with a single item
    /// </summary>
    public SumDescription(string name, int value)
    {
        var items = new SumItem[] { new SumItem(name, value) };
        init(items);
    }

    public SumDescription(List<SumItem> items)
    {
        init(items.ToArray());
    }

    private void init(SumItem[] items)
    {
        _sumItems = new List<SumItem>(items);
        _sum = GetTotalCostFromItems(_sumItems);
    }

    public void AddSumItem(SumItem item)
    {
        _sumItems.Add(item);
        _sum += item.Value;
    }

    public void AddItem(string name, int value)
    {
        AddSumItem(new SumItem(name, value));
    }

    /// <summary>
    /// Add all items of the given sum to the current sum
    /// </summary>
    public void AddSumDescription(SumDescription otherDesc)
    {
        _sumItems.AddRange(otherDesc._sumItems);
        _sum += otherDesc.Sum;
    }

    /// <summary>
    /// Get the full description for the given sum
    /// </summary>
    public string GetDescriptionText(string delimiter = ",")
    {
        var strings = _sumItems.Select(item => item.Name + " " + item.Value.ToString());
        return string.Join(delimiter, strings);
    }

    public override string ToString()
    {
        return "Sum[" + GetDescriptionText(",") + "]";
    }

    public static int GetTotalCostFromItems(SumItem[] items)
    {
        return items.Sum(item => item.Value);
    }

    public static int GetTotalCostFromItems(List<SumItem> items)
    {
        return items.Sum(item => item.Value);
    }

    public static SumDescription Empty {
        get {
            return new SumDescription(new SumItem[0]);
        }
    }
}

/// <summary>
/// Individual part of a sum
/// </summary>
public struct SumItem
{
    public readonly string Name;
    public readonly int Value;

    public SumItem(string name, int value)
    {
        Name = name;
        Value = value;
    }
}