using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO for all items in the game
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]
public class ItemData : ScriptableObject, IAssetLoaderItem
{
    public string id;

    public virtual ItemCategory category { get { return ItemCategory.None; } }

    public new string name;

    public string description;

    public Sprite sprite;

    [Tooltip("if true, inventory will be a stack instead of single slot per item")]
    public bool stackable;

    [Tooltip("Price the item is sold for in stores. 0 or less means not sold")]
    public int price;

    public string GetKey() { return id; }

    public GameEvent[] gameEvents;
}

/// <summary>
/// Category of item. Used for store filtering
/// </summary>
public enum ItemCategory { None, Food, Tools, Weapons, Ammo, Resources, Carcass }