using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]
public class ItemData : ScriptableObject, IAssetLoaderItem
{
    public ItemId id;


    // TODO: Should this be replaced with sub-classes?
    [Tooltip("Category of item. Determines which stores sell which items")]
    public ItemCategory category;

    public new string name;

    public string description;

    public Sprite sprite;

    [Tooltip("if true, inventory will be a stack instead of single slot per item")]
    public bool stackable;

    [Tooltip("Price the item is sold for in stores. 0 or less means not sold")]
    public int price;

    public string GetKey() { return id.ToString(); }
}

/// <summary>
/// Unique Item identifier. Links to SO
/// </summary>
public enum ItemId { Money, Ration, ForagedFood, Pan, GoldNugget, Bow, Arrow }

/// <summary>
/// Category of item. Used for store filtering
/// </summary>
public enum ItemCategory { None, Food, Tools, Weapons, Ammo, Resources }