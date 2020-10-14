using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public ItemType type;

    public new string name;

    public string description;

    public Sprite sprite;
}

public enum ItemType { Ration = 1, GoldNugget = 2 }