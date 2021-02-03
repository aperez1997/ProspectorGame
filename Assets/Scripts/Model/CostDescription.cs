using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


/// <summary>
/// A simple class that is used to represent costs associated with an action
/// </summary>
public class CostDescription
{
    public readonly CostItem[] CostItems;

    public int TotalCost {
        get {
            return GetTotalCostFromCostItems(CostItems);
        }
    }

    public CostDescription(CostItem[] costs)
    {
        CostItems = costs;
    }

    public CostDescription(List<CostItem> costs)
    {
        CostItems = costs.ToArray();
    }

    public static int GetTotalCostFromCostItems(CostItem[] costItems)
    {
        return costItems.Sum(item => item.Value);
    }

    public static int GetTotalCostFromCostItems(List<CostItem> costItems)
    {
        return costItems.Sum(item => item.Value);
    }

    public static CostDescription Empty {
        get {
            return new CostDescription(new CostItem[0]);
        }
    }
}

public struct CostItem
{
    public string Name;
    public int Value;

    public CostItem(string name, int value)
    {
        Name = name;
        Value = value;
    }
}