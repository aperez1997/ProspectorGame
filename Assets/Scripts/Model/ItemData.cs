using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType type;

    public new string name;

    public string description;

    public Sprite sprite;

    // if true, inventory will be a stack instead of single slot per item
    public bool stackable;
}

public enum ItemType { Money, Ration, GoldNugget, Pan }