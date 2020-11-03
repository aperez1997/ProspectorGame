using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemDataLoader
{
    private static ItemDataLoader _instance = null;
    private static readonly object padlock = new object();
    public static ItemDataLoader Instance
    {
        get {
            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new ItemDataLoader();
                }
                return _instance;
            }
        }
    }

    private readonly Dictionary<ItemType, ItemData> itemDict = new Dictionary<ItemType, ItemData>();

    public ItemDataLoader()
    {
        PopulateList();
    }

    public static ItemData LoadItemByType(ItemType type)
    {
        return Instance.LoadByType(type);
    }


    public ItemData LoadByType(ItemType type)
    {
        bool rv = itemDict.TryGetValue(type, out ItemData item);
        if (!rv) { Debug.LogError("No item SO found for type [" + type + "]"); }
        return item;
    }

    private void PopulateList()
    {
        itemDict.Clear();
        string[] assetNames = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/GameData/Items" });
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var item = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDict.Add(item.type, item);
            //Debug.Log("Found item with type " + item.type);
        }
        Debug.Log("Found " + itemDict.Count + " Items");
    }
}
