using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    public static ItemLoader Instance;

    public Dictionary<ItemType, Item> itemDict = new Dictionary<ItemType, Item>();

    private void Awake()
    {
        Instance = this;
        PopulateList();
    }

    public static Item LoadItemByType(ItemType type)
    {
        bool rv = Instance.itemDict.TryGetValue(type, out Item item);
        if (!rv){ Debug.LogError("No item SO found for type [" + type + "]"); }
        return item;
    }

    private void PopulateList()
    {
        string[] assetNames = AssetDatabase.FindAssets("t:Item", new[] { "Assets/Items" });
        itemDict.Clear();
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var item = AssetDatabase.LoadAssetAtPath<Item>(SOpath);
            itemDict.Add(item.type, item);
            Debug.Log("Found item with type " + item.type);
        }
        Debug.Log("Found " + itemDict.Count + " Items");
    }
}
