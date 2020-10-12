
using UnityEngine;

public class Item
{
    public enum ItemType { Ration }

    public ItemType type;

    public int amount;

    public Sprite GetSprite()
    {
        switch (type)
        {
            case ItemType.Ration:
                return ItemAssets.Instance.rationSprite;
            default:
                return null;
        }
    }
}