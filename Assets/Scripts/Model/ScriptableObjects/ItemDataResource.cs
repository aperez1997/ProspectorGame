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
}
