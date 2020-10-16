using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType type;

    public new string name;

    public string description;

    public Sprite sprite;
}

public enum ItemType { Ration = 1, GoldNugget = 2 }