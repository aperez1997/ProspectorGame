using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item that is consumed, like food
/// </summary>
[CreateAssetMenu(fileName = "food-", menuName = "Item: Food")]
public class ItemDataConsumable : ItemData
{
    public override ItemCategory category { get { return ItemCategory.Food; } }
}