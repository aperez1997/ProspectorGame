using UnityEngine;

/// <summary>
/// Sub-class of SO items that are weapons
/// </summary>
[CreateAssetMenu(fileName = "Weapon-", menuName = "Item: Weapon")]
public class ItemDataWeapon : ItemData
{
    [Tooltip("Ammo SO for this weapon")]
    public ItemData Ammo;

    [Tooltip("Added to hunting chance")]
    public int HuntingModifier;

    void Reset()
    {
        category = ItemCategory.Weapons;
    }
}
