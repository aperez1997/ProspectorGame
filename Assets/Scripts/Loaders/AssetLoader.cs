using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Generic class that loads Assets of a given type. Assets must be in a Resources folder
/// TODO: this is not performant. Seek out other solutions eventually.
/// </summary>
/// <typeparam name="Type">The type of asset to load.</typeparam>
public abstract class AssetLoader<Type> where Type : IAssetLoaderItem
{
    protected readonly Dictionary<string, Type> itemDict = new Dictionary<string, Type>();

    public AssetLoader()
    {
        PopulateList();
    }

    public Type LoadByKey(string key)
    {
        bool rv = itemDict.TryGetValue(key, out Type item);
        //if (!rv) { Debug.LogError("No " + typeof(Type).Name +" SO found for key [" + key + "]"); }
        return item;
    }

    protected void PopulateList()
    {
        itemDict.Clear();

        // Load them with a path and not a type
        // The type filtering keeps failing in builds for some reason, so leaving it out for now
        var items = Resources.LoadAll(GetPath()).Cast<Type>();
        foreach (var item in items){
            //Debug.Log("Found item " + item.ToString());
            itemDict.Add(item.GetKey(), item);
        }
        Debug.Log("Found " + itemDict.Count + " items of type " + typeof(Type).Name);
    }

    abstract protected string GetPath();
}
