using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Loads ItemData ScriptableObjects
/// </summary>
public class ItemDataLoader : AssetLoader<ItemData>
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

    public static ItemData LoadItemById(string id)
    {
        return Instance.LoadByKey(id);
    }

    public static ItemData[] GetItemsByCategoryStatic(ItemCategory itemCategory)
    {
        return Instance.GetItemsByCategory(itemCategory);
    }

    /// <summary>
    /// Get all items with the given category
    /// TODO: this is not very efficient. Should probably cache this info on init
    /// </summary>
    public ItemData[] GetItemsByCategory(ItemCategory itemCategory)
    {
        var output = new List<ItemData>();
        foreach (var tuple in itemDict) {
            var value = tuple.Value;
            if (value.category == itemCategory) {
                output.Add(value);
            }
        }
        return output.ToArray();
    }

    protected override string GetPath(){ return "GameData/Items"; }
}
