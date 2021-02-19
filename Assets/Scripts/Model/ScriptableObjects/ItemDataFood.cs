using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item that is consumed for nutrients
/// </summary>
[CreateAssetMenu(fileName = "food-", menuName = "Item: Food")]
public class ItemDataFood : ItemData
{
    public override ItemCategory category { get { return ItemCategory.Food; } }

    [Tooltip("Items that are given to player when cooked, if any")]
    public FoodItemQuantity[] CookedItems;
}

/// <summary>
/// Food item and quantity
/// </summary>
[System.Serializable]
public class FoodItemQuantity
{
    public ItemDataFood FoodItem;
    public int Quantity;
}