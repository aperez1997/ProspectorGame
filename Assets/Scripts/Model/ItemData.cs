using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType type;

    public new string name;

    public string description;

    public Sprite sprite;

    [Tooltip("if true, inventory will be a stack instead of single slot per item")]
    public bool stackable;

    [Tooltip("Price the item is sold for in stores. 0 or less means not sold")]
    public int price;
}

public enum ItemType { Money, Ration, GoldNugget, Pan }