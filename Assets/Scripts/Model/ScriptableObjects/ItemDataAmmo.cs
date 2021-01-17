using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ammo used for weapons
/// </summary>
[CreateAssetMenu(fileName = "ammo-", menuName = "Item: Ammo")]
public class ItemDataAmmo : ItemData
{
    public override ItemCategory category { get { return ItemCategory.Ammo; } }

    public void Reset()
    {
        stackable = true;
    }
}
