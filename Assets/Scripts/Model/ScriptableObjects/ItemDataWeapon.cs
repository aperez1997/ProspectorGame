using UnityEngine;

/// <summary>
/// tool item that is used for killing
/// </summary>
[CreateAssetMenu(fileName = "Weapon-", menuName = "Item: Weapon")]
public class ItemDataWeapon : ItemDataTool
{
    [Tooltip("Ammo SO for this weapon")]
    public ItemData Ammo;

    [Tooltip("Added to hunting chance")]
    public int HuntingModifier;

    public override ItemCategory category { get { return ItemCategory.Weapons; } }
}
