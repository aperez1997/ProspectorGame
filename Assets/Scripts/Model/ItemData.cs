﻿using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType type;

    [Tooltip("Category of item. Determines which stores sell which items")]
    public ItemCategory category;

    public new string name;

    public string description;

    public Sprite sprite;

    [Tooltip("if true, inventory will be a stack instead of single slot per item")]
    public bool stackable;

    [Tooltip("Price the item is sold for in stores. 0 or less means not sold")]
    public int price;
}

/// <summary>
/// Unique Item identifier. Links to SO
/// </summary>
public enum ItemType { Money, Ration, Pan, GoldNugget }

/// <summary>
/// Category of item. Used for store filtering
/// </summary>
public enum ItemCategory { None, Food, Tools, Resources }