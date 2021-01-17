using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item that is used to perform actions
/// </summary>
[CreateAssetMenu(fileName = "Tool-", menuName = "Item: Tool")]
public class ItemDataTool : ItemData
{
    public override ItemCategory category { get { return ItemCategory.Tools; } }
}
