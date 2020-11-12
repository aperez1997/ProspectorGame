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

    private readonly Dictionary<ItemId, ItemData> itemDict = new Dictionary<ItemId, ItemData>();

    public ItemDataLoader()
    {
        PopulateList();
    }

    public static ItemData LoadItemById(ItemId id)
    {
        return Instance.LoadById(id);
    }


    public ItemData LoadById(ItemId id)
    {
        bool rv = itemDict.TryGetValue(id, out ItemData item);
        if (!rv) { Debug.LogError("No item SO found for type [" + id + "]"); }
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
            itemDict.Add(item.id, item);
            //Debug.Log("Found item with type " + item.id);
        }
        Debug.Log("Found " + itemDict.Count + " Items");
    }
}
