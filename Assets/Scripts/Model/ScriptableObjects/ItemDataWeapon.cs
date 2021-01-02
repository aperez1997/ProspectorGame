using UnityEngine;

[CreateAssetMenu(fileName = "Weapon-", menuName = "Item: Weapon")]
public class ItemDataWeapon : ItemData
{
    public ItemId AmmoId;

    [Tooltip("Added to hunting chance")]
    public int HuntingModifier;

    void Reset()
    {
        category = ItemCategory.Weapons;
    }
}
