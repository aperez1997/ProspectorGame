using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An animal carcass. These are obtained from hunting, and broken down into "output" via skinning
/// Currently, the carcass given when hunting is chosen randomly from all available carcasses
/// </summary>
[CreateAssetMenu(fileName = "carcass-", menuName = "Item: Carcass")]
public class ItemDataCarcass : ItemDataResource
{
    public override ItemCategory category { get { return ItemCategory.Carcass; } }

    [Tooltip("Items that are given to player when the carcass is skinned using a knife")]
    public ItemData[] Output;
}
