using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Natural resources like gold
/// </summary>
[CreateAssetMenu(fileName = "resource-", menuName = "Item: Resource")]
public class ItemDataResource : ItemData
{
    public override ItemCategory category { get { return ItemCategory.Resources; } }

    [Tooltip("If the quantity needs to be displayed a special way, use this")]
    public QuantityStyle quantityStyle;

    public void Reset()
    {
        quantityStyle = QuantityStyle.None;
    }
}

/// <summary>
/// 
/// </summary>
public enum QuantityStyle
{
    None = 1, Money = 2
}