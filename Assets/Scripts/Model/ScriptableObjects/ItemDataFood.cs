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

    [Tooltip("Amount of nourishment given to player when eaten")]
    public int Nourishment;

    [Tooltip("Food Items that are given to player when cooked, if any")]
    public FoodItemQuantityRange[] CookedItems;

    public void Reset()
    {
        Nourishment = 1;
    }
}

/// <summary>
/// Food item and range of quantity
/// </summary>
[System.Serializable]
public class FoodItemQuantityRange : IQuantityRange
{
    [Tooltip("The food item")]
    public ItemDataFood FoodItem;
    [Tooltip("Required min qty. Can be zero if the max is set")]
    public int QuantityMin;
    [Tooltip("Optional max qty. If set and larger than min, a random value in between will be given")]
    public int QuantityMax;

    public int GetQuantityMin() { return QuantityMin; }
    public int GetQuantityMax() { return QuantityMax; }
}