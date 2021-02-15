using UnityEngine;

/// <summary>
/// tool item that is used for killing
/// </summary>
[CreateAssetMenu(fileName = "Weapon-", menuName = "Item: Weapon")]
public class ItemDataWeapon : ItemDataTool
{
    [Tooltip("Ammo SO for this weapon")]
    public ItemData Ammo;

    public override ItemCategory category { get { return ItemCategory.Weapons; } }

    /// Override to make sure all weapons have hunt ability, even if it's not setup properly
    public override bool HasAbility(ActionType actionType, out int modifier)
    {
        var parent = base.HasAbility(actionType, out modifier);
        if (actionType == ActionType.Hunt && !parent) {
            Debug.LogWarning("Weapon " + ToString() + " does not have hunt ability. Please add it");
            modifier = 0;
            return true;
        }
        return parent;
    }
}
